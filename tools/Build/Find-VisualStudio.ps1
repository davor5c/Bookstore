# Returns the Visual Studio tools location

$ErrorActionPreference = 'Stop'

$vswhere = "${Env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
$vsPath = & $vswhere -latest -property installationPath

$msbuild = Get-ChildItem -Path "$vsPath\MSBuild\*\Bin\MSBuild.exe" -Recurse | Where-Object FullName -Like '*Bin\MSBuild.exe' | Select-Object -First 1 -ExpandProperty FullName
$vstest = "$vsPath\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

return $msbuild, $vstest
