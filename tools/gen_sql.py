"""Generates database/create-database.sql from the EDMX storage model + demo seed data."""
import base64, hashlib, os, re, sys

edmx_path, out_path = sys.argv[1], sys.argv[2]
x = open(edmx_path, encoding='utf-8-sig').read()
ssdl = x[x.find('<edmx:StorageModels>'):x.find('</edmx:StorageModels>')]

tables, fks = [], []
for name, body in re.findall(r'<EntityType Name="(\w+)">(.*?)</EntityType>', ssdl, re.S):
    keys = re.findall(r'PropertyRef Name="(\w+)"', body)
    cols = []
    for p in re.findall(r'<Property ([^/]*)/>', body):
        a = dict(re.findall(r'(\w+)="([^"]*)"', p))
        t = a['Type']
        if 'MaxLength' in a:
            t += '(%s)' % a['MaxLength']
        elif 'Precision' in a and 'Scale' in a:
            t += '(%s,%s)' % (a['Precision'], a['Scale'])
        ident = ' IDENTITY(1,1)' if a.get('StoreGeneratedPattern') == 'Identity' else ''
        null = ' NOT NULL' if a.get('Nullable') == 'false' else ' NULL'
        default = ''
        if a.get('Nullable') == 'false' and a['Type'] in ('bit', 'int') and a.get('StoreGeneratedPattern') != 'Identity':
            default = ' CONSTRAINT [DF_%s_%s] DEFAULT %d' % (name, a['Name'], 1 if a['Name'] == 'IsActive' else 0)
        cols.append('    [%s] %s%s%s%s' % (a['Name'], t, ident, null, default))
    tables.append((name, keys, cols))
for an, body in re.findall(r'<Association Name="(\w+)">(.*?)</Association>', ssdl, re.S):
    ends = dict(re.findall(r'<End Role="(\w+)" Type="[\w\.]+\.(\w+)"', body))
    pr = re.search(r'<Principal Role="(\w+)">\s*<PropertyRef Name="(\w+)"', body)
    dp = re.search(r'<Dependent Role="(\w+)">\s*<PropertyRef Name="(\w+)"', body)
    fks.append((an, ends[dp.group(1)], dp.group(2), ends[pr.group(1)], pr.group(2)))

# dependency order for create / reverse for drop
order = ['Users', 'AuditLog', 'Settings', 'Holidays', 'Customers', 'Courts', 'CaseTypes', 'Procedures', 'CourtCaseTypes',
         'CaseTypeSteps', 'Cases', 'CaseSteps', 'Payments']
tables.sort(key=lambda t: order.index(t[0]))


def q(v):
    if v is None:
        return 'NULL'
    if isinstance(v, bool):
        return '1' if v else '0'
    if isinstance(v, (int, float)):
        return str(v)
    if isinstance(v, Day):
        return v.sql()
    return "N'%s'" % str(v).replace("'", "''")


class Day:
    """A date relative to the day the script runs."""
    def __init__(self, offset):
        self.offset = offset

    def sql(self):
        return 'DATEADD(day, %d, @Today)' % self.offset


def pbkdf2(password, salt):
    h = hashlib.pbkdf2_hmac('sha256', password.encode('utf-8'), salt, 100000, 32)
    return 'pbkdf2$100000$%s$%s' % (base64.b64encode(salt).decode(), base64.b64encode(h).decode())


def israeli_id(base8):
    digits = [int(c) for c in base8.zfill(8)]
    total = 0
    for i, d in enumerate(digits):
        k = d * (1 if i % 2 == 0 else 2)
        total += k - 9 if k > 9 else k
    return base8.zfill(8) + str((10 - total % 10) % 10)


def insert(table, cols, rows, identity=True):
    out = []
    if identity:
        out.append('SET IDENTITY_INSERT [dbo].[%s] ON;' % table)
    out.append('INSERT INTO [dbo].[%s] (%s) VALUES' % (table, ', '.join('[%s]' % c for c in cols)))
    out.append(',\n'.join('    (%s)' % ', '.join(q(v) for v in r) for r in rows) + ';')
    if identity:
        out.append('SET IDENTITY_INSERT [dbo].[%s] OFF;' % table)
    return '\n'.join(out) + '\n'


# ------------------------------------------------------------------ seed data (all fictional)
users = [(1, 'admin', pbkdf2('Quedin2024', b'quedin-demo-salt1'), 'מנהל', True, 0, None),
         (2, 'worker', pbkdf2('Worker2024', b'quedin-demo-salt2'), 'משתמש רגיל', True, 0, None)]
settings = [('VatRate', '0.18'), ('DueSoonDays', '7'), ('OfficeName', 'משרד עורכי דין לדוגמה'),
            ('OfficeAddress', 'רחוב הדוגמה 1, ירושלים'), ('OfficePhone', '02-0000000'), ('OfficeTaxId', '')]

courts = [(1, 'בית משפט השלום ירושלים'), (2, 'בית המשפט המחוזי ירושלים'), (3, 'בית משפט השלום תל אביב'),
          (4, 'בית הדין האזורי לעבודה תל אביב'), (5, 'בית משפט השלום חיפה')]

processes = [  # id, name, days, type
    (1, 'פגישת היכרות וחתימה על ייפוי כוח', 7, 'פגישה'),
    (2, 'איסוף מסמכים וראיות', 14, 'הכנה'),
    (3, 'הגשת כתב תביעה', 21, 'הגשה'),
    (4, 'המתנה לכתב הגנה', 30, 'המתנה'),
    (5, 'קדם משפט', 45, 'דיון'),
    (6, 'הגשת תצהירי עדות ראשית', 30, 'הגשה'),
    (7, 'דיון הוכחות', 60, 'דיון'),
    (8, 'הגשת סיכומים', 30, 'הגשה'),
    (9, 'פסק דין', 45, 'החלטה'),
    (10, 'בדיקת נסח טאבו', 3, 'בדיקה'),
    (11, 'ניסוח הסכם מכר', 10, 'ניסוח'),
    (12, 'חתימה על הסכם המכר', 7, 'חתימה'),
    (13, 'רישום הערת אזהרה', 5, 'רישום'),
    (14, 'העברת בעלות בטאבו', 60, 'רישום'),
]
days = {p[0]: p[2] for p in processes}

subjects = [(1, 'תביעה אזרחית', 15000, 1200), (2, 'תביעת נזיקין', 12000, 900),
            (3, 'דיני עבודה', 8000, 0), (4, 'עסקת מקרקעין', 6000, 0)]
plans = {1: [1, 2, 3, 4, 5, 6, 7, 8, 9], 2: [1, 2, 3, 4, 5, 7, 9], 3: [1, 2, 3, 5, 7, 9], 4: [1, 10, 11, 12, 13, 14]}
fixed = []
fid = 1
for sid, steps in plans.items():
    for level, pid in enumerate(steps, 1):
        fixed.append((fid, sid, pid, level))
        fid += 1
links = [(1, 1, 1), (2, 3, 1), (3, 5, 1), (4, 2, 1), (5, 1, 2), (6, 3, 2), (7, 2, 2), (8, 4, 3)]  # id, court, subject

people = [  # first, last, city, street, house, type
    ('דנה', 'לוי', 'ירושלים', 'יפו', '12', 'לקוח'),
    ('יוסף', 'אברהם', 'תל אביב', 'דיזנגוף', '80', 'לקוח'),
    ('מיכל', 'כהן', 'חיפה', 'הנביאים', '5', 'לקוח'),
    ('אבי', 'מזרחי', 'בית שמש', 'נהר הירדן', '21', 'לקוח'),
    ('נועה', 'פרץ', 'מודיעין', 'עמק דותן', '3', 'לקוח'),
    ('שמעון', 'דהן', 'ירושלים', 'עזה', '40', 'לקוח נגדי'),
    ('אורית', 'שגיא', 'רמת גן', 'ביאליק', '17', 'לקוח נגדי'),
    ('גיל', 'רוזן', 'חיפה', 'מוריה', '102', 'לקוח נגדי'),
    ('רחל', 'גולן', 'ירושלים', 'המלך ג׳ורג׳', '30', 'עורך דין נגדי'),
    ('אהרון', 'קליין', 'תל אביב', 'רוטשילד', '45', 'עורך דין נגדי'),
    ('תמר', 'שמש', 'חיפה', 'חורב', '8', 'עורך דין נגדי'),
    ('אלפא נכסים בע"מ', '', 'תל אביב', 'הארבעה', '19', 'לקוח נגדי'),
]
customers = []
ids = []
for i, (fn, ln, city, street, house, kind) in enumerate(people):
    company = ln == ''
    pid = israeli_id(str(51234567 + i)) if company else israeli_id(str(30000000 + i * 1234567))
    ids.append(pid)
    customers.append((pid, fn, ln, city, street, house, None, '02%07d' % (6000000 + i * 1111), '05%08d' % (21234567 + i * 1010),
                      'demo%d@example.com' % (i + 1), '1980-01-%02d' % (i + 1) if kind == 'לקוח' else None, kind,
                      'ח.פ.' if company else 'ת.ז.'))
C, OC, OL = ids[0:5], ids[5:8] + [ids[11]], ids[8:11]

# cases: id, client, subject, court(None = deal), opened days ago, steps done, ipui, claim, percent, mikdama, opp client, opp lawyer, deal fields
cases_spec = [
    (1, C[0], 1, 1, 160, 4, 'נחתם 02/06/2026', 85000, 20, 3000, OC[0], OL[0], None),
    (2, C[1], 3, 4, 50, 2, 'נחתם 20/09/2026', 42000, 25, 2000, OC[1], OL[1], None),
    (3, C[2], 2, 5, 35, 1, '', 120000, 18, 5000, OC[2], OL[2], None),
    (4, C[3], 4, None, 20, 3, 'נחתם 23/10/2026', 1850000, 0.5, 1500, OC[3], OL[0], ('נהר הירדן 21, בית שמש', '5432', '17', '4')),
    (5, C[4], 1, 3, 400, 9, 'נחתם 05/10/2025', 60000, 25, 2500, OC[1], OL[1], None),
]
cases, steps_rows, payments = [], [], []
sid_counter, pay_counter = 1, 1
for (cid, client, subj, court, opened, done, ipui, claim, pct, mikdama, oc, ol, deal) in cases_spec:
    plan = plans[subj]
    cursor = -opened
    rows = []
    for k, pid in enumerate(plan):
        cursor += days[pid]
        rows.append([sid_counter, cid, pid, cursor, None, False, days[pid]])
        sid_counter += 1
    # complete the first `done` steps a little late/early, then push the rest (same rule as CaseScheduler)
    for k in range(min(done, len(rows))):
        actual = rows[k][3] + (3 if k % 2 == 0 else -2)
        actual = min(actual, 0)
        rows[k][4] = actual
        rows[k][5] = True
        c = actual
        for j in range(k + 1, len(rows)):
            c += days[rows[j][2]]
            rows[j][3] = c
    pending = [r for r in rows if not r[5]]
    status = pending[0][2] if pending else rows[-1][2]
    subject_row = [s for s in subjects if s[0] == subj][0]
    agra, minimum_fee = subject_row[3], subject_row[2]
    total = round(max(claim * pct / 100, minimum_fee) + agra, 2)  # same rule as FeeCalculator.CaseTotal
    paid = []
    if cid in (1, 4, 5):
        amounts = {1: [3000, 5000], 4: [1500], 5: [total]}[cid]
        bal = total
        for n, amt in enumerate(amounts):
            amt = min(amt, bal)
            bal = round(bal - amt, 2)
            method = ['מזומן', 'העברה בנקאית', 'אשראי'][n % 3] if cid != 5 else 'צ\'ק'
            payments.append((pay_counter, cid, bal, method, 'שכר טרחה', 0.18, False, None,
                             '****4417' if method == 'אשראי' else None,
                             '1000%03d' % pay_counter if method == 'צ\'ק' else None,
                             '12' if method == 'צ\'ק' else None, '345' if method == 'צ\'ק' else None,
                             '987654' if method == 'צ\'ק' else None, amt, Day(-opened + 10 + n * 20),
                             'מקדמה' if n == 0 else 'תשלום שכר טרחה',
                             'TR-%05d' % pay_counter if method == 'העברה בנקאית' else None,
                             Day(-opened + 10) if method == 'צ\'ק' else None))
            pay_counter += 1
        paid_all = bal == 0
    else:
        paid_all = False
    cases.append((cid, client, subj, court is not None, court, status, ipui, paid_all, mikdama, cid in (1, 4, 5),
                  deal[1] if deal else None, deal[2] if deal else None, deal[3] if deal else None, deal[0] if deal else None,
                  claim, 65000 if cid == 5 else None, pct, ol, oc))
    for r in rows:
        fixed_date = cid == 1 and r[2] == 5 and not r[5]  # a hearing date set by the court
        note = 'הדיון נקבע בהחלטת בית המשפט' if fixed_date else None
        steps_rows.append((r[0], r[1], r[2], Day(r[3]), Day(r[4]) if r[4] is not None else None, r[5], r[6], note, fixed_date))

# ------------------------------------------------------------------ write
o = []
w = o.append
w('''/*
    QUEDIN - database script
    -------------------------------------------------------------
    Creates the QuedinDB database on SQL Server LocalDB, builds the schema
    and loads FICTIONAL demo data (people, IDs, phones and cases are invented).

    Run from Visual Studio (SQL Server Object Explorer) or:
        sqlcmd -S "(localdb)\\MSSQLLocalDB" -i database\\create-database.sql

    Demo users:  admin / Quedin2024   (manager)
                 worker / Worker2024  (regular user)

    Generated from LawyerProject/DB/QuedinModel.edmx. Re-running the script
    drops and recreates all tables.
*/
SET NOCOUNT ON;
GO
IF DB_ID(N'QuedinDB') IS NULL
    CREATE DATABASE [QuedinDB];
GO
USE [QuedinDB];
GO

-- ---------------------------------------------------------- drop (child tables first)
''')
for proc in ('usp_ChangeOwnPassword', 'usp_TryLogin', 'usp_GetLoginSalt'):
    w("IF OBJECT_ID(N'dbo.%s', N'P') IS NOT NULL DROP PROCEDURE [dbo].[%s];\n" % (proc, proc))
for name, _, _ in reversed(tables):
    w("IF OBJECT_ID(N'dbo.%s', N'U') IS NOT NULL DROP TABLE [dbo].[%s];\n" % (name, name))
w('GO\n\n-- ---------------------------------------------------------- tables\n')
for name, keys, cols in tables:
    w('CREATE TABLE [dbo].[%s] (\n%s,\n    CONSTRAINT [PK_%s] PRIMARY KEY CLUSTERED (%s)\n);\n' %
      (name, ',\n'.join(cols), name, ', '.join('[%s]' % k for k in keys)))
w('GO\n\n-- ---------------------------------------------------------- relationships\n')
for an, dt, dc, pt, pc in fks:
    w('ALTER TABLE [dbo].[%s] ADD CONSTRAINT [%s] FOREIGN KEY ([%s]) REFERENCES [dbo].[%s] ([%s]);\n' % (dt, an, dc, pt, pc))
w('''ALTER TABLE [dbo].[Users] ADD CONSTRAINT [UQ_Users_UserName] UNIQUE ([UserName]);
ALTER TABLE [dbo].[CaseTypeSteps] ADD CONSTRAINT [UQ_CaseTypeSteps_Order] UNIQUE ([CaseTypeId], [StepOrder]);
CREATE INDEX [IX_CaseSteps_CaseId] ON [dbo].[CaseSteps] ([CaseId]);
CREATE INDEX [IX_Payments_CaseId] ON [dbo].[Payments] ([CaseId]);
CREATE INDEX [IX_Cases_ClientId] ON [dbo].[Cases] ([ClientId]);
-- Links the EF model does not navigate, but the database still guarantees.
ALTER TABLE [dbo].[Cases] ADD CONSTRAINT [FK_Cases_Customers_OpposingParty] FOREIGN KEY ([OpposingPartyId]) REFERENCES [dbo].[Customers] ([CustomerId]);
ALTER TABLE [dbo].[Cases] ADD CONSTRAINT [FK_Cases_Customers_OpposingLawyer] FOREIGN KEY ([OpposingLawyerId]) REFERENCES [dbo].[Customers] ([CustomerId]);
ALTER TABLE [dbo].[Payments] ADD CONSTRAINT [FK_Payments_CancelsPayment] FOREIGN KEY ([CancelsPaymentId]) REFERENCES [dbo].[Payments] ([PaymentId]);
-- A payment can be cancelled only once.
CREATE UNIQUE INDEX [UQ_Payments_CancelsPaymentId] ON [dbo].[Payments] ([CancelsPaymentId]) WHERE [CancelsPaymentId] IS NOT NULL;
-- The same rules the business layer checks, kept by the database too.
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [CK_Users_Role] CHECK ([Role] IN (N'מנהל', N'משתמש רגיל'));
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [CK_Users_FailedLogins] CHECK ([FailedLogins] >= 0);
ALTER TABLE [dbo].[CaseTypes] ADD CONSTRAINT [CK_CaseTypes_Fees] CHECK (ISNULL([BaseFee], 0) >= 0 AND ISNULL([CourtFee], 0) >= 0);
ALTER TABLE [dbo].[Procedures] ADD CONSTRAINT [CK_Procedures_DurationDays] CHECK ([DurationDays] >= 0);
ALTER TABLE [dbo].[Cases] ADD CONSTRAINT [CK_Cases_Amounts] CHECK ([Advance] >= 0 AND ISNULL([ClaimAmount], 0) >= 0 AND ISNULL([AwardedAmount], 0) >= 0);
ALTER TABLE [dbo].[Cases] ADD CONSTRAINT [CK_Cases_FeePercent] CHECK ([FeePercent] IS NULL OR [FeePercent] BETWEEN 0 AND 100);
ALTER TABLE [dbo].[Cases] ADD CONSTRAINT [CK_Cases_NotOwnOpponent] CHECK ([OpposingPartyId] <> [ClientId]);
ALTER TABLE [dbo].[Payments] ADD CONSTRAINT [CK_Payments_Amount] CHECK ([Amount] <> 0);
ALTER TABLE [dbo].[Payments] ADD CONSTRAINT [CK_Payments_VatRate] CHECK ([VatRate] IS NULL OR [VatRate] BETWEEN 0 AND 1);
GO

-- The audit log is append-only: nobody can change or delete what was recorded.
CREATE TRIGGER [dbo].[TR_AuditLog_AppendOnly] ON [dbo].[AuditLog]
INSTEAD OF UPDATE, DELETE
AS
BEGIN
    -- INSTEAD OF: the change itself never runs; the caller gets an error.
    RAISERROR (N'The audit log is append-only.', 16, 1);
END
GO

-- A payment is a receipt: once written, only "cancelled" may change (from 0 to 1).
-- Corrections are made with a reversing entry, never by editing the original.
CREATE TRIGGER [dbo].[TR_Payments_Immutable] ON [dbo].[Payments]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1 FROM inserted i JOIN deleted d ON d.[PaymentId] = i.[PaymentId]
        WHERE i.[CaseId] <> d.[CaseId] OR i.[Amount] <> d.[Amount] OR i.[PaidOn] <> d.[PaidOn]
           OR i.[Method] <> d.[Method] OR ISNULL(i.[Kind], N'') <> ISNULL(d.[Kind], N'')
           OR i.[BalanceAfter] <> d.[BalanceAfter] OR ISNULL(i.[VatRate], -1) <> ISNULL(d.[VatRate], -1)
           OR ISNULL(i.[CancelsPaymentId], -1) <> ISNULL(d.[CancelsPaymentId], -1)
           OR (d.[IsCancelled] = 1 AND i.[IsCancelled] = 0))
    BEGIN
        RAISERROR (N'A payment cannot be edited. Cancel it with a reversing entry instead.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END
GO

-- ---------------------------------------------------------- login procedures
-- Password hashes never leave the database: the app asks for the salt, hashes the typed password
-- with PBKDF2 and the database compares. With security.sql, employees cannot read or change the
-- Users table directly - only through these procedures (EXECUTE AS OWNER).

CREATE PROCEDURE [dbo].[usp_GetLoginSalt] @UserName nvarchar(50)
WITH EXECUTE AS OWNER
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @stored nvarchar(200) = (SELECT [PasswordHash] FROM [dbo].[Users] WHERE [UserName] = @UserName);
    IF @stored IS NULL
    BEGIN
        -- An unknown user gets a stable made-up salt, so the answer does not reveal which names exist.
        DECLARE @fake varbinary(16) = CAST(HASHBYTES('SHA2_256', N'quedin-unknown-user:' + LOWER(@UserName)) AS varbinary(16));
        SELECT N'pbkdf2$100000$' + CAST(CAST('' AS xml).value('xs:base64Binary(sql:variable("@fake"))', 'varchar(64)') AS nvarchar(64)) + N'$' AS [Salt];
        RETURN;
    END
    IF @stored NOT LIKE N'pbkdf2$%'
    BEGIN
        SELECT N'legacy$' AS [Salt]; -- a password stored as plain text by the 2024 version
        RETURN;
    END
    -- "pbkdf2$<iterations>$<salt>$" : everything up to the last '$'
    SELECT LEFT(@stored, LEN(@stored) - CHARINDEX(N'$', REVERSE(@stored)) + 1) AS [Salt];
END
GO

-- Result: 0 = success, 1 = wrong password, 2 = locked, 3 = deactivated, 4 = unknown user,
--         5 = wrong password and now locked. 5 wrong passwords lock the user for 15 minutes
--         (the same numbers as LoginPolicy in the app).
CREATE PROCEDURE [dbo].[usp_TryLogin] @UserName nvarchar(50), @Candidate nvarchar(200), @UpgradeHash nvarchar(200) = NULL
WITH EXECUTE AS OWNER
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @now datetime = SYSDATETIME();
    DECLARE @id int, @stored nvarchar(200), @active bit, @fails int, @locked datetime, @role nvarchar(50);
    SELECT @id = [UserId], @stored = [PasswordHash], @active = [IsActive], @fails = [FailedLogins],
           @locked = [LockedUntil], @role = [Role]
    FROM [dbo].[Users] WHERE [UserName] = @UserName;

    IF @id IS NULL
    BEGIN
        SELECT 4 AS [Result], CAST(NULL AS int) AS [UserId], CAST(NULL AS nvarchar(50)) AS [Role], CAST(NULL AS datetime) AS [LockedUntil];
        RETURN;
    END
    IF @locked > @now
    BEGIN
        SELECT 2 AS [Result], CAST(NULL AS int) AS [UserId], CAST(NULL AS nvarchar(50)) AS [Role], @locked AS [LockedUntil];
        RETURN;
    END

    -- Hashes are base64 and therefore case-sensitive: compare with a binary collation.
    IF @Candidate IS NULL OR @stored COLLATE Latin1_General_BIN2 <> @Candidate COLLATE Latin1_General_BIN2
    BEGIN
        SET @fails = @fails + 1;
        IF @fails >= 5
        BEGIN
            UPDATE [dbo].[Users] SET [FailedLogins] = 0, [LockedUntil] = DATEADD(minute, 15, @now) WHERE [UserId] = @id;
            SELECT 5 AS [Result], CAST(NULL AS int) AS [UserId], CAST(NULL AS nvarchar(50)) AS [Role], DATEADD(minute, 15, @now) AS [LockedUntil];
        END
        ELSE
        BEGIN
            UPDATE [dbo].[Users] SET [FailedLogins] = @fails WHERE [UserId] = @id;
            SELECT 1 AS [Result], CAST(NULL AS int) AS [UserId], CAST(NULL AS nvarchar(50)) AS [Role], CAST(NULL AS datetime) AS [LockedUntil];
        END
        RETURN;
    END

    UPDATE [dbo].[Users]
    SET [FailedLogins] = 0, [LockedUntil] = NULL,
        [PasswordHash] = CASE WHEN [PasswordHash] NOT LIKE N'pbkdf2$%' AND @UpgradeHash LIKE N'pbkdf2$%' THEN @UpgradeHash ELSE [PasswordHash] END
    WHERE [UserId] = @id;
    -- Only someone who knows the password learns that the user was deactivated.
    SELECT CASE WHEN @active = 1 THEN 0 ELSE 3 END AS [Result], @id AS [UserId], @role AS [Role], CAST(NULL AS datetime) AS [LockedUntil];
END
GO

-- Changes a password only when the current one (hashed with the stored salt) matches.
CREATE PROCEDURE [dbo].[usp_ChangeOwnPassword] @UserName nvarchar(50), @OldCandidate nvarchar(200), @NewHash nvarchar(200)
WITH EXECUTE AS OWNER
AS
BEGIN
    SET NOCOUNT ON;
    IF @NewHash IS NULL OR @NewHash NOT LIKE N'pbkdf2$%'
    BEGIN
        SELECT 0 AS [Changed];
        RETURN;
    END
    UPDATE [dbo].[Users] SET [PasswordHash] = @NewHash
    WHERE [UserName] = @UserName AND [IsActive] = 1
      AND [PasswordHash] COLLATE Latin1_General_BIN2 = @OldCandidate COLLATE Latin1_General_BIN2;
    SELECT @@ROWCOUNT AS [Changed];
END
GO

-- ---------------------------------------------------------- demo data (fictional)
DECLARE @Today date = CAST(GETDATE() AS date);

''')
w(insert('Users', ['UserId', 'UserName', 'PasswordHash', 'Role', 'IsActive', 'FailedLogins', 'LockedUntil'], users))
w(insert('Settings', ['SettingKey', 'Value'], settings, identity=False))
w(insert('Courts', ['CourtId', 'Name'], courts))
w(insert('CaseTypes', ['CaseTypeId', 'Name', 'BaseFee', 'CourtFee'], subjects))
w(insert('Procedures', ['ProcedureId', 'Name', 'DurationDays', 'Category'], processes))
w(insert('CourtCaseTypes', ['CourtCaseTypeId', 'CourtId', 'CaseTypeId'], links))
w(insert('CaseTypeSteps', ['CaseTypeStepId', 'CaseTypeId', 'ProcedureId', 'StepOrder'], fixed))
w(insert('Customers', ['CustomerId', 'FirstName', 'LastName', 'City', 'Street', 'HouseNumber', 'PostalCode', 'Phone', 'MobilePhone',
                       'Email', 'BirthDate', 'PersonType', 'IdType'], customers, identity=False))
w(insert('Cases', ['CaseId', 'ClientId', 'CaseTypeId', 'IsInCourt', 'CourtId', 'CurrentProcedureId', 'PowerOfAttorney', 'IsPaid',
                   'Advance', 'IsAdvancePaid', 'Block', 'Parcel', 'SubParcel', 'PropertyAddress', 'ClaimAmount', 'AwardedAmount',
                   'FeePercent', 'OpposingLawyerId', 'OpposingPartyId'], cases))
w(insert('CaseSteps', ['CaseStepId', 'CaseId', 'ProcedureId', 'DueDate', 'DoneDate', 'IsDone', 'DurationDays', 'Notes', 'IsFixedDate'], steps_rows))
w(insert('Payments', ['PaymentId', 'CaseId', 'BalanceAfter', 'Method', 'Kind', 'VatRate', 'IsCancelled', 'CancelsPaymentId', 'CardLast4',
                      'ChequeNumber', 'BankNumber', 'BranchNumber', 'AccountNumber', 'Amount', 'PaidOn', 'Description', 'Reference',
                      'ChequeDate'], payments))
w("GO\n\nPRINT N'QuedinDB is ready. Log in with admin / Quedin2024';\nGO\n")

os.makedirs(os.path.dirname(out_path), exist_ok=True)
open(out_path, 'w', encoding='utf-8-sig').write(''.join(o))
print('ok', len(''.join(o).splitlines()), 'lines;', len(cases), 'cases,', len(steps_rows), 'steps,', len(payments), 'payments')
