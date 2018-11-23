Set-PSDebug -Trace 1
$ErrorActionPreference = "Stop"

Try
{
  Import-Module .\tools\Build\BookstoreBuildTools.psm1
  Install-RhetosServer 2.9.0

  $connectionStringsPath = '.\dist\BookstoreRhetosServer\bin\ConnectionStrings.config'
  if (!(Test-Path $connectionStringsPath))
  {
    throw "Database connection string is not congifured. Please create a Bookstore database, copy 'Template.ConnectionStrings.config' file to '$connectionStringsPath', and setup the connection string in this file."
  }

  & '.\dist\BookstoreRhetosServer\bin\DeployPackages.exe' /nopause
  if (-not $?)
  {
      throw 'deployment failed'
  }
}
Finally
{
  Remove-Module -Name BookstoreBuildTools
}
