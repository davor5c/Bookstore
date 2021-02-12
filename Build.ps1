# Builds binaries and deploys the Bookstore application on it.
# Prerequisites: VS2017 or newer, with MSBuild.
# See Readme.md for more info.

"=== Setup ==="
$ErrorActionPreference = 'Stop'
.\tools\Build\Test-RhetosPrerequisites.ps1

"=== Build ==="
& dotnet build 'Bookstore.sln' -p:Configuration=Debug -p:RhetosDeploy=False --verbosity minimal
if ($LastExitCode -ne 0) { throw "MSBuild failed." }

"=== Deploy ==="
& '.\src\Bookstore.Service\bin\Debug\net5.0\rhetos.exe' dbupdate '.\src\Bookstore.Service\bin\Debug\net5.0\Bookstore.Service.dll'
if ($LastExitCode -ne 0) { throw "rhetos dbupdate failed." }
