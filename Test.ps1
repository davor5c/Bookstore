# Runs all unit tests.
# Prerequisites: Build.ps1.
# See Readme.md for more info.

"=== Setup ==="
$ErrorActionPreference = 'Stop'
$msbuild, $vstest = .\tools\Build\Find-VisualStudio.ps1

"=== Simple unit tests ==="
& $vstest 'test\Bookstore.Algorithms.Test\bin\Debug\Bookstore.Algorithms.Test.dll'
if ($LastExitCode -ne 0) { throw "vstest failed." }

"=== Integration tests ==="
& $vstest 'test\Bookstore.RhetosServer.Test\bin\Debug\Bookstore.RhetosServer.Test.dll'
if ($LastExitCode -ne 0) { throw "vstest failed." }
