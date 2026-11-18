@echo off
rem Creates QuedinDB with fictional demo data on LocalDB and starts the application.
rem Do not run this against a database that holds real data: it recreates the database.
setlocal
cd /d "%~dp0"

echo This recreates the QuedinDB database on (localdb)\MSSQLLocalDB with demo data.
echo Any existing data in QuedinDB will be lost.
choice /c YN /m "Continue"
if errorlevel 2 exit /b 1

where sqlcmd >nul 2>&1
if errorlevel 1 (
    echo sqlcmd was not found. Install SQL Server Express LocalDB and the sqlcmd utility, then try again.
    pause
    exit /b 1
)

sqllocaldb start MSSQLLocalDB >nul 2>&1
sqlcmd -S "(localdb)\MSSQLLocalDB" -I -b -i database\create-database.sql
if errorlevel 1 (
    echo Database setup failed. See the messages above.
    pause
    exit /b 1
)

start "" app\LawyerProject.exe
