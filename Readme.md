# Bookstore

Bookstore is a demo application for Rhetos development platform.

## Build

To build this application from source, run `.\Build.ps1` in PowerShell console.

* If the build fails, see the error description to setup any missing prerequisites.
* If the build fails with a **ConnectionString** error,
  follow the instructions in "Database setup" chapter at the
  [Development Environment Setup](https://github.com/Rhetos/Rhetos/wiki/Development-Environment-Setup#database-setup).

The build output is a web application in `dist\BookstoreRhetosServer` subfolder.

* To setup the **IIS web application** follow the instructions in "IIS setup" chapter at
  [Development Environment Setup](https://github.com/Rhetos/Rhetos/wiki/Development-Environment-Setup),
  using `dist\BookstoreRhetosServer` for Rhetos server folder.

See `docs\Build process diagram.vsdx` for overview of build & testing.

## Unit testing

Automated tests are executed by running `.\Test.ps1` in PowerShell console.

There are two kinds of tests in this project:

1. **Standard unit tests** (`test\Bookstore.Algorithms.Test`, part of `Bookstore.sln`)
   that test the algorithm implemented in external assembly.
    * These tests are very fast and independent of the deployment environment.
2. **Integration tests** (`test\Bookstore.ServerDom.Test\Bookstore.ServerDom.Test.sln`)
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

Before creating a pull request, please run `tools\Build\CleanBuildTest.ps1` in PowerShell console,
to make sure there are not any errors when running a clean full build without cached output binaries.
