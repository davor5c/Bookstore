param([Parameter(Mandatory=$true)][string]$destPath)

# Delete old version of Rhetos server before installing a new one,
# while preserving certain .config, .linq and Logs files.

if (Test-Path "$destPath\bin")
  { Get-ChildItem "$destPath\bin" | Where-Object Name -ne 'ConnectionStrings.config' | Remove-Item -Recurse }
if (Test-Path "$destPath\GeneratedFilesCache")
  { Remove-Item "$destPath\GeneratedFilesCache" -Recurse }
if (Test-Path "$destPath\PackagesCache")
  { Remove-Item "$destPath\PackagesCache" -Recurse }
if (Test-Path "$destPath\Resources")
  { Remove-Item "$destPath\Resources" -Recurse }
if (Test-Path "$destPath\Web.config")
  { Copy-Item "$destPath\Web.config" ("$destPath\Web.config." + (Get-Date -Format "yyyyMMdd.HHmmss") + ".backup") }
$rootFiles = 'ChangeLog.md','Default.aspx','Global.asax','Readme.md','Rhetos Server DOM.linq','Rhetos Server SOAP.linq','RhetosService.svc','Template.RhetosPackages.config','Template.RhetosPackageSources.config','Web.config'
Get-ChildItem $destPath | Where-Object Name -In $rootFiles | Remove-Item

"Keeping files:"
Get-ChildItem $destPath -Recurse | Where-Object { ! $_.PSIsContainer } | Select-Object -ExpandProperty FullName
