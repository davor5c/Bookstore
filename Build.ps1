# Set-PSDebug -Trace 1
$ErrorActionPreference = "Stop"

Try
{
  Import-Module .\tools\Build\BookstoreBuildTools.psm1

  Install-RhetosServer 2.9.0
  Test-Prerequisites
  Build-RhetosServer
}
Finally
{
  Remove-Module -Name BookstoreBuildTools
}
