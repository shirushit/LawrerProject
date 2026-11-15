@echo off
rem QUEDIN - nightly database backup
rem ------------------------------------------------------------
rem Backs up QuedinDB to a dated .bak file and deletes backups older than 30 days.
rem Schedule it in Windows Task Scheduler (daily, e.g. 23:00), on the computer that runs SQL Server:
rem     schtasks /Create /TN "QUEDIN backup" /SC DAILY /ST 23:00 /TR "C:\Quedin\database\backup-nightly.cmd"
rem
rem Restore (close the app on every computer first):
rem     sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "RESTORE DATABASE [QuedinDB] FROM DISK=N'C:\QuedinBackups\QuedinDB_20261115_2300.bak' WITH REPLACE"

setlocal
set SERVER=(localdb)\MSSQLLocalDB
set FOLDER=C:\QuedinBackups
if not exist "%FOLDER%" mkdir "%FOLDER%"

for /f %%i in ('powershell -NoProfile -Command "Get-Date -Format yyyyMMdd_HHmm"') do set STAMP=%%i
set FILE=%FOLDER%\QuedinDB_%STAMP%.bak

sqlcmd -S "%SERVER%" -b -Q "BACKUP DATABASE [QuedinDB] TO DISK = N'%FILE%' WITH INIT; INSERT INTO [QuedinDB].[dbo].[AuditLog] ([OccurredAt], [UserName], [Action], [Entity], [Details]) VALUES (SYSDATETIME(), N'nightly-task', NCHAR(1490)+NCHAR(1497)+NCHAR(1489)+NCHAR(1493)+NCHAR(1497), NCHAR(1502)+NCHAR(1506)+NCHAR(1512)+NCHAR(1499)+NCHAR(1514), N'%FILE%');"
if errorlevel 1 (
    echo Backup FAILED. >&2
    exit /b 1
)

forfiles /p "%FOLDER%" /m QuedinDB_*.bak /d -30 /c "cmd /c del @path" 2>nul
echo Backup saved to %FILE%
endlocal
