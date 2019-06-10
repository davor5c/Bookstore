# Returns the Visual Studio tools location

$ErrorActionPreference = 'Stop'

$vswhere = "${Env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
$vsPath = & $vswhere -latest -property installationPath

$msbuild = "$vsPath\MSBuild\15.0\Bin\MSBuild.exe"
$vstest = "$vsPath\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

return $msbuild, $vstest
