# Builds binaries and deploys the Bookstore application on it.
# Prerequisites: VS2017 or newer, with MSBuild.
# See Readme.md for more info.

"=== Setup ==="
$ErrorActionPreference = 'Stop'

"=== Build ==="
# Disabled database update (RhetosDeploy) in build script. It will be executed later when testing.
& dotnet build 'Bookstore.sln' -p:Configuration=Debug -p:RhetosDeploy=False
if ($LastExitCode -ne 0) { throw "MSBuild failed." }
