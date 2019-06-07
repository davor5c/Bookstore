param([Parameter(Mandatory=$true)][string]$rhetosVersion)

$ErrorActionPreference = 'Stop'

$uri = "https://github.com/Rhetos/Rhetos/releases/download/v$rhetosVersion/RhetosServer.$rhetosVersion.zip"
$zipName = "BookstoreRhetosServer.$rhetosVersion.zip"
$zipDestPath = "$PSScriptRoot\..\..\dist\$zipName"
$destPath = "$PSScriptRoot\..\..\dist\BookstoreRhetosServer"
$testFile = "$destPath\bin\DeployPackages.exe"

# Delete old version of Rhetos server to install a new one, while preserving certain .config, .linq and Logs files.
if ((Test-Path ($testFile)) -and (Get-Item ($testFile)).VersionInfo.ProductVersion -ne $rhetosVersion)
{
  & $PSScriptRoot\Remove-RhetosServer.ps1
}

if (!(Test-Path ($testFile)))
{
  if (!(Test-Path $zipDestPath))
  {
    if (!(Test-Path $destPath)) { mkdir $destPath }
    Write-Host "Downloading Rhetos server binaries v$rhetosVersion ..."
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    Invoke-WebRequest -Uri $uri -OutFile "$zipDestPath.partial"
    Rename-Item -Path "$zipDestPath.partial" -NewName $zipName
  }

  Expand-Archive -Path $zipDestPath -DestinationPath $destPath -Force
  if (!(Test-Path ($testFile))) { throw "Extracting '$zipDestPath' failed." }
  Remove-Item $zipDestPath
  Write-Host "Downloaded Rhetos server binaries v$rhetosVersion to $destPath."
}
else
{
  Write-Host "Rhetos server binaries are already downloaded at $destPath."
}
