
# Playwright with .NET - Page Object Model Reference Project

This project serves as a comprehensive reference for implementing and practicing Playwright with .NET using the Page Object Model (POM) design pattern.

## Project Overview

The project demonstrates automation testing of a car website (carwale.com) using:
- Playwright for .NET
- NUnit test framework
- Page Object Model design pattern
- Modern C# features

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- IDE (Visual Studio, Rider, or VS Code)

### Setup

1. Clone the repository
2. Install Playwright browsers:
   ```
   pwsh bin/Debug/net9.0/playwright.ps1 install
   ```
   or
   ```
   dotnet tool install --global Microsoft.Playwright.CLI
   playwright install
   ```

3. Build the project:
   ```
   dotnet build
   ```

## Project Structure

- **Pages/**: Contains Page Object classes
  - `HomePage.cs`: Page object for the website's home page
- **Testcases/**: Contains test classes
  - `FindNewCarTest.cs`: Sample test for finding new cars

## Key Learnings

### 1. Page Object Model Implementation

```csharp
// Pages/HomePage.cs
public class HomePage(IPage page)
{
    // Locators
    private ILocator Banner => page.GetByRole(AriaRole.Banner);
    private ILocator NewCarsLink => Banner.GetByText("NEW CARS");

    // Methods
    public async Task FindNewCars()
    {
        await NewCarsLink.HoverAsync();
        await FindNewCarsLink.ClickAsync();
    }
}
```

**Benefits of POM:**
- Separation of concerns
- Reusable page interactions
- Improved test maintenance
- Better readability

### 2. Proper Test Structure

```csharp
[Test]
public async Task FindNewCar_ShouldNavigateToNewCarsPage()
{
    // Arrange
    await _page!.GotoAsync("https://www.carwale.com/");
    var homePage = new HomePage(_page);

    // Act
    await homePage.FindNewCars();

    // Assert
    await Assertions.Expect(_page).ToHaveURLAsync(new Regex(".*new-cars.*"));
}
```

**Best Practices:**
- Following AAA (Arrange-Act-Assert) pattern
- Descriptive test names
- Single responsibility per test

### 3. Test Lifecycle Management

```csharp
[OneTimeSetUp]
public async Task OneTimeSetUp()
{
    _playwright = await Playwright.CreateAsync();
    _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
    {
        Headless = Environment.GetEnvironmentVariable("HEADLESS") != "false"
    });
}

[SetUp]
public async Task SetUp()
{
    _context = await _browser!.NewContextAsync(new BrowserNewContextOptions
    {
        ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
    });
    _page = await _context.NewPageAsync();
}
```

**Best Practices:**
- Using `OneTimeSetUp` for browser initialization
- Using `SetUp` for context and page creation
- Proper resource management
- Error handling in teardown methods

### 4. Playwright Best Practices

1. **Browser Context Isolation**
   - Each test gets a fresh browser context

2. **Resource Management**
   - Proper disposal of Playwright resources

3. **Configurable Browser Options**
   - Environment-based configuration for headless mode

4. **Error Handling**
   - Try-catch blocks in teardown methods

5. **Flexible Test Configuration**
   - Environment variables for runtime configuration

## Running Tests

### Run Tests in Headless Mode (Default)
```
dotnet test
```

### Run Tests in Headed Mode
```
# Windows (Command Prompt)
set HEADLESS=false
dotnet test

# Windows (PowerShell)
$env:HEADLESS="false"
dotnet test

# macOS/Linux
export HEADLESS=false
dotnet test
```

## Advanced Features

### 1. Assertions
Playwright provides powerful assertion capabilities:

```csharp
// Verify URL
await Assertions.Expect(_page).ToHaveURLAsync(new Regex(".*new-cars.*"));

// Verify element visibility
await Assertions.Expect(element).ToBeVisibleAsync();

// Verify text content
await Assertions.Expect(element).ToContainTextAsync("Expected Text");
```

### 2. Handling Timeouts
```csharp
// Global timeout configuration
_page.SetDefaultTimeout(30000);

// Per-action timeout
await _page.ClickAsync("#button", new PageClickOptions { Timeout = 5000 });
```

### 3. Parallel Test Execution
NUnit supports parallel test execution, which works well with Playwright's isolated contexts.

## Best Practices and Coding Standards

1. **Naming Conventions**
   - Use `_underscorePrefix` for private fields
   - Descriptive method names for page actions
   - Descriptive test names following behavior

2. **Code Organization**
   - Separate page objects from test logic
   - Group related locators and methods

3. **Error Handling**
   - Proper try-catch blocks in teardown methods
   - Graceful resource disposal

4. **Configuration Management**
   - Environment variables for test configuration
   - Flexible browser options

## Resources

- [Playwright for .NET Documentation](https://playwright.dev/dotnet/docs/intro)
- [NUnit Documentation](https://docs.nunit.org/)
- [Page Object Model Best Practices](https://www.selenium.dev/documentation/test_practices/encouraged/page_object_models/)

## Contributing

Feel free to contribute to this project by adding more page objects, test cases, or improving existing code.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
