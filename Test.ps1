# Set-PSDebug -Trace 1
$ErrorActionPreference = 'Stop'

# TODO: Detect available VS versions.
$msbuild = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe'
$vstest = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe'

& $vstest 'test\Bookstore.Algorithms.Test\bin\Debug\Bookstore.Algorithms.Test.dll'

& $msbuild 'test\Bookstore.ServerDom.Test\Bookstore.ServerDom.Test.sln' /target:rebuild /p:Configuration=Debug /verbosity:minimal
& $vstest 'test\Bookstore.ServerDom.Test\bin\Debug\Bookstore.ServerDom.Test.dll'
