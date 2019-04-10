# Set-PSDebug -Trace 1
$ErrorActionPreference = 'Stop'

# TODO: Detect available VS versions.
$msbuild = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe'

.\tools\Build\Install-RhetosServer.ps1 2.11.0
.\tools\Build\Test-RhetosPrerequisites.ps1

& NuGet.exe restore 'Bookstore.sln' -NonInteractive
& $msbuild 'Bookstore.sln' /target:rebuild /p:Configuration=Debug /verbosity:minimal

& '.\dist\BookstoreRhetosServer\bin\DeployPackages.exe' /debug /nopause
