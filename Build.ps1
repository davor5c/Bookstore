# Builds binaries, downloads Rhetos server and deploys the Bookstore application on it.
# Prerequisites: VS2017 or newer, with MSBuild.
# See Readme.md for more info.

# Setup:
$ErrorActionPreference = 'Stop'
$msbuild, $vstest = .\tools\Build\Find-VisualStudio.ps1
.\tools\Build\Test-RhetosPrerequisites.ps1

# Build:
& NuGet.exe restore 'Bookstore.sln' -NonInteractive
if ($LastExitCode -ne 0) { throw "NuGet restore failed." }
& $msbuild 'Bookstore.sln' /target:rebuild /p:Configuration=Debug /verbosity:minimal
if ($LastExitCode -ne 0) { throw "MSBuild failed." }

# Deploy:
.\tools\Build\Install-RhetosServer.ps1 2.12.0
& '.\dist\BookstoreRhetosServer\bin\DeployPackages.exe' /debug /nopause
if ($LastExitCode -ne 0) { throw "DeployPackages failed." }
