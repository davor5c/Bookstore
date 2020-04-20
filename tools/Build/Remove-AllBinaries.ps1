# Deletes all build outputs and temporary files, for testing and debugging purpose.

$ErrorActionPreference = 'Stop'

# Obsolete NuGet cache folder. It might remain from an older Bookstore version.
$nugetCacheFolders = Get-ChildItem "$PSScriptRoot\..\.." -Recurse -Directory -Filter "packages"
$nugetCacheFolders | Remove-Item -Recurse -Force

# Obsolete build output folder. It might remain from an older Bookstore version.
if (Test-Path "$PSScriptRoot\..\..\dist") {
    Remove-Item -Path "$PSScriptRoot\..\..\dist" -Recurse -Force
}

# Build intermediate and output folders.
$binFolders = Get-ChildItem "$PSScriptRoot\..\..\src", "$PSScriptRoot\..\..\test" -Recurse -Directory -In "bin", "obj"
$binFolders | Remove-Item -Recurse -Force
