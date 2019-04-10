param([Parameter(Mandatory=$true)][string]$rhetosVersion)

$uri = "https://github.com/Rhetos/Rhetos/releases/download/v$rhetosVersion/RhetosServer.$rhetosVersion.zip"
$zipDestPath = "$PSScriptRoot\..\..\dist\BookstoreRhetosServer.$rhetosVersion.zip"
$destPath = "$PSScriptRoot\..\..\dist\BookstoreRhetosServer"

# Delete old version of Rhetos server to install a new one, while preserving certain .config, .linq and Logs files.
if ((Test-Path ($destPath + '\bin\Rhetos.dll')) -and (Get-Item ($destPath + '\bin\Rhetos.dll')).VersionInfo.ProductVersion -ne $rhetosVersion)
{
  & $PSScriptRoot\Remove-RhetosServer.ps1 $destPath
}

if (!(Test-Path ($destPath + '\bin\Rhetos.dll')))
{
  if (!(Test-Path $zipDestPath))
  {
    if (!(Test-Path $destPath)) { mkdir $destPath }
    Write-Host "Downloading Rhetos server binaries v$rhetosVersion ..."
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    Invoke-WebRequest -Uri $uri -OutFile $zipDestPath
  }

  Expand-Archive -Path $zipDestPath -DestinationPath $destPath -Force
  Remove-Item $zipDestPath
  Write-Host "Downloaded Rhetos server binaries v$rhetosVersion to $destPath."
}
else
{
  Write-Host "Rhetos server binaries are already downloaded at $destPath."
}
