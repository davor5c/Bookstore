# Runs all unit tests.
# Prerequisites: Build.ps1.
# See Readme.md for more info.

"=== Setup ==="
$ErrorActionPreference = 'Stop'
$msbuild, $vstest = .\tools\Build\Find-VisualStudio.ps1

"=== Run tests ==="
& $vstest 'test\Bookstore.Service.Test\bin\Debug\Bookstore.Service.Test.dll'
if ($LastExitCode -ne 0) { throw "vstest failed." }
