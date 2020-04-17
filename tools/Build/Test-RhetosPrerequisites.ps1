$templateConnectionStringsPath = Resolve-Path "$PSScriptRoot\..\Configs\Templates\Template.ConnectionStrings.config"
$serverFolderLocation = Resolve-Path "$PSScriptRoot\..\..\src\Bookstore.RhetosServer"
$connectionStringsPath = "$serverFolderLocation\ConnectionStrings.config"

if (!(Test-Path $connectionStringsPath))
{
  $msg = "Prerequisites: Database connection string is not configured." `
    + " Please create a Bookstore database, copy '$templateConnectionStringsPath' file" `
    + " to '$connectionStringsPath', and setup the connection string in that file."
  throw $msg
}
