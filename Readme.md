# Bookstore

Bookstore is a demo application for Rhetos development platform.
This project can be used as a *prototype* for new Rhetos applications,
regarding its project structure and content.

For the most part, this application is developed by following tutorial articles
from official [Rhetos Wiki](https://github.com/Rhetos/Rhetos/wiki).

The Bookstore application in this demo is **a business service** (Bookstore.Service)
that implements business features and database.

* It does not contain front-end implementation. This is typically a part of the N-tier
  enterprise system where a front-end is developed as a separate application
  (for example, an Angular web app).
* It exposes all business features through REST API.
  Other web protocols can be included with additional Rhetos plugins packages.

Project structure (see *Bookstore.sln*):

* **Bookstore.Service** - Main application (business service).
* Bookstore.Service.Test - Integration tests.
* Bookstore.Concepts - Custom concepts library that extend Rhetos DSL programming language
  with features specific to this application.
* Bookstore.Algorithms - Example of a stand-alone library of custom features that the service uses.
* Bookstore.Algorithms.Test - Unit tests.
* Bookstore.Playground - Example of a command-line utility that uses the generated business
  object model from Bookstore.Service.

Aside from the project structure, please note the following key components that
most Rhetos applications should contain:

1. The build script `Build.ps1`, that does everything needed to produce the application binaries from the source in command prompt:
   1. It checks for installed prerequisites (MSBuild, database connection string, ...).
   2. Runs `MSBuild` to build all application components (new custom DSL concepts,
      and an external algorithm implemented in a separate DLL).
   3. Runs `rhetos.exe dbupdate` command to update the database.
2. The test script `Test.ps1`. It builds and runs the automated unit tests and the integration tests.

## Build

To build this application from source, run `.\Build.ps1` in PowerShell console.

* If the build fails, check the error message for instructions to setup any missing prerequisites.
* For additional information on build and development environment see
  [Development Environment Setup](https://github.com/Rhetos/Rhetos/wiki/Development-Environment-Setup).

The build output is a web application in `src\Bookstore.Service` subfolder.

* To setup the **IIS web application** follow the instructions in "IIS setup" chapter at
  [Development Environment Setup](https://github.com/Rhetos/Rhetos/wiki/Development-Environment-Setup),
  using `src\Bookstore.Service` for Rhetos server folder.

## Unit testing

Automated tests are executed by running `.\Test.ps1` in PowerShell console.

There are two kinds of tests in this project:

1. **Standard unit tests** (`test\Bookstore.Algorithms.Test`)
   that test the algorithm implemented in external assembly.
    * These tests are very fast and independent of the deployment environment.
2. **Integration tests** (`test\Bookstore.Service.Test`)
   that test the generated applications together with the database.
    * These tests can test full business processes, including the business logic
      that is implemented in the database, but are slower and need a database to run
      (the database connection is set up earlier during the Build).

There are some important considerations for the integration tests
that we apply to keep their complexity under control:

* Each test should insert its own data and not rely on pre-existing data in the database,
  or on the data inserted by other tests.
* Each test should clear its data when finished.
  This is done automatically by `RhetosTestContainer`,
  see that the constructor parameter `commitChanges` is `false` by default,
  which means that the SQL transaction will be rolled back at the end of the `using` block.
* The tests should not be affected by the existing data in the database.
  They can be executed on an empty database (after the application is deployed to it),
  or on database with some user-entered data.

## Run the application

Configure user authentication for your application:

* Option A) *(Recommended for quick-start)* Enable **anonymous access** by creating
 `rhetos-app.local.settings.json` in Bookstore.Service project folder, with the following content:
 `{ "Rhetos": { "AppSecurity": { "AllClaimsForAnonymous": true } } }`.
* Option B) If you want to use **Windows authentication with IIS**:
  * Run Visual Studio *as Administrator*. Open project properties => Web => Change from "ISS Express" to "Local IIS". On save answer Yes.
  * Open IIS Manager => Find your application => Authentication => Disable Anonymous, Enable Windows Authentication.
  * Open IIS Manager => Find your application => Basic settings => Change application pool to new RhetosAppPool that is configured to run with your development account (see IIS Setup)
  * In the table `Common.Principal` insert a record which has the `Name` column set to your username
  * Create file `rhetos-app.local.settings.json` in project folder, with the following content: `{ "Rhetos": { "AppSecurity": { "AllClaimsForUsers": "username@computername" } } }`, for your account and your machine name, to simplify testing. See instructions in
    [Suppressing permissions in a development environment](Basic-permissions#suppressing-permissions-in-a-development-environment).
* Option C) If you want to use **Windows authentication with IIS Express**:
  * Edit `.vs\<solution name>\config\applicationhost.config`
    * in element `anonymousAuthentication` set `enabled` attribute to `false`.
    * in element `windowsAuthentication` set `enabled` attribute to `true`.
  * In the table `Common.Principal` insert a record which has the `Name` column set to your username
  * Create file `rhetos-app.local.settings.json` in project folder, with the following content: `{ "Rhetos": { "AppSecurity": { "AllClaimsForUsers": "username@computername" } } }`, for your account and your machine name, to simplify testing. See instructions in
    [Suppressing permissions in a development environment](Basic-permissions#suppressing-permissions-in-a-development-environment).

Open Bookstore.sln in Visual Studio, right-click project "Bookstore.Service" and select "Set as Startup Project".

Start the web application with Debug => Start Debugging **(F5)**.

* Rhetos application homepage should open in browser, displaying Installed packages and Server status.
* In the browser address bar append `/rest/Bookstore/Book/` to display the list of records from the database table Bookstore.Book.
* See [Troubleshooting](https://github.com/Rhetos/Rhetos/wiki/Creating-new-WCF-Rhetos-application#troubleshooting) section to resolve any errors.

## Contributions

Before creating a pull request, please run `tools\Build\Test-CleanBuild.ps1` in PowerShell console,
to make sure there are not any errors when running a clean full build without cached output binaries.
