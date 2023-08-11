# Enaxis - Selenium Test Automation

## Overview

This project is a work in progress Selenium test automation solution written in C# using .NET 6. It aims to automate testing for Enaxis webapps. 

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Google Chrome](https://www.google.com/chrome/) Version 115.0.5790.110 (Official Build) (64-bit) (for running tests using Chrome WebDriver) 

## Setup Instructions

1. Clone the repository:
   `git clone https://github.com/AgusR-99/EnaxisSelenium.git`
2. Build the project:
   `dotnet build`
3. Create `appsettings.json` file:
   - At the path "EnaxisSelenium\EnaxisSelenium\bin\Debug\net6.0" of the project, create a file named `appsettings.json`.
   - Copy the contents from `appsettings.template.json` and replace the placeholder values with your actual login credentials for the application being tested.

   **Note:** Never commit the `appsettings.json` file to version control to avoid exposing sensitive information. It is already added to the .gitignore file.

4. Run the tests:
`dotnet test`
The tests will use the configurations provided in `appsettings.json` to perform login and testing.

## Test Scenarios
1. Login Test: The login test will verify that the user can successfully log in to the web application using the correct credentials.
2. Table Sorting Test: The sorting test will verify that each column in the table can be sorted in ascending and descending order. It will validate that the data is correctly ordered after sorting.
3. Table Dropdown Filter Test: The dropdown filter test will verify that each dropdown filter in the table's filter row can be selected and applied. It will check for any system errors that might happen.
4. Table Searchbox Filter Test: The searchbox filter test will verify that each searchbox filter in the table's filter row can be used to search for specific text. It will check for any system errors that might happen.

## Notes
1. The test suite is designed to handle only dropdown and search box filters at the moment. Additional filter types may be supported in future updates.
2. The tests will only execute if there is a compatible table present on the page.

## Contributing

If you find any issues or have suggestions for improvements, feel free to open an issue or submit a pull request.
