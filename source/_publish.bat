@echo off
cd /d "%~dp0"

:: CONSTANTS
set compressor=%ProgramFiles%\7-Zip\7z.exe
set prj=DuplicateFilesRemover
set db=FileSystem
set release_dir=%~dp0bin\Release\net10.0-windows7.0

:: Compile project if needed
if not exist "%release_dir%\%prj%.exe" (
  set DOTNET_CLI_TELEMETRY_OPTOUT=1
  dotnet build "DuplicatesFinder.sln" --configuration Release /p:Platform="Any CPU"
)

if not exist "%release_dir%\%prj%.exe" (
  echo Failed to build the solution!
  pause>NUL & exit /b
)

cd /d "%release_dir%"

:: Remove logfile
del "main.log"
del "%prj%.deps.json"
del "%prj%.pdb"
:: Remove database
del "%db%.db"
del "%db%.db-shm"
del "%db%.db-wal"
:: Remove unused dlls
del "SQLitePCLRaw.batteries_v2.dll"
del "SQLitePCLRaw.core.dll"
del "SQLitePCLRaw.provider.e_sqlite3.dll"
pushd runtimes
for /f %%a in ('dir /b /ad') do (
  if "%%a" neq "win-x64" if "%%a" neq "win-x86" rd /s /q "%%a"
)
popd

:: Prepare release archive
if exist "%prj%.zip" del "%prj%.zip"
"%compressor%" a -tzip -mx=9 -r "%prj%.zip" .\*
move "%prj%.zip" "%~dp0"

pause