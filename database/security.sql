/*
    QUEDIN - database permissions (optional, recommended for a real office)
    -------------------------------------------------------------
    The app checks roles in its own code, but anyone who can open the database
    directly (SQL Server Management Studio, Excel) could bypass those checks.
    This script moves the important rules into SQL Server itself.

    The app connects with Windows authentication, so each employee's Windows
    account is mapped to one of two database roles:

        QuedinWorker   - daily work: customers, cases, steps, payments.
                         Cannot delete anything and cannot change the catalogue.
        QuedinManager  - everything a worker can do, plus the catalogue (courts,
                         case types, procedures), users, settings, holidays,
                         deleting a case opened by mistake, and backups.

    Nobody can change or delete audit-log rows: the table also has an
    append-only trigger (see create-database.sql).

    Run once, after create-database.sql, then add the office's accounts at the end.
*/
USE [QuedinDB];
GO

IF DATABASE_PRINCIPAL_ID(N'QuedinWorker') IS NULL CREATE ROLE [QuedinWorker];
IF DATABASE_PRINCIPAL_ID(N'QuedinManager') IS NULL CREATE ROLE [QuedinManager];
GO

-- ---------------------------------------------------------- workers
GRANT SELECT ON SCHEMA::[dbo] TO [QuedinWorker];
GRANT INSERT, UPDATE ON [dbo].[Customers] TO [QuedinWorker];
GRANT INSERT, UPDATE ON [dbo].[Cases] TO [QuedinWorker];
GRANT INSERT, UPDATE ON [dbo].[CaseSteps] TO [QuedinWorker];
GRANT INSERT, UPDATE ON [dbo].[Payments] TO [QuedinWorker];   -- payments are cancelled with a reversing entry, never deleted
GRANT INSERT ON [dbo].[AuditLog] TO [QuedinWorker];
-- logging in counts failed attempts; changing one's own password updates the hash
GRANT UPDATE ([PasswordHash], [FailedLogins], [LockedUntil]) ON [dbo].[Users] TO [QuedinWorker];
GO

-- ---------------------------------------------------------- managers (also members of QuedinWorker)
ALTER ROLE [QuedinWorker] ADD MEMBER [QuedinManager];
GRANT INSERT, UPDATE, DELETE ON [dbo].[Courts] TO [QuedinManager];
GRANT INSERT, UPDATE, DELETE ON [dbo].[CaseTypes] TO [QuedinManager];
GRANT INSERT, UPDATE, DELETE ON [dbo].[Procedures] TO [QuedinManager];
GRANT INSERT, UPDATE, DELETE ON [dbo].[CourtCaseTypes] TO [QuedinManager];
GRANT INSERT, UPDATE, DELETE ON [dbo].[CaseTypeSteps] TO [QuedinManager];
GRANT INSERT, UPDATE ON [dbo].[Users] TO [QuedinManager];
GRANT INSERT, UPDATE ON [dbo].[Settings] TO [QuedinManager];
GRANT INSERT, DELETE ON [dbo].[Holidays] TO [QuedinManager];
GRANT DELETE ON [dbo].[Cases] TO [QuedinManager];             -- only cases with no payments and no completed steps (checked by the app)
GRANT DELETE ON [dbo].[CaseSteps] TO [QuedinManager];
ALTER ROLE [db_backupoperator] ADD MEMBER [QuedinManager];
GO

-- ---------------------------------------------------------- nobody edits the audit log
DENY UPDATE, DELETE ON [dbo].[AuditLog] TO [QuedinWorker];
DENY UPDATE, DELETE ON [dbo].[AuditLog] TO [QuedinManager];
GO

/*  Add the office's Windows accounts (replace OFFICE\... with real names):

    CREATE LOGIN [OFFICE\dana] FROM WINDOWS;
    CREATE USER  [OFFICE\dana] FOR LOGIN [OFFICE\dana];
    ALTER ROLE   [QuedinManager] ADD MEMBER [OFFICE\dana];

    CREATE LOGIN [OFFICE\yossi] FROM WINDOWS;
    CREATE USER  [OFFICE\yossi] FOR LOGIN [OFFICE\yossi];
    ALTER ROLE   [QuedinWorker] ADD MEMBER [OFFICE\yossi];
*/
