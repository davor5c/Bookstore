# Runs all unit tests.
# Prerequisites: Build.ps1.
# See Readme.md for more info.

# Setup:
$ErrorActionPreference = 'Stop'
$msbuild, $vstest = .\tools\Build\Find-VisualStudio.ps1

# Simple unit tests:
& $vstest 'test\Bookstore.Algorithms.Test\bin\Debug\Bookstore.Algorithms.Test.dll'
if ($LastExitCode -ne 0) { throw "vstest failed." }

# Integration tests:
& NuGet.exe restore 'test\Bookstore.ServerDom.Test\Bookstore.ServerDom.Test.sln' -NonInteractive
if ($LastExitCode -ne 0) { throw "NuGet restore failed." }
& $msbuild 'test\Bookstore.ServerDom.Test\Bookstore.ServerDom.Test.sln' /target:rebuild /p:Configuration=Debug /verbosity:minimal
if ($LastExitCode -ne 0) { throw "MSBuild failed." }
& $vstest 'test\Bookstore.ServerDom.Test\bin\Debug\Bookstore.ServerDom.Test.dll'
if ($LastExitCode -ne 0) { throw "vstest failed." }
