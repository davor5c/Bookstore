# Delete old version of Rhetos server before installing a new one,
# while preserving certain .config, .linq and Logs files.

$ErrorActionPreference = 'Stop'
$destPath = "$PSScriptRoot\..\..\dist\BookstoreRhetosServer"

if (Test-Path "$destPath\bin")
  { Get-ChildItem "$destPath\bin" | Where-Object Name -ne 'ConnectionStrings.config' | Remove-Item -Recurse }
if (Test-Path "$destPath\GeneratedFilesCache")
  { Remove-Item "$destPath\GeneratedFilesCache" -Recurse }
if (Test-Path "$destPath\PackagesCache")
  { Remove-Item "$destPath\PackagesCache" -Recurse }
if (Test-Path "$destPath\Resources")
  { Remove-Item "$destPath\Resources" -Recurse }
if (Test-Path "$destPath\Web.config")
{
  $sameBackupExists = $false;
  if (Test-Path "$destPath\Web.config*.backup")
  {
    $lastBackup = Get-ChildItem "$destPath\Web.config*.backup" | Sort-Object Name | Select-Object -Last 1 -ExpandProperty FullName
    $diff = Compare-Object -ReferenceObject $(Get-Content "$destPath\Web.config") -DifferenceObject $(Get-Content $lastBackup)
    if ($diff.Length -eq 0)
      { $sameBackupExists = $true }
  }
  if (!$sameBackupExists)
    { Copy-Item "$destPath\Web.config" ("$destPath\Web.config." + (Get-Date -Format "yyyyMMdd.HHmmss") + ".backup") }
}

$rootFiles = 'ChangeLog.md','Default.aspx','Global.asax','Readme.md','Rhetos Server DOM.linq','Rhetos Server SOAP.linq','RhetosService.svc','Template.RhetosPackages.config','Template.RhetosPackageSources.config','Web.config'
Get-ChildItem $destPath | Where-Object Name -In $rootFiles | Remove-Item

"Keeping files:"
Get-ChildItem $destPath -Recurse | Where-Object { ! $_.PSIsContainer } | Select-Object -ExpandProperty FullName
