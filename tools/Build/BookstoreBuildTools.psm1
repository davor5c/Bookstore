function Install-RhetosServer
{
  param([Parameter(Mandatory=$true)][string]$rhetosVersion)

  $uri = "https://github.com/Rhetos/Rhetos/releases/download/v$rhetosVersion/RhetosServer.$rhetosVersion.zip"
  $zipDestPath = ".\dist\BookstoreRhetosServer.$rhetosVersion.zip"
  $destPath = ".\dist\BookstoreRhetosServer"

  # TODO: Test if (Get-Item ($destPath + '\bin\Rhetos.dll')).VersionInfo.ProductVersion equals $rhetosVersion.
  # If not, delete old BookstoreRhetosServer, while preserving certain .config, .linq and Logs files.

  if (!(Test-Path ($destPath + '\bin\Rhetos.dll')))
  {
    if (!(Test-Path $zipDestPath))
    {
      if (!(Test-Path $destPath)) { mkdir $destPath }
      Write-Host "Downloading Rhetos server binaries v$rhetosVersion ..."
      [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
      Invoke-WebRequest -Uri $uri -OutFile $zipDestPath
    }

    Expand-Archive -Path $zipDestPath -DestinationPath $destPath
    Remove-Item $zipDestPath
    Write-Host "Downloaded Rhetos server binaries v$rhetosVersion to $destPath."
  }
  else
  {
    Write-Host "Rhetos server binaries are already downloaded at $destPath."
  }
}

function Test-RhetosPrerequisites
{
  $connectionStringsPath = '.\dist\BookstoreRhetosServer\bin\ConnectionStrings.config'
  if (!(Test-Path $connectionStringsPath))
  {
    throw "Prerequisites: Database connection string is not configured. Please create a Bookstore database, copy 'Template.ConnectionStrings.config' file to '$connectionStringsPath', and setup the connection string in this file."
  }
}
