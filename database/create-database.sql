/*
    QUEDIN - database script
    -------------------------------------------------------------
    Creates the QuedinDB database on SQL Server LocalDB, builds the schema
    and loads FICTIONAL demo data (people, IDs, phones and cases are invented).

    Run from Visual Studio (SQL Server Object Explorer) or:
        sqlcmd -S "(localdb)\MSSQLLocalDB" -i database\create-database.sql

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
IF OBJECT_ID(N'dbo.usp_ChangeOwnPassword', N'P') IS NOT NULL DROP PROCEDURE [dbo].[usp_ChangeOwnPassword];
IF OBJECT_ID(N'dbo.usp_TryLogin', N'P') IS NOT NULL DROP PROCEDURE [dbo].[usp_TryLogin];
IF OBJECT_ID(N'dbo.usp_GetLoginSalt', N'P') IS NOT NULL DROP PROCEDURE [dbo].[usp_GetLoginSalt];
IF OBJECT_ID(N'dbo.Payments', N'U') IS NOT NULL DROP TABLE [dbo].[Payments];
IF OBJECT_ID(N'dbo.CaseSteps', N'U') IS NOT NULL DROP TABLE [dbo].[CaseSteps];
IF OBJECT_ID(N'dbo.Cases', N'U') IS NOT NULL DROP TABLE [dbo].[Cases];
IF OBJECT_ID(N'dbo.CaseTypeSteps', N'U') IS NOT NULL DROP TABLE [dbo].[CaseTypeSteps];
IF OBJECT_ID(N'dbo.CourtCaseTypes', N'U') IS NOT NULL DROP TABLE [dbo].[CourtCaseTypes];
IF OBJECT_ID(N'dbo.Procedures', N'U') IS NOT NULL DROP TABLE [dbo].[Procedures];
IF OBJECT_ID(N'dbo.CaseTypes', N'U') IS NOT NULL DROP TABLE [dbo].[CaseTypes];
IF OBJECT_ID(N'dbo.Courts', N'U') IS NOT NULL DROP TABLE [dbo].[Courts];
IF OBJECT_ID(N'dbo.Customers', N'U') IS NOT NULL DROP TABLE [dbo].[Customers];
IF OBJECT_ID(N'dbo.Holidays', N'U') IS NOT NULL DROP TABLE [dbo].[Holidays];
IF OBJECT_ID(N'dbo.Settings', N'U') IS NOT NULL DROP TABLE [dbo].[Settings];
IF OBJECT_ID(N'dbo.AuditLog', N'U') IS NOT NULL DROP TABLE [dbo].[AuditLog];
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL DROP TABLE [dbo].[Users];
GO

-- ---------------------------------------------------------- tables
CREATE TABLE [dbo].[Users] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(50) NOT NULL,
    [PasswordHash] nvarchar(200) NOT NULL,
    [Role] nvarchar(50) NOT NULL,
    [IsActive] bit NOT NULL CONSTRAINT [DF_Users_IsActive] DEFAULT 1,
    [FailedLogins] int NOT NULL CONSTRAINT [DF_Users_FailedLogins] DEFAULT 0,
    [LockedUntil] datetime NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId])
);
CREATE TABLE [dbo].[AuditLog] (
    [AuditEntryId] int IDENTITY(1,1) NOT NULL,
    [OccurredAt] datetime NOT NULL,
    [UserName] nvarchar(50) NULL,
    [Action] nvarchar(50) NOT NULL,
    [Entity] nvarchar(50) NULL,
    [EntityId] nvarchar(50) NULL,
    [Details] nvarchar(400) NULL,
    CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([AuditEntryId])
);
CREATE TABLE [dbo].[Settings] (
    [SettingKey] nvarchar(50) NOT NULL,
    [Value] nvarchar(200) NULL,
    CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED ([SettingKey])
);
CREATE TABLE [dbo].[Holidays] (
    [HolidayDate] date NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Holidays] PRIMARY KEY CLUSTERED ([HolidayDate])
);
CREATE TABLE [dbo].[Customers] (
    [CustomerId] nvarchar(20) NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(25) NOT NULL,
    [City] nvarchar(50) NULL,
    [Street] nvarchar(50) NULL,
    [HouseNumber] nvarchar(5) NULL,
    [PostalCode] nvarchar(7) NULL,
    [Phone] nvarchar(20) NULL,
    [MobilePhone] nvarchar(20) NULL,
    [Email] nvarchar(50) NULL,
    [BirthDate] date NULL,
    [PersonType] nvarchar(50) NOT NULL,
    [IdType] nvarchar(20) NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([CustomerId])
);
CREATE TABLE [dbo].[Courts] (
    [CourtId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Courts] PRIMARY KEY CLUSTERED ([CourtId])
);
CREATE TABLE [dbo].[CaseTypes] (
    [CaseTypeId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [BaseFee] money NULL,
    [CourtFee] money NULL,
    CONSTRAINT [PK_CaseTypes] PRIMARY KEY CLUSTERED ([CaseTypeId])
);
CREATE TABLE [dbo].[Procedures] (
    [ProcedureId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [DurationDays] int NOT NULL CONSTRAINT [DF_Procedures_DurationDays] DEFAULT 0,
    [Category] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Procedures] PRIMARY KEY CLUSTERED ([ProcedureId])
);
CREATE TABLE [dbo].[CourtCaseTypes] (
    [CourtCaseTypeId] int IDENTITY(1,1) NOT NULL,
    [CourtId] int NOT NULL CONSTRAINT [DF_CourtCaseTypes_CourtId] DEFAULT 0,
    [CaseTypeId] int NOT NULL CONSTRAINT [DF_CourtCaseTypes_CaseTypeId] DEFAULT 0,
    CONSTRAINT [PK_CourtCaseTypes] PRIMARY KEY CLUSTERED ([CourtCaseTypeId])
);
CREATE TABLE [dbo].[CaseTypeSteps] (
    [CaseTypeStepId] int IDENTITY(1,1) NOT NULL,
    [CaseTypeId] int NOT NULL CONSTRAINT [DF_CaseTypeSteps_CaseTypeId] DEFAULT 0,
    [ProcedureId] int NOT NULL CONSTRAINT [DF_CaseTypeSteps_ProcedureId] DEFAULT 0,
    [StepOrder] int NOT NULL CONSTRAINT [DF_CaseTypeSteps_StepOrder] DEFAULT 0,
    CONSTRAINT [PK_CaseTypeSteps] PRIMARY KEY CLUSTERED ([CaseTypeStepId])
);
CREATE TABLE [dbo].[Cases] (
    [CaseId] int IDENTITY(1,1) NOT NULL,
    [ClientId] nvarchar(20) NOT NULL,
    [CaseTypeId] int NOT NULL CONSTRAINT [DF_Cases_CaseTypeId] DEFAULT 0,
    [IsInCourt] bit NULL,
    [CourtId] int NULL,
    [CurrentProcedureId] int NOT NULL CONSTRAINT [DF_Cases_CurrentProcedureId] DEFAULT 0,
    [PowerOfAttorney] nvarchar(50) NOT NULL,
    [IsPaid] bit NOT NULL CONSTRAINT [DF_Cases_IsPaid] DEFAULT 0,
    [Advance] money NOT NULL,
    [IsAdvancePaid] bit NULL,
    [Block] nvarchar(50) NULL,
    [Parcel] nvarchar(50) NULL,
    [SubParcel] nvarchar(50) NULL,
    [PropertyAddress] nvarchar(50) NULL,
    [ClaimAmount] money NULL,
    [AwardedAmount] money NULL,
    [FeePercent] float NULL,
    [OpposingLawyerId] nvarchar(20) NOT NULL,
    [OpposingPartyId] nvarchar(20) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Cases] PRIMARY KEY CLUSTERED ([CaseId])
);
CREATE TABLE [dbo].[CaseSteps] (
    [CaseStepId] int IDENTITY(1,1) NOT NULL,
    [CaseId] int NOT NULL CONSTRAINT [DF_CaseSteps_CaseId] DEFAULT 0,
    [ProcedureId] int NOT NULL CONSTRAINT [DF_CaseSteps_ProcedureId] DEFAULT 0,
    [DueDate] date NULL,
    [DoneDate] date NULL,
    [IsDone] bit NULL,
    [Notes] nvarchar(200) NULL,
    [DurationDays] int NULL,
    [IsFixedDate] bit NOT NULL CONSTRAINT [DF_CaseSteps_IsFixedDate] DEFAULT 0,
    CONSTRAINT [PK_CaseSteps] PRIMARY KEY CLUSTERED ([CaseStepId])
);
CREATE TABLE [dbo].[Payments] (
    [PaymentId] int IDENTITY(1,1) NOT NULL,
    [CaseId] int NOT NULL CONSTRAINT [DF_Payments_CaseId] DEFAULT 0,
    [BalanceAfter] money NOT NULL,
    [Method] nvarchar(50) NOT NULL,
    [CardLast4] nvarchar(8) NULL,
    [ChequeNumber] nchar(10) NULL,
    [BankNumber] nchar(3) NULL,
    [BranchNumber] nchar(3) NULL,
    [AccountNumber] nvarchar(50) NULL,
    [Amount] money NOT NULL,
    [PaidOn] date NOT NULL,
    [Description] nvarchar(50) NULL,
    [Reference] nvarchar(50) NULL,
    [ChequeDate] date NULL,
    [Kind] nvarchar(20) NULL,
    [VatRate] decimal(5,4) NULL,
    [IsCancelled] bit NOT NULL CONSTRAINT [DF_Payments_IsCancelled] DEFAULT 0,
    [CancelsPaymentId] int NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED ([PaymentId])
);
GO

-- ---------------------------------------------------------- relationships
ALTER TABLE [dbo].[CourtCaseTypes] ADD CONSTRAINT [FK_CourtCaseTypes_Courts] FOREIGN KEY ([CourtId]) REFERENCES [dbo].[Courts] ([CourtId]);
ALTER TABLE [dbo].[CourtCaseTypes] ADD CONSTRAINT [FK_CourtCaseTypes_CaseTypes] FOREIGN KEY ([CaseTypeId]) REFERENCES [dbo].[CaseTypes] ([CaseTypeId]);
ALTER TABLE [dbo].[CaseTypeSteps] ADD CONSTRAINT [FK_CaseTypeSteps_CaseTypes] FOREIGN KEY ([CaseTypeId]) REFERENCES [dbo].[CaseTypes] ([CaseTypeId]);
ALTER TABLE [dbo].[CaseTypeSteps] ADD CONSTRAINT [FK_CaseTypeSteps_Procedures] FOREIGN KEY ([ProcedureId]) REFERENCES [dbo].[Procedures] ([ProcedureId]);
ALTER TABLE [dbo].[CaseSteps] ADD CONSTRAINT [FK_CaseSteps_Cases] FOREIGN KEY ([CaseId]) REFERENCES [dbo].[Cases] ([CaseId]);
ALTER TABLE [dbo].[CaseSteps] ADD CONSTRAINT [FK_CaseSteps_Procedures] FOREIGN KEY ([ProcedureId]) REFERENCES [dbo].[Procedures] ([ProcedureId]);
ALTER TABLE [dbo].[Payments] ADD CONSTRAINT [FK_Payments_Cases] FOREIGN KEY ([CaseId]) REFERENCES [dbo].[Cases] ([CaseId]);
ALTER TABLE [dbo].[Cases] ADD CONSTRAINT [FK_Cases_CaseTypes] FOREIGN KEY ([CaseTypeId]) REFERENCES [dbo].[CaseTypes] ([CaseTypeId]);
ALTER TABLE [dbo].[Cases] ADD CONSTRAINT [FK_Cases_Courts] FOREIGN KEY ([CourtId]) REFERENCES [dbo].[Courts] ([CourtId]);
ALTER TABLE [dbo].[Cases] ADD CONSTRAINT [FK_Cases_Procedures_Current] FOREIGN KEY ([CurrentProcedureId]) REFERENCES [dbo].[Procedures] ([ProcedureId]);
ALTER TABLE [dbo].[Cases] ADD CONSTRAINT [FK_Cases_Customers] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Customers] ([CustomerId]);
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [UQ_Users_UserName] UNIQUE ([UserName]);
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

SET IDENTITY_INSERT [dbo].[Users] ON;
INSERT INTO [dbo].[Users] ([UserId], [UserName], [PasswordHash], [Role], [IsActive], [FailedLogins], [LockedUntil]) VALUES
    (1, N'admin', N'pbkdf2$100000$cXVlZGluLWRlbW8tc2FsdDE=$Kmr3WtJMlhRlHwEsl64u3hXYgOI4X3NMPBL5JyRKBa4=', N'מנהל', 1, 0, NULL),
    (2, N'worker', N'pbkdf2$100000$cXVlZGluLWRlbW8tc2FsdDI=$D4bLJ4IlgXxnzMguYNwv+DaETNIhWIS73LeJBgkOPsA=', N'משתמש רגיל', 1, 0, NULL);
SET IDENTITY_INSERT [dbo].[Users] OFF;
INSERT INTO [dbo].[Settings] ([SettingKey], [Value]) VALUES
    (N'VatRate', N'0.18'),
    (N'DueSoonDays', N'7'),
    (N'OfficeName', N'משרד עורכי דין לדוגמה'),
    (N'OfficeAddress', N'רחוב הדוגמה 1, ירושלים'),
    (N'OfficePhone', N'02-0000000'),
    (N'OfficeTaxId', N'');
SET IDENTITY_INSERT [dbo].[Courts] ON;
INSERT INTO [dbo].[Courts] ([CourtId], [Name]) VALUES
    (1, N'בית משפט השלום ירושלים'),
    (2, N'בית המשפט המחוזי ירושלים'),
    (3, N'בית משפט השלום תל אביב'),
    (4, N'בית הדין האזורי לעבודה תל אביב'),
    (5, N'בית משפט השלום חיפה');
SET IDENTITY_INSERT [dbo].[Courts] OFF;
SET IDENTITY_INSERT [dbo].[CaseTypes] ON;
INSERT INTO [dbo].[CaseTypes] ([CaseTypeId], [Name], [BaseFee], [CourtFee]) VALUES
    (1, N'תביעה אזרחית', 15000, 1200),
    (2, N'תביעת נזיקין', 12000, 900),
    (3, N'דיני עבודה', 8000, 0),
    (4, N'עסקת מקרקעין', 6000, 0);
SET IDENTITY_INSERT [dbo].[CaseTypes] OFF;
SET IDENTITY_INSERT [dbo].[Procedures] ON;
INSERT INTO [dbo].[Procedures] ([ProcedureId], [Name], [DurationDays], [Category]) VALUES
    (1, N'פגישת היכרות וחתימה על ייפוי כוח', 7, N'פגישה'),
    (2, N'איסוף מסמכים וראיות', 14, N'הכנה'),
    (3, N'הגשת כתב תביעה', 21, N'הגשה'),
    (4, N'המתנה לכתב הגנה', 30, N'המתנה'),
    (5, N'קדם משפט', 45, N'דיון'),
    (6, N'הגשת תצהירי עדות ראשית', 30, N'הגשה'),
    (7, N'דיון הוכחות', 60, N'דיון'),
    (8, N'הגשת סיכומים', 30, N'הגשה'),
    (9, N'פסק דין', 45, N'החלטה'),
    (10, N'בדיקת נסח טאבו', 3, N'בדיקה'),
    (11, N'ניסוח הסכם מכר', 10, N'ניסוח'),
    (12, N'חתימה על הסכם המכר', 7, N'חתימה'),
    (13, N'רישום הערת אזהרה', 5, N'רישום'),
    (14, N'העברת בעלות בטאבו', 60, N'רישום');
SET IDENTITY_INSERT [dbo].[Procedures] OFF;
SET IDENTITY_INSERT [dbo].[CourtCaseTypes] ON;
INSERT INTO [dbo].[CourtCaseTypes] ([CourtCaseTypeId], [CourtId], [CaseTypeId]) VALUES
    (1, 1, 1),
    (2, 3, 1),
    (3, 5, 1),
    (4, 2, 1),
    (5, 1, 2),
    (6, 3, 2),
    (7, 2, 2),
    (8, 4, 3);
SET IDENTITY_INSERT [dbo].[CourtCaseTypes] OFF;
SET IDENTITY_INSERT [dbo].[CaseTypeSteps] ON;
INSERT INTO [dbo].[CaseTypeSteps] ([CaseTypeStepId], [CaseTypeId], [ProcedureId], [StepOrder]) VALUES
    (1, 1, 1, 1),
    (2, 1, 2, 2),
    (3, 1, 3, 3),
    (4, 1, 4, 4),
    (5, 1, 5, 5),
    (6, 1, 6, 6),
    (7, 1, 7, 7),
    (8, 1, 8, 8),
    (9, 1, 9, 9),
    (10, 2, 1, 1),
    (11, 2, 2, 2),
    (12, 2, 3, 3),
    (13, 2, 4, 4),
    (14, 2, 5, 5),
    (15, 2, 7, 6),
    (16, 2, 9, 7),
    (17, 3, 1, 1),
    (18, 3, 2, 2),
    (19, 3, 3, 3),
    (20, 3, 5, 4),
    (21, 3, 7, 5),
    (22, 3, 9, 6),
    (23, 4, 1, 1),
    (24, 4, 10, 2),
    (25, 4, 11, 3),
    (26, 4, 12, 4),
    (27, 4, 13, 5),
    (28, 4, 14, 6);
SET IDENTITY_INSERT [dbo].[CaseTypeSteps] OFF;
INSERT INTO [dbo].[Customers] ([CustomerId], [FirstName], [LastName], [City], [Street], [HouseNumber], [PostalCode], [Phone], [MobilePhone], [Email], [BirthDate], [PersonType], [IdType]) VALUES
    (N'300000007', N'דנה', N'לוי', N'ירושלים', N'יפו', N'12', NULL, N'026000000', N'0521234567', N'demo1@example.com', N'1980-01-01', N'לקוח', N'ת.ז.'),
    (N'312345671', N'יוסף', N'אברהם', N'תל אביב', N'דיזנגוף', N'80', NULL, N'026001111', N'0521235577', N'demo2@example.com', N'1980-01-02', N'לקוח', N'ת.ז.'),
    (N'324691344', N'מיכל', N'כהן', N'חיפה', N'הנביאים', N'5', NULL, N'026002222', N'0521236587', N'demo3@example.com', N'1980-01-03', N'לקוח', N'ת.ז.'),
    (N'337037014', N'אבי', N'מזרחי', N'בית שמש', N'נהר הירדן', N'21', NULL, N'026003333', N'0521237597', N'demo4@example.com', N'1980-01-04', N'לקוח', N'ת.ז.'),
    (N'349382689', N'נועה', N'פרץ', N'מודיעין', N'עמק דותן', N'3', NULL, N'026004444', N'0521238607', N'demo5@example.com', N'1980-01-05', N'לקוח', N'ת.ז.'),
    (N'361728355', N'שמעון', N'דהן', N'ירושלים', N'עזה', N'40', NULL, N'026005555', N'0521239617', N'demo6@example.com', NULL, N'לקוח נגדי', N'ת.ז.'),
    (N'374074029', N'אורית', N'שגיא', N'רמת גן', N'ביאליק', N'17', NULL, N'026006666', N'0521240627', N'demo7@example.com', NULL, N'לקוח נגדי', N'ת.ז.'),
    (N'386419691', N'גיל', N'רוזן', N'חיפה', N'מוריה', N'102', NULL, N'026007777', N'0521241637', N'demo8@example.com', NULL, N'לקוח נגדי', N'ת.ז.'),
    (N'398765362', N'רחל', N'גולן', N'ירושלים', N'המלך ג׳ורג׳', N'30', NULL, N'026008888', N'0521242647', N'demo9@example.com', NULL, N'עורך דין נגדי', N'ת.ז.'),
    (N'411111032', N'אהרון', N'קליין', N'תל אביב', N'רוטשילד', N'45', NULL, N'026009999', N'0521243657', N'demo10@example.com', NULL, N'עורך דין נגדי', N'ת.ז.'),
    (N'423456706', N'תמר', N'שמש', N'חיפה', N'חורב', N'8', NULL, N'026011110', N'0521244667', N'demo11@example.com', NULL, N'עורך דין נגדי', N'ת.ז.'),
    (N'512345786', N'אלפא נכסים בע"מ', N'', N'תל אביב', N'הארבעה', N'19', NULL, N'026012221', N'0521245677', N'demo12@example.com', NULL, N'לקוח נגדי', N'ח.פ.');
SET IDENTITY_INSERT [dbo].[Cases] ON;
INSERT INTO [dbo].[Cases] ([CaseId], [ClientId], [CaseTypeId], [IsInCourt], [CourtId], [CurrentProcedureId], [PowerOfAttorney], [IsPaid], [Advance], [IsAdvancePaid], [Block], [Parcel], [SubParcel], [PropertyAddress], [ClaimAmount], [AwardedAmount], [FeePercent], [OpposingLawyerId], [OpposingPartyId]) VALUES
    (1, N'300000007', 1, 1, 1, 5, N'נחתם 02/06/2026', 0, 3000, 1, NULL, NULL, NULL, NULL, 85000, NULL, 20, N'398765362', N'361728355'),
    (2, N'312345671', 3, 1, 4, 3, N'נחתם 20/09/2026', 0, 2000, 0, NULL, NULL, NULL, NULL, 42000, NULL, 25, N'411111032', N'374074029'),
    (3, N'324691344', 2, 1, 5, 2, N'', 0, 5000, 0, NULL, NULL, NULL, NULL, 120000, NULL, 18, N'423456706', N'386419691'),
    (4, N'337037014', 4, 0, NULL, 12, N'נחתם 23/10/2026', 0, 1500, 1, N'5432', N'17', N'4', N'נהר הירדן 21, בית שמש', 1850000, NULL, 0.5, N'398765362', N'512345786'),
    (5, N'349382689', 1, 1, 3, 9, N'נחתם 05/10/2025', 1, 2500, 1, NULL, NULL, NULL, NULL, 60000, 65000, 25, N'411111032', N'374074029');
SET IDENTITY_INSERT [dbo].[Cases] OFF;
SET IDENTITY_INSERT [dbo].[CaseSteps] ON;
INSERT INTO [dbo].[CaseSteps] ([CaseStepId], [CaseId], [ProcedureId], [DueDate], [DoneDate], [IsDone], [DurationDays], [Notes], [IsFixedDate]) VALUES
    (1, 1, 1, DATEADD(day, -153, @Today), DATEADD(day, -150, @Today), 1, 7, NULL, 0),
    (2, 1, 2, DATEADD(day, -136, @Today), DATEADD(day, -138, @Today), 1, 14, NULL, 0),
    (3, 1, 3, DATEADD(day, -117, @Today), DATEADD(day, -114, @Today), 1, 21, NULL, 0),
    (4, 1, 4, DATEADD(day, -84, @Today), DATEADD(day, -86, @Today), 1, 30, NULL, 0),
    (5, 1, 5, DATEADD(day, -41, @Today), NULL, 0, 45, N'הדיון נקבע בהחלטת בית המשפט', 1),
    (6, 1, 6, DATEADD(day, -11, @Today), NULL, 0, 30, NULL, 0),
    (7, 1, 7, DATEADD(day, 49, @Today), NULL, 0, 60, NULL, 0),
    (8, 1, 8, DATEADD(day, 79, @Today), NULL, 0, 30, NULL, 0),
    (9, 1, 9, DATEADD(day, 124, @Today), NULL, 0, 45, NULL, 0),
    (10, 2, 1, DATEADD(day, -43, @Today), DATEADD(day, -40, @Today), 1, 7, NULL, 0),
    (11, 2, 2, DATEADD(day, -26, @Today), DATEADD(day, -28, @Today), 1, 14, NULL, 0),
    (12, 2, 3, DATEADD(day, -7, @Today), NULL, 0, 21, NULL, 0),
    (13, 2, 5, DATEADD(day, 38, @Today), NULL, 0, 45, NULL, 0),
    (14, 2, 7, DATEADD(day, 98, @Today), NULL, 0, 60, NULL, 0),
    (15, 2, 9, DATEADD(day, 143, @Today), NULL, 0, 45, NULL, 0),
    (16, 3, 1, DATEADD(day, -28, @Today), DATEADD(day, -25, @Today), 1, 7, NULL, 0),
    (17, 3, 2, DATEADD(day, -11, @Today), NULL, 0, 14, NULL, 0),
    (18, 3, 3, DATEADD(day, 10, @Today), NULL, 0, 21, NULL, 0),
    (19, 3, 4, DATEADD(day, 40, @Today), NULL, 0, 30, NULL, 0),
    (20, 3, 5, DATEADD(day, 85, @Today), NULL, 0, 45, NULL, 0),
    (21, 3, 7, DATEADD(day, 145, @Today), NULL, 0, 60, NULL, 0),
    (22, 3, 9, DATEADD(day, 190, @Today), NULL, 0, 45, NULL, 0),
    (23, 4, 1, DATEADD(day, -13, @Today), DATEADD(day, -10, @Today), 1, 7, NULL, 0),
    (24, 4, 10, DATEADD(day, -7, @Today), DATEADD(day, -9, @Today), 1, 3, NULL, 0),
    (25, 4, 11, DATEADD(day, 1, @Today), DATEADD(day, 0, @Today), 1, 10, NULL, 0),
    (26, 4, 12, DATEADD(day, 7, @Today), NULL, 0, 7, NULL, 0),
    (27, 4, 13, DATEADD(day, 12, @Today), NULL, 0, 5, NULL, 0),
    (28, 4, 14, DATEADD(day, 72, @Today), NULL, 0, 60, NULL, 0),
    (29, 5, 1, DATEADD(day, -393, @Today), DATEADD(day, -390, @Today), 1, 7, NULL, 0),
    (30, 5, 2, DATEADD(day, -376, @Today), DATEADD(day, -378, @Today), 1, 14, NULL, 0),
    (31, 5, 3, DATEADD(day, -357, @Today), DATEADD(day, -354, @Today), 1, 21, NULL, 0),
    (32, 5, 4, DATEADD(day, -324, @Today), DATEADD(day, -326, @Today), 1, 30, NULL, 0),
    (33, 5, 5, DATEADD(day, -281, @Today), DATEADD(day, -278, @Today), 1, 45, NULL, 0),
    (34, 5, 6, DATEADD(day, -248, @Today), DATEADD(day, -250, @Today), 1, 30, NULL, 0),
    (35, 5, 7, DATEADD(day, -190, @Today), DATEADD(day, -187, @Today), 1, 60, NULL, 0),
    (36, 5, 8, DATEADD(day, -157, @Today), DATEADD(day, -159, @Today), 1, 30, NULL, 0),
    (37, 5, 9, DATEADD(day, -114, @Today), DATEADD(day, -111, @Today), 1, 45, NULL, 0);
SET IDENTITY_INSERT [dbo].[CaseSteps] OFF;
SET IDENTITY_INSERT [dbo].[Payments] ON;
INSERT INTO [dbo].[Payments] ([PaymentId], [CaseId], [BalanceAfter], [Method], [Kind], [VatRate], [IsCancelled], [CancelsPaymentId], [CardLast4], [ChequeNumber], [BankNumber], [BranchNumber], [AccountNumber], [Amount], [PaidOn], [Description], [Reference], [ChequeDate]) VALUES
    (1, 1, 15200.0, N'מזומן', N'שכר טרחה', 0.18, 0, NULL, NULL, NULL, NULL, NULL, NULL, 3000, DATEADD(day, -150, @Today), N'מקדמה', NULL, NULL),
    (2, 1, 10200.0, N'העברה בנקאית', N'שכר טרחה', 0.18, 0, NULL, NULL, NULL, NULL, NULL, NULL, 5000, DATEADD(day, -130, @Today), N'תשלום שכר טרחה', N'TR-00002', NULL),
    (3, 4, 7750.0, N'מזומן', N'שכר טרחה', 0.18, 0, NULL, NULL, NULL, NULL, NULL, NULL, 1500, DATEADD(day, -10, @Today), N'מקדמה', NULL, NULL),
    (4, 5, 0.0, N'צ''ק', N'שכר טרחה', 0.18, 0, NULL, NULL, N'1000004', N'12', N'345', N'987654', 16200.0, DATEADD(day, -390, @Today), N'מקדמה', NULL, DATEADD(day, -390, @Today));
SET IDENTITY_INSERT [dbo].[Payments] OFF;
GO

PRINT N'QuedinDB is ready. Log in with admin / Quedin2024';
GO
