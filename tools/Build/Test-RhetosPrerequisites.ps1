$templateAppLocalSettingsPath = Resolve-Path "$PSScriptRoot\..\Configs\Templates\Template.rhetos-app.local.settings.json"
$serverFolderLocation = Resolve-Path "$PSScriptRoot\..\..\src\Bookstore.Service"
$appLocalSettingsPath = "$serverFolderLocation\rhetos-app.local.settings.json"

if (!(Test-Path $appLocalSettingsPath))
{
  $msg = "Bookstore Prerequisites: Database connection string is not configured." `
    + " Please create an empty Bookstore database, copy '$templateAppLocalSettingsPath' file" `
    + " to '$appLocalSettingsPath', setup the connection string in that file, and build the project again."
  throw $msg
}
