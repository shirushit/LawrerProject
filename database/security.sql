/*
    QUEDIN - database permissions (optional, recommended for a real office)
    -------------------------------------------------------------
    The app checks roles in its own code, but anyone who can open the database
    directly (SQL Server Management Studio, Excel) could bypass those checks.
    This script moves the important rules into SQL Server itself.

    The app connects with Windows authentication, so each employee's Windows
    account is mapped to one of two database roles:

        QuedinWorker   - daily work: customers, cases, steps, payments.
                         Cannot delete anything, cannot change the catalogue,
                         cannot read or change the Users table (logging in goes
                         through procedures) and cannot edit a recorded payment.
        QuedinManager  - everything a worker can do, plus the catalogue (courts,
                         case types, procedures), users, settings, holidays,
                         deleting a case opened by mistake, and backups.

    Nobody can change or delete audit-log rows: the table also has an
    append-only trigger (see create-database.sql).

    Run after create-database.sql (and again whenever create-database.sql is re-run,
    since that recreates the tables), then add the office's accounts at the end.
*/
USE [QuedinDB];
GO

IF DATABASE_PRINCIPAL_ID(N'QuedinWorker') IS NULL CREATE ROLE [QuedinWorker];
IF DATABASE_PRINCIPAL_ID(N'QuedinManager') IS NULL CREATE ROLE [QuedinManager];
GO
-- v1.3.1: the two roles are separate (a manager used to be a member of QuedinWorker, and the
-- DENY on the Users table below would then apply to managers too).
IF IS_ROLEMEMBER(N'QuedinWorker', N'QuedinManager') = 1 ALTER ROLE [QuedinWorker] DROP MEMBER [QuedinManager];
GO

-- ---------------------------------------------------------- workers
GRANT SELECT ON SCHEMA::[dbo] TO [QuedinWorker];
GRANT INSERT, UPDATE ON [dbo].[Customers] TO [QuedinWorker];
GRANT INSERT, UPDATE ON [dbo].[Cases] TO [QuedinWorker];
GRANT INSERT, UPDATE ON [dbo].[CaseSteps] TO [QuedinWorker];
-- Payments: new rows and the "cancelled" flag only. A trigger (create-database.sql) also refuses
-- any change to an amount, date or method, whoever makes it.
GRANT INSERT ON [dbo].[Payments] TO [QuedinWorker];
GRANT UPDATE ([IsCancelled]) ON [dbo].[Payments] TO [QuedinWorker];
GRANT INSERT ON [dbo].[AuditLog] TO [QuedinWorker];
-- The Users table is closed to workers: no reading (password hashes) and no writing. Logging in and
-- changing one's own password go through procedures that check the password inside the database.
DENY SELECT, INSERT, UPDATE, DELETE ON [dbo].[Users] TO [QuedinWorker];
GRANT EXECUTE ON [dbo].[usp_GetLoginSalt] TO [QuedinWorker];
GRANT EXECUTE ON [dbo].[usp_TryLogin] TO [QuedinWorker];
GRANT EXECUTE ON [dbo].[usp_ChangeOwnPassword] TO [QuedinWorker];
GO

-- ---------------------------------------------------------- managers (everything a worker can do, and more)
GRANT SELECT ON SCHEMA::[dbo] TO [QuedinManager];
GRANT INSERT, UPDATE ON [dbo].[Customers] TO [QuedinManager];
GRANT INSERT, UPDATE ON [dbo].[Cases] TO [QuedinManager];
GRANT INSERT, UPDATE ON [dbo].[CaseSteps] TO [QuedinManager];
GRANT INSERT ON [dbo].[Payments] TO [QuedinManager];
GRANT UPDATE ([IsCancelled]) ON [dbo].[Payments] TO [QuedinManager];
GRANT INSERT ON [dbo].[AuditLog] TO [QuedinManager];
GRANT EXECUTE ON [dbo].[usp_GetLoginSalt] TO [QuedinManager];
GRANT EXECUTE ON [dbo].[usp_TryLogin] TO [QuedinManager];
GRANT EXECUTE ON [dbo].[usp_ChangeOwnPassword] TO [QuedinManager];
GRANT INSERT, UPDATE, DELETE ON [dbo].[Courts] TO [QuedinManager];
GRANT INSERT, UPDATE, DELETE ON [dbo].[CaseTypes] TO [QuedinManager];
GRANT INSERT, UPDATE, DELETE ON [dbo].[Procedures] TO [QuedinManager];
GRANT INSERT, UPDATE, DELETE ON [dbo].[CourtCaseTypes] TO [QuedinManager];
GRANT INSERT, UPDATE, DELETE ON [dbo].[CaseTypeSteps] TO [QuedinManager];
GRANT INSERT, UPDATE ON [dbo].[Users] TO [QuedinManager];     -- add users, reset passwords, (de)activate
GRANT INSERT, UPDATE ON [dbo].[Settings] TO [QuedinManager];
GRANT INSERT, DELETE ON [dbo].[Holidays] TO [QuedinManager];
GRANT DELETE ON [dbo].[Cases] TO [QuedinManager];             -- only cases with no payments and no completed steps (checked by the app)
GRANT DELETE ON [dbo].[CaseSteps] TO [QuedinManager];
ALTER ROLE [db_backupoperator] ADD MEMBER [QuedinManager];
GO

-- ---------------------------------------------------------- only a manager may re-open a completed step
CREATE OR ALTER TRIGGER [dbo].[TR_CaseSteps_UndoByManager] ON [dbo].[CaseSteps]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted i JOIN deleted d ON d.[CaseStepId] = i.[CaseStepId]
               WHERE d.[IsDone] = 1 AND ISNULL(i.[IsDone], 0) = 0)
       AND IS_MEMBER(N'QuedinManager') = 0 AND IS_MEMBER(N'db_owner') = 0
    BEGIN
        RAISERROR (N'Only a manager can re-open a completed step.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END
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
