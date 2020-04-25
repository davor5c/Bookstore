# Builds binaries and deploys the Bookstore application on it.
# Prerequisites: VS2017 or newer, with MSBuild.
# See Readme.md for more info.

"=== Setup ==="
$ErrorActionPreference = 'Stop'
$msbuild, $vstest = .\tools\Build\Find-VisualStudio.ps1
.\tools\Build\Test-RhetosPrerequisites.ps1

"=== Build ==="
& $msbuild 'Bookstore.sln' /t:restore /t:rebuild /p:Configuration=Debug /p:RhetosDeploy=False /verbosity:minimal
if ($LastExitCode -ne 0) { throw "MSBuild failed." }

"=== Deploy ==="
& '.\src\Bookstore.Service\bin\rhetos.exe' dbupdate
if ($LastExitCode -ne 0) { throw "rhetos dbupdate failed." }
