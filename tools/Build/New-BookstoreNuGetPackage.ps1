# This script packs the Bookstore source into a NuGet package, with specification from src\Bookstore.nuspec.
# DeployPackages.exe should use the created package, instead of deploying directly from src folder (see RhetosPackages.config).
# This simplifies deployment on production with /DatabaseOnly switch, by making sure that all assets are in NuGet packages or in PackagesCache folder (if server is copied directly).

$ErrorActionPreference = 'Stop'

# Packing the files with an older version of nuget.exe for backward compatibility (spaces in file names, https://github.com/Rhetos/Rhetos/issues/80).
if (!(Test-Path ('dist\NuGet.exe')))
{
  (New-Object System.Net.WebClient).DownloadFile('https://dist.nuget.org/win-x86-commandline/v4.5.1/nuget.exe', 'dist\NuGet.exe')
}

& dist\NuGet.exe pack src\Bookstore.nuspec -OutputDirectory dist\BookstorePackage
if ($LastExitCode -ne 0) { throw "NuGet pack failed." }

# Remove the cached NuGet package, since we don't update the package version on each build.
if (Test-Path 'dist\BookstoreRhetosServer\PackagesCache\Bookstore.*')
  { Remove-Item 'dist\BookstoreRhetosServer\PackagesCache\Bookstore.*' -Recurse }
