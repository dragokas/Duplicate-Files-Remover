@echo off
cd /d "%~dp0"

:: CONSTANTS
set compressor=%ProgramFiles%\7-Zip\7z.exe
set prj=FindMyDuplicates
set db=FileSystem
set release_dir=%~dp0bin\Release\net10.0-windows7.0

rd /s /q "%release_dir%"

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
2>nul del "main.log"
2>nul del "%prj%.deps.json"
2>nul del "%prj%.pdb"
:: Remove database
2>nul del "%db%.db"
2>nul del "%db%.db-shm"
2>nul del "%db%.db-wal"
:: Remove unused dlls
2>nul del "SQLitePCLRaw.batteries_v2.dll"
2>nul del "SQLitePCLRaw.core.dll"
2>nul del "SQLitePCLRaw.provider.e_sqlite3.dll"
pushd runtimes
for /f %%a in ('dir /b /ad') do (
  if "%%a" neq "win-x64" if "%%a" neq "win-x86" rd /s /q "%%a"
)
popd

call "%~DP0\protector\ProtectMe.cmd" -silent

:: save mapping file
set "map_folder=%~DP0\protector\mapping\%date%"
md "%map_folder%" 2>nul
move /y "%release_dir%\*.nrmap" "%map_folder%\"

:: Prepare release archive
if exist "%prj%.zip" del "%prj%.zip"
"%compressor%" a -tzip -mx=9 -r "%prj%.zip" .\*
move "%prj%.zip" "%~dp0"

pause