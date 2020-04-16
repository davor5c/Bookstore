# Bookstore

Bookstore is a demo application for Rhetos development platform.

You can use it as a *prototype* for a new Rhetos application.
Aside from the project structure, please note the following key components that
most Rhetos applications should contain:

1. The build script `Build.ps1`, that does everything needed to produce the application binaries from the source:
   1. It checks for installed prerequisites (MSBuild, NuGet, database connection string, ...).
   2. Runs MSBuild to build all application components (new custom DSL concepts,
      and an external algorithm implemented in a separate dll).
   3. Runs rhetos.exe dbupdate command to update the databse
2. The NuGet specification file `src\Bookstore.nuspec`.
   It specifies the list of application components that will be deployed to the Rhetos server.
   More info at [Creating a Rhetos package](https://github.com/Rhetos/Rhetos/wiki/Creating-a-Rhetos-package)
3. The test script `Test.ps1`. It builds and runs the automated unit tests and the integration tests.

See `docs\Build process diagram.vsdx` for an overview of build & testing.

## Build

To build this application from source, run `.\Build.ps1` in PowerShell console.

* If the build fails, see the error description to setup any missing prerequisites.
* If the build fails with a **ConnectionString** error,
  follow the instructions in "Database setup" chapter at the
  [Development Environment Setup](https://github.com/Rhetos/Rhetos/wiki/Development-Environment-Setup#database-setup).

The build output is a web application in `src\Bookstore.RhetosServer` subfolder.

* To setup the **IIS web application** follow the instructions in "IIS setup" chapter at
  [Development Environment Setup](https://github.com/Rhetos/Rhetos/wiki/Development-Environment-Setup),
  using `src\Bookstore.RhetosServer` for Rhetos server folder.

## Unit testing

Automated tests are executed by running `.\Test.ps1` in PowerShell console.

There are two kinds of tests in this project:

1. **Standard unit tests** (`test\Bookstore.Algorithms.Test`)
   that test the algorithm implemented in external assembly.
    * These tests are very fast and independent of the deployment environment.
2. **Integration tests** (`test\Bookstore.ServerDom.Test`)
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

## Contributions

Before creating a pull request, please run `tools\Build\Test-CleanBuild.ps1` in PowerShell console,
to make sure there are not any errors when running a clean full build without cached output binaries.
