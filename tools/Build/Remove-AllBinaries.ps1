# Deletes all build outputs.

$ErrorActionPreference = 'Stop'

& "$PSScriptRoot\Remove-RhetosServer.ps1"

if (Test-Path "$PSScriptRoot\..\..\packages")
  { Remove-Item "$PSScriptRoot\..\..\packages" -Recurse }

$binFolders = Get-ChildItem "$PSScriptRoot\..\..\src", "$PSScriptRoot\..\..\test" -Recurse -Directory -In "bin", "obj"
$binFolders | Remove-Item -Recurse -Force
