$connectionStringsPath = "$PSScriptRoot\..\..\dist\BookstoreRhetosServer\bin\ConnectionStrings.config"

if (!(Test-Path $connectionStringsPath))
{
  $msg = "Prerequisites: Database connection string is not configured." `
    + " Please create a Bookstore database, copy 'Template.ConnectionStrings.config' file" `
    + " to '$connectionStringsPath', and setup the connection string in that file."
  throw $msg
}
