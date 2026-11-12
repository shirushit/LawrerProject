/*
    QUEDIN - database script
    -------------------------------------------------------------
    Creates the QuedinDB database on SQL Server LocalDB, builds the schema
    and loads FICTIONAL demo data (people, IDs, phones and cases are invented).

    Run from Visual Studio (SQL Server Object Explorer) or:
        sqlcmd -S "(localdb)\MSSQLLocalDB" -i database\create-database.sql

    Demo users:  admin / Quedin2024   (manager)
                 worker / Worker2024  (regular user)

    Generated from LawyerProject/DB/LawyerProjekt.edmx. Re-running the script
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
IF OBJECT_ID(N'dbo.Tashlumim_Letik', N'U') IS NOT NULL DROP TABLE [dbo].[Tashlumim_Letik];
IF OBJECT_ID(N'dbo.Tahalich_Tipul_Betik', N'U') IS NOT NULL DROP TABLE [dbo].[Tahalich_Tipul_Betik];
IF OBJECT_ID(N'dbo.Tikim_Letipul_Orech_Din', N'U') IS NOT NULL DROP TABLE [dbo].[Tikim_Letipul_Orech_Din];
IF OBJECT_ID(N'dbo.Taalichim_Kvuim_Batik', N'U') IS NOT NULL DROP TABLE [dbo].[Taalichim_Kvuim_Batik];
IF OBJECT_ID(N'dbo.Subjekts_Shebet_Mishpat_Metapel', N'U') IS NOT NULL DROP TABLE [dbo].[Subjekts_Shebet_Mishpat_Metapel];
IF OBJECT_ID(N'dbo.Taalichim_Letikim', N'U') IS NOT NULL DROP TABLE [dbo].[Taalichim_Letikim];
IF OBJECT_ID(N'dbo.Subjekt_Tik', N'U') IS NOT NULL DROP TABLE [dbo].[Subjekt_Tik];
IF OBJECT_ID(N'dbo.Bet_Mishpat', N'U') IS NOT NULL DROP TABLE [dbo].[Bet_Mishpat];
IF OBJECT_ID(N'dbo.Customers', N'U') IS NOT NULL DROP TABLE [dbo].[Customers];
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL DROP TABLE [dbo].[Users];
GO

-- ---------------------------------------------------------- tables
CREATE TABLE [dbo].[Users] (
    [Code] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(50) NOT NULL,
    [PasswordWorker] nvarchar(200) NOT NULL,
    [KindWorker] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Code])
);
CREATE TABLE [dbo].[Customers] (
    [IDcustemer] nvarchar(9) NOT NULL,
    [FirstName] nvarchar(25) NOT NULL,
    [LastName] nvarchar(25) NOT NULL,
    [City] nvarchar(50) NOT NULL,
    [Adress] nvarchar(50) NOT NULL,
    [House] nvarchar(5) NOT NULL,
    [Mikud] nvarchar(50) NULL,
    [Telephone] nvarchar(10) NOT NULL,
    [SmartPhone] nvarchar(10) NULL,
    [Mail] nvarchar(50) NOT NULL,
    [DateBirthday] date NULL,
    [CopyID] nvarchar(50) NULL,
    [SugPerson] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([IDcustemer])
);
CREATE TABLE [dbo].[Bet_Mishpat] (
    [IDBetMishpat] int IDENTITY(1,1) NOT NULL,
    [NameBetMishpat] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Bet_Mishpat] PRIMARY KEY CLUSTERED ([IDBetMishpat])
);
CREATE TABLE [dbo].[Subjekt_Tik] (
    [IDSugTik] int IDENTITY(1,1) NOT NULL,
    [NameSugTik] nvarchar(50) NOT NULL,
    [ScharTircha] money NULL,
    [Agra] money NULL,
    CONSTRAINT [PK_Subjekt_Tik] PRIMARY KEY CLUSTERED ([IDSugTik])
);
CREATE TABLE [dbo].[Taalichim_Letikim] (
    [IDTaalich] int IDENTITY(1,1) NOT NULL,
    [NameTaalich] nvarchar(50) NOT NULL,
    [MeshechZmanLetaalich] nvarchar(50) NULL,
    [SugTaalich] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Taalichim_Letikim] PRIMARY KEY CLUSTERED ([IDTaalich])
);
CREATE TABLE [dbo].[Subjekts_Shebet_Mishpat_Metapel] (
    [IDTable] int IDENTITY(1,1) NOT NULL,
    [IDBetMishpat] int NOT NULL,
    [IDSubjekt] int NOT NULL,
    CONSTRAINT [PK_Subjekts_Shebet_Mishpat_Metapel] PRIMARY KEY CLUSTERED ([IDTable])
);
CREATE TABLE [dbo].[Taalichim_Kvuim_Batik] (
    [IDTaalichKavua] int IDENTITY(1,1) NOT NULL,
    [IDSubjektTik] int NOT NULL,
    [IDTaalich] int NOT NULL,
    [NumLevel] int NOT NULL,
    CONSTRAINT [PK_Taalichim_Kvuim_Batik] PRIMARY KEY CLUSTERED ([IDTaalichKavua])
);
CREATE TABLE [dbo].[Tikim_Letipul_Orech_Din] (
    [IDTik] int IDENTITY(1,1) NOT NULL,
    [IDCostemer] nvarchar(9) NOT NULL,
    [IDSubjektTik] int NOT NULL,
    [IsTheTikMenualInBETMishpat] bit NULL,
    [IDBetMishpat] int NULL,
    [StatusTik] int NOT NULL,
    [IpuiKohach] nvarchar(50) NOT NULL,
    [Shulam] bit NOT NULL,
    [Mikdama] money NOT NULL,
    [ShulamMikdama] bit NULL,
    [Gush] nvarchar(50) NULL,
    [Chelka] nvarchar(50) NULL,
    [TatChelka] nvarchar(50) NULL,
    [AdressNeches] nvarchar(50) NULL,
    [SchumLetvia] money NULL,
    [SchumShenitba] money NULL,
    [AchuzRevachim] float NULL,
    [KodOrechDinNegdi] nvarchar(50) NOT NULL,
    [KodLakoachNegdi] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Tikim_Letipul_Orech_Din] PRIMARY KEY CLUSTERED ([IDTik])
);
CREATE TABLE [dbo].[Tahalich_Tipul_Betik] (
    [IDTahalichTipulLetik] int IDENTITY(1,1) NOT NULL,
    [IDTik] int NOT NULL,
    [IDTahalich] int NOT NULL,
    [TaharichMeshoar] date NULL,
    [TaharichBizhua] date NULL,
    [Buzha] bit NULL,
    CONSTRAINT [PK_Tahalich_Tipul_Betik] PRIMARY KEY CLUSTERED ([IDTahalichTipulLetik])
);
CREATE TABLE [dbo].[Tashlumim_Letik] (
    [IDTashlum] int IDENTITY(1,1) NOT NULL,
    [IDTik] int NOT NULL,
    [Itra] money NOT NULL,
    [EmzhaeyTashlum] nvarchar(50) NOT NULL,
    [NumCartis] nchar(16) NULL,
    [Tokef] nchar(7) NULL,
    [ThreeNums] nchar(3) NULL,
    [NumTeudatZeut] nchar(9) NULL,
    [NumChek] nchar(10) NULL,
    [NumBank] nchar(3) NULL,
    [NumSnif] nchar(3) NULL,
    [NumCheshbon] nvarchar(50) NULL,
    [Schum] money NOT NULL,
    [TaarichTashlum] date NOT NULL,
    [Teur] nvarchar(50) NULL,
    [NumAsmachta] nvarchar(50) NULL,
    [TaarichLachek] date NULL,
    CONSTRAINT [PK_Tashlumim_Letik] PRIMARY KEY CLUSTERED ([IDTashlum])
);
GO

-- ---------------------------------------------------------- relationships
ALTER TABLE [dbo].[Subjekts_Shebet_Mishpat_Metapel] ADD CONSTRAINT [FK_Subjekts_Shebet_Mishpat_Metapel_ToTable] FOREIGN KEY ([IDBetMishpat]) REFERENCES [dbo].[Bet_Mishpat] ([IDBetMishpat]);
ALTER TABLE [dbo].[Subjekts_Shebet_Mishpat_Metapel] ADD CONSTRAINT [FK_Subjekts_Shebet_Mishpat_Metapel_ToTable_1] FOREIGN KEY ([IDSubjekt]) REFERENCES [dbo].[Subjekt_Tik] ([IDSugTik]);
ALTER TABLE [dbo].[Taalichim_Kvuim_Batik] ADD CONSTRAINT [FK_Taalichim_Kvuim_Batik_ToTable] FOREIGN KEY ([IDSubjektTik]) REFERENCES [dbo].[Subjekt_Tik] ([IDSugTik]);
ALTER TABLE [dbo].[Taalichim_Kvuim_Batik] ADD CONSTRAINT [FK_Taalichim_Kvuim_Batik_ToTable_1] FOREIGN KEY ([IDTaalich]) REFERENCES [dbo].[Taalichim_Letikim] ([IDTaalich]);
ALTER TABLE [dbo].[Tahalich_Tipul_Betik] ADD CONSTRAINT [FK_Tahalich_Tipul_Betik_ToTable] FOREIGN KEY ([IDTik]) REFERENCES [dbo].[Tikim_Letipul_Orech_Din] ([IDTik]);
ALTER TABLE [dbo].[Tahalich_Tipul_Betik] ADD CONSTRAINT [FK_Tahalich_Tipul_Betik_ToTable_1] FOREIGN KEY ([IDTahalich]) REFERENCES [dbo].[Taalichim_Letikim] ([IDTaalich]);
ALTER TABLE [dbo].[Tashlumim_Letik] ADD CONSTRAINT [FK_Tashlumim_Letik_ToTable] FOREIGN KEY ([IDTik]) REFERENCES [dbo].[Tikim_Letipul_Orech_Din] ([IDTik]);
ALTER TABLE [dbo].[Tikim_Letipul_Orech_Din] ADD CONSTRAINT [FK_Tikim_Letipul_Orech_Din_ToTable_1] FOREIGN KEY ([IDSubjektTik]) REFERENCES [dbo].[Subjekt_Tik] ([IDSugTik]);
ALTER TABLE [dbo].[Tikim_Letipul_Orech_Din] ADD CONSTRAINT [FK_Tikim_Letipul_Orech_Din_ToTable_2] FOREIGN KEY ([IDBetMishpat]) REFERENCES [dbo].[Bet_Mishpat] ([IDBetMishpat]);
ALTER TABLE [dbo].[Tikim_Letipul_Orech_Din] ADD CONSTRAINT [FK_Tikim_Letipul_Orech_Din_ToTable_3] FOREIGN KEY ([StatusTik]) REFERENCES [dbo].[Taalichim_Letikim] ([IDTaalich]);
ALTER TABLE [dbo].[Tikim_Letipul_Orech_Din] ADD CONSTRAINT [FK_Tikim_Letipul_Orech_Din_ToTable3] FOREIGN KEY ([IDCostemer]) REFERENCES [dbo].[Customers] ([IDcustemer]);
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [UQ_Users_UserName] UNIQUE ([UserName]);
ALTER TABLE [dbo].[Taalichim_Kvuim_Batik] ADD CONSTRAINT [UQ_Taalichim_Kvuim_Batik_Level] UNIQUE ([IDSubjektTik], [NumLevel]);
CREATE INDEX [IX_Tahalich_Tipul_Betik_IDTik] ON [dbo].[Tahalich_Tipul_Betik] ([IDTik]);
CREATE INDEX [IX_Tashlumim_Letik_IDTik] ON [dbo].[Tashlumim_Letik] ([IDTik]);
CREATE INDEX [IX_Tikim_Letipul_Orech_Din_IDCostemer] ON [dbo].[Tikim_Letipul_Orech_Din] ([IDCostemer]);
GO

-- ---------------------------------------------------------- demo data (fictional)
DECLARE @Today date = CAST(GETDATE() AS date);

SET IDENTITY_INSERT [dbo].[Users] ON;
INSERT INTO [dbo].[Users] ([Code], [UserName], [PasswordWorker], [KindWorker]) VALUES
    (1, N'admin', N'pbkdf2$100000$cXVlZGluLWRlbW8tc2FsdDE=$Kmr3WtJMlhRlHwEsl64u3hXYgOI4X3NMPBL5JyRKBa4=', N'מנהל'),
    (2, N'worker', N'pbkdf2$100000$cXVlZGluLWRlbW8tc2FsdDI=$D4bLJ4IlgXxnzMguYNwv+DaETNIhWIS73LeJBgkOPsA=', N'משתמש רגיל');
SET IDENTITY_INSERT [dbo].[Users] OFF;
SET IDENTITY_INSERT [dbo].[Bet_Mishpat] ON;
INSERT INTO [dbo].[Bet_Mishpat] ([IDBetMishpat], [NameBetMishpat]) VALUES
    (1, N'בית משפט השלום ירושלים'),
    (2, N'בית המשפט המחוזי ירושלים'),
    (3, N'בית משפט השלום תל אביב'),
    (4, N'בית הדין האזורי לעבודה תל אביב'),
    (5, N'בית משפט השלום חיפה');
SET IDENTITY_INSERT [dbo].[Bet_Mishpat] OFF;
SET IDENTITY_INSERT [dbo].[Subjekt_Tik] ON;
INSERT INTO [dbo].[Subjekt_Tik] ([IDSugTik], [NameSugTik], [ScharTircha], [Agra]) VALUES
    (1, N'תביעה אזרחית', 15000, 1200),
    (2, N'תביעת נזיקין', 12000, 900),
    (3, N'דיני עבודה', 8000, 0),
    (4, N'עסקת מקרקעין', 6000, 0);
SET IDENTITY_INSERT [dbo].[Subjekt_Tik] OFF;
SET IDENTITY_INSERT [dbo].[Taalichim_Letikim] ON;
INSERT INTO [dbo].[Taalichim_Letikim] ([IDTaalich], [NameTaalich], [MeshechZmanLetaalich], [SugTaalich]) VALUES
    (1, N'פגישת היכרות וחתימה על ייפוי כוח', N'7', N'פגישה'),
    (2, N'איסוף מסמכים וראיות', N'14', N'הכנה'),
    (3, N'הגשת כתב תביעה', N'21', N'הגשה'),
    (4, N'המתנה לכתב הגנה', N'30', N'המתנה'),
    (5, N'קדם משפט', N'45', N'דיון'),
    (6, N'הגשת תצהירי עדות ראשית', N'30', N'הגשה'),
    (7, N'דיון הוכחות', N'60', N'דיון'),
    (8, N'הגשת סיכומים', N'30', N'הגשה'),
    (9, N'פסק דין', N'45', N'החלטה'),
    (10, N'בדיקת נסח טאבו', N'3', N'בדיקה'),
    (11, N'ניסוח הסכם מכר', N'10', N'ניסוח'),
    (12, N'חתימה על הסכם המכר', N'7', N'חתימה'),
    (13, N'רישום הערת אזהרה', N'5', N'רישום'),
    (14, N'העברת בעלות בטאבו', N'60', N'רישום');
SET IDENTITY_INSERT [dbo].[Taalichim_Letikim] OFF;
SET IDENTITY_INSERT [dbo].[Subjekts_Shebet_Mishpat_Metapel] ON;
INSERT INTO [dbo].[Subjekts_Shebet_Mishpat_Metapel] ([IDTable], [IDBetMishpat], [IDSubjekt]) VALUES
    (1, 1, 1),
    (2, 3, 1),
    (3, 5, 1),
    (4, 2, 1),
    (5, 1, 2),
    (6, 3, 2),
    (7, 2, 2),
    (8, 4, 3);
SET IDENTITY_INSERT [dbo].[Subjekts_Shebet_Mishpat_Metapel] OFF;
SET IDENTITY_INSERT [dbo].[Taalichim_Kvuim_Batik] ON;
INSERT INTO [dbo].[Taalichim_Kvuim_Batik] ([IDTaalichKavua], [IDSubjektTik], [IDTaalich], [NumLevel]) VALUES
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
SET IDENTITY_INSERT [dbo].[Taalichim_Kvuim_Batik] OFF;
INSERT INTO [dbo].[Customers] ([IDcustemer], [FirstName], [LastName], [City], [Adress], [House], [Mikud], [Telephone], [SmartPhone], [Mail], [DateBirthday], [CopyID], [SugPerson]) VALUES
    (N'300000007', N'דנה', N'לוי', N'ירושלים', N'יפו', N'12', NULL, N'026000000', N'0521234567', N'demo1@example.com', N'1980-01-01', NULL, N'לקוח'),
    (N'312345671', N'יוסף', N'אברהם', N'תל אביב', N'דיזנגוף', N'80', NULL, N'026001111', N'0521235577', N'demo2@example.com', N'1980-01-02', NULL, N'לקוח'),
    (N'324691344', N'מיכל', N'כהן', N'חיפה', N'הנביאים', N'5', NULL, N'026002222', N'0521236587', N'demo3@example.com', N'1980-01-03', NULL, N'לקוח'),
    (N'337037014', N'אבי', N'מזרחי', N'בית שמש', N'נהר הירדן', N'21', NULL, N'026003333', N'0521237597', N'demo4@example.com', N'1980-01-04', NULL, N'לקוח'),
    (N'349382689', N'נועה', N'פרץ', N'מודיעין', N'עמק דותן', N'3', NULL, N'026004444', N'0521238607', N'demo5@example.com', N'1980-01-05', NULL, N'לקוח'),
    (N'361728355', N'שמעון', N'דהן', N'ירושלים', N'עזה', N'40', NULL, N'026005555', N'0521239617', N'demo6@example.com', NULL, NULL, N'לקוח נגדי'),
    (N'374074029', N'אורית', N'שגיא', N'רמת גן', N'ביאליק', N'17', NULL, N'026006666', N'0521240627', N'demo7@example.com', NULL, NULL, N'לקוח נגדי'),
    (N'386419691', N'גיל', N'רוזן', N'חיפה', N'מוריה', N'102', NULL, N'026007777', N'0521241637', N'demo8@example.com', NULL, NULL, N'לקוח נגדי'),
    (N'398765362', N'רחל', N'גולן', N'ירושלים', N'המלך ג׳ורג׳', N'30', NULL, N'026008888', N'0521242647', N'demo9@example.com', NULL, NULL, N'עורך דין נגדי'),
    (N'411111032', N'אהרון', N'קליין', N'תל אביב', N'רוטשילד', N'45', NULL, N'026009999', N'0521243657', N'demo10@example.com', NULL, NULL, N'עורך דין נגדי'),
    (N'423456706', N'תמר', N'שמש', N'חיפה', N'חורב', N'8', NULL, N'026011110', N'0521244667', N'demo11@example.com', NULL, NULL, N'עורך דין נגדי');
SET IDENTITY_INSERT [dbo].[Tikim_Letipul_Orech_Din] ON;
INSERT INTO [dbo].[Tikim_Letipul_Orech_Din] ([IDTik], [IDCostemer], [IDSubjektTik], [IsTheTikMenualInBETMishpat], [IDBetMishpat], [StatusTik], [IpuiKohach], [Shulam], [Mikdama], [ShulamMikdama], [Gush], [Chelka], [TatChelka], [AdressNeches], [SchumLetvia], [SchumShenitba], [AchuzRevachim], [KodOrechDinNegdi], [KodLakoachNegdi]) VALUES
    (1, N'300000007', 1, 1, 1, 5, N'יפ"כ 2024/101', 0, 3000, 1, NULL, NULL, NULL, NULL, 85000, NULL, 20, N'398765362', N'361728355'),
    (2, N'312345671', 3, 1, 4, 3, N'יפ"כ 2024/102', 0, 2000, 0, NULL, NULL, NULL, NULL, 42000, NULL, 25, N'411111032', N'374074029'),
    (3, N'324691344', 2, 1, 5, 2, N'יפ"כ 2024/103', 0, 5000, 0, NULL, NULL, NULL, NULL, 120000, NULL, 18, N'423456706', N'386419691'),
    (4, N'337037014', 4, 0, NULL, 12, N'יפ"כ 2024/104', 0, 1500, 1, N'5432', N'17', N'4', N'נהר הירדן 21, בית שמש', 1850000, NULL, 0.5, N'398765362', N'361728355'),
    (5, N'349382689', 1, 1, 3, 9, N'יפ"כ 2023/087', 1, 2500, 1, NULL, NULL, NULL, NULL, 60000, 65000, 20, N'411111032', N'374074029');
SET IDENTITY_INSERT [dbo].[Tikim_Letipul_Orech_Din] OFF;
SET IDENTITY_INSERT [dbo].[Tahalich_Tipul_Betik] ON;
INSERT INTO [dbo].[Tahalich_Tipul_Betik] ([IDTahalichTipulLetik], [IDTik], [IDTahalich], [TaharichMeshoar], [TaharichBizhua], [Buzha]) VALUES
    (1, 1, 1, DATEADD(day, -153, @Today), DATEADD(day, -150, @Today), 1),
    (2, 1, 2, DATEADD(day, -136, @Today), DATEADD(day, -138, @Today), 1),
    (3, 1, 3, DATEADD(day, -117, @Today), DATEADD(day, -114, @Today), 1),
    (4, 1, 4, DATEADD(day, -84, @Today), DATEADD(day, -86, @Today), 1),
    (5, 1, 5, DATEADD(day, -41, @Today), NULL, 0),
    (6, 1, 6, DATEADD(day, -11, @Today), NULL, 0),
    (7, 1, 7, DATEADD(day, 49, @Today), NULL, 0),
    (8, 1, 8, DATEADD(day, 79, @Today), NULL, 0),
    (9, 1, 9, DATEADD(day, 124, @Today), NULL, 0),
    (10, 2, 1, DATEADD(day, -43, @Today), DATEADD(day, -40, @Today), 1),
    (11, 2, 2, DATEADD(day, -26, @Today), DATEADD(day, -28, @Today), 1),
    (12, 2, 3, DATEADD(day, -7, @Today), NULL, 0),
    (13, 2, 5, DATEADD(day, 38, @Today), NULL, 0),
    (14, 2, 7, DATEADD(day, 98, @Today), NULL, 0),
    (15, 2, 9, DATEADD(day, 143, @Today), NULL, 0),
    (16, 3, 1, DATEADD(day, -28, @Today), DATEADD(day, -25, @Today), 1),
    (17, 3, 2, DATEADD(day, -11, @Today), NULL, 0),
    (18, 3, 3, DATEADD(day, 10, @Today), NULL, 0),
    (19, 3, 4, DATEADD(day, 40, @Today), NULL, 0),
    (20, 3, 5, DATEADD(day, 85, @Today), NULL, 0),
    (21, 3, 7, DATEADD(day, 145, @Today), NULL, 0),
    (22, 3, 9, DATEADD(day, 190, @Today), NULL, 0),
    (23, 4, 1, DATEADD(day, -13, @Today), DATEADD(day, -10, @Today), 1),
    (24, 4, 10, DATEADD(day, -7, @Today), DATEADD(day, -9, @Today), 1),
    (25, 4, 11, DATEADD(day, 1, @Today), DATEADD(day, 0, @Today), 1),
    (26, 4, 12, DATEADD(day, 7, @Today), NULL, 0),
    (27, 4, 13, DATEADD(day, 12, @Today), NULL, 0),
    (28, 4, 14, DATEADD(day, 72, @Today), NULL, 0),
    (29, 5, 1, DATEADD(day, -393, @Today), DATEADD(day, -390, @Today), 1),
    (30, 5, 2, DATEADD(day, -376, @Today), DATEADD(day, -378, @Today), 1),
    (31, 5, 3, DATEADD(day, -357, @Today), DATEADD(day, -354, @Today), 1),
    (32, 5, 4, DATEADD(day, -324, @Today), DATEADD(day, -326, @Today), 1),
    (33, 5, 5, DATEADD(day, -281, @Today), DATEADD(day, -278, @Today), 1),
    (34, 5, 6, DATEADD(day, -248, @Today), DATEADD(day, -250, @Today), 1),
    (35, 5, 7, DATEADD(day, -190, @Today), DATEADD(day, -187, @Today), 1),
    (36, 5, 8, DATEADD(day, -157, @Today), DATEADD(day, -159, @Today), 1),
    (37, 5, 9, DATEADD(day, -114, @Today), DATEADD(day, -111, @Today), 1);
SET IDENTITY_INSERT [dbo].[Tahalich_Tipul_Betik] OFF;
SET IDENTITY_INSERT [dbo].[Tashlumim_Letik] ON;
INSERT INTO [dbo].[Tashlumim_Letik] ([IDTashlum], [IDTik], [Itra], [EmzhaeyTashlum], [NumCartis], [Tokef], [ThreeNums], [NumTeudatZeut], [NumChek], [NumBank], [NumSnif], [NumCheshbon], [Schum], [TaarichTashlum], [Teur], [NumAsmachta], [TaarichLachek]) VALUES
    (1, 1, 15200.0, N'מזומן', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 3000, DATEADD(day, -150, @Today), N'מקדמה', NULL, NULL),
    (2, 1, 10200.0, N'העברה בנקאית', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5000, DATEADD(day, -130, @Today), N'תשלום שכר טרחה', N'TR-00002', NULL),
    (3, 4, 7750.0, N'מזומן', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1500, DATEADD(day, -10, @Today), N'מקדמה', NULL, NULL),
    (4, 5, 0.0, N'צ''ק', NULL, NULL, NULL, NULL, N'1000004', N'12', N'345', N'987654', 13200.0, DATEADD(day, -390, @Today), N'מקדמה', NULL, DATEADD(day, -390, @Today));
SET IDENTITY_INSERT [dbo].[Tashlumim_Letik] OFF;
GO

PRINT N'QuedinDB is ready. Log in with admin / Quedin2024';
GO
