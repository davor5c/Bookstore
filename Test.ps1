# Runs all unit tests.
# Prerequisites: Build.ps1.
# See Readme.md for more info.

"=== Setup ==="
$ErrorActionPreference = 'Stop'

"=== Run all tests ==="
& dotnet test Bookstore.sln --no-build
if ($LastExitCode -ne 0) { throw "MSTest failed." }
