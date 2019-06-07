# Deletes all cached binaries and runs the build and tests from scratch.

$ErrorActionPreference = 'Stop'

& "$PSScriptRoot\Remove-AllBinaries.ps1"

Set-Location "$PSScriptRoot\..\.."
.\Build.ps1
.\Test.ps1
