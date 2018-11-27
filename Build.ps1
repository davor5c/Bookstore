# Set-PSDebug -Trace 1
$ErrorActionPreference = 'Stop'

# TODO: Detect available VS versions.
$msbuild = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe'

Try
{
  Import-Module .\tools\Build\BookstoreBuildTools.psm1
  Install-RhetosServer 2.9.0
  Test-RhetosPrerequisites

  & NuGet.exe restore 'Bookstore.sln' -NonInteractive
  & $msbuild 'Bookstore.sln' /target:rebuild /p:Configuration=Debug /verbosity:minimal

  & '.\dist\BookstoreRhetosServer\bin\DeployPackages.exe' /debug /nopause
}
Finally
{
  Remove-Module -Name BookstoreBuildTools
}
