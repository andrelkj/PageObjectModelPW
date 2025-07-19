
# Playwright with .NET - Page Object Model Reference Project

This project serves as a comprehensive reference for implementing and practicing Playwright with .NET using the Page Object Model (POM) design pattern and Page Factory pattern.

## Project Overview

The project demonstrates automation testing of a car website (carwale.com) using:
- Playwright for .NET
- NUnit test framework
- Page Object Model design pattern
- Page Factory pattern for efficient page object management
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

- **Core/**: Contains infrastructure classes
  - `BaseTest.cs`: Base class with Playwright setup/teardown
  - `PageFactory.cs`: Manages page object instantiation and caching
- **Pages/**: Contains Page Object classes
  - `HomePage.cs`: Page object for the website's home page
- **Testcases/**: Contains test classes
  - `FindNewCarTest.cs`: Sample test for finding new cars

## Key Learnings

### 1. Page Factory Pattern Implementation

The Page Factory pattern provides a way to efficiently manage page objects:

```csharp
// Core/PageFactory.cs
public class PageFactory(IPage page)
{
    // Caching mechanism
    private readonly Dictionary<Type, object> _pages = new();

    // Generic page creation
    private T GetPage<T>() where T : class
    {
        var pageType = typeof(T);

        if (!_pages.TryGetValue(pageType, out var page))
        {
            page = CreatePage<T>();
            _pages[pageType] = page;
        }

        return (T)page;
    }

    // Convenience properties for common pages
    public HomePage HomePage => GetPage<HomePage>();
}
```

**Benefits of Page Factory:**
- **Centralized page object management**: All page objects are created and managed in one place
- **Lazy initialization**: Page objects are only created when needed
- **Caching**: Page objects are reused rather than recreated for each test step
- **Clean test code**: Tests focus on behavior rather than object creation

### 2. Page Object Model Implementation

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

### 3. Proper Test Structure

```csharp
[TestFixture]
public class FindNewCarTest : BaseTest // Inherit from BaseTest
{
    [Test]
    public async Task FindNewCar_ShouldNavigateToNewCarsPage()
    {
        // Arrange
        await _page!.GotoAsync("https://www.carwale.com/");

        // Act - Using the PageFactory instead of direct instantiation
        await Pages!.HomePage.FindNewCars();

        // Assert
        await Assertions.Expect(_page).ToHaveURLAsync(new Regex(".*new-cars.*"));
    }
}
```

**Best Practices:**
- Following AAA (Arrange-Act-Assert) pattern
- Descriptive test names
- Single responsibility per test

### 4. Test Lifecycle Management with BaseTest

```csharp
public abstract class BaseTest
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;
    protected PageFactory? Pages;

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

        // Initialize the PageFactory with the current page
        Pages = new PageFactory(_page);
    }

    // TearDown and OneTimeTearDown methods for cleanup
}
```

**Benefits of BaseTest:**
- **Inherit once, use everywhere**: All test classes just inherit from BaseTest
- **Zero code duplication**: No need to duplicate setup/teardown in test classes
- **Consistent test environment**: All tests use the same configuration
- **Automatic PageFactory initialization**: Pages ready to use in all tests
- **Proper resource management**: Centralized cleanup in teardown methods

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

### 1. Page Factory Advanced Usage

```csharp
// Adding multiple page types
public class PageFactory(IPage page)
{
    // ... existing code ...

    // Add convenience properties for all your pages
    public HomePage HomePage => GetPage<HomePage>();
    public SearchResultsPage SearchResultsPage => GetPage<SearchResultsPage>();
    public CarDetailsPage CarDetailsPage => GetPage<CarDetailsPage>();
}

// Using multiple pages in a test flow
[Test]
public async Task ComplexUserJourney()
{
    await _page!.GotoAsync("https://www.carwale.com/");

    // Chain operations across different pages
    await Pages!.HomePage.SearchCars();
    await Pages!.SearchResultsPage.SelectFirstCar();
    await Pages!.CarDetailsPage.ViewSpecifications();

    // Assert final state
    await Assertions.Expect(_page).ToHaveURLAsync(new Regex(".*specifications.*"));
}
```

**Adding New Pages:**
1. Create the page class with an `IPage` constructor parameter
2. Add a convenience property to the `PageFactory` class
3. Access it in tests via `Pages!.NewPageName`

### 2. Assertions
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

1. **Architecture**
   - Inherit from `BaseTest` - never duplicate setup/teardown
   - Use `Pages!.PageName` instead of `new PageName(_page)`
   - Keep page objects focused on single responsibility

2. **Naming Conventions**
   - Use `_underscorePrefix` for private fields
   - Descriptive method names for page actions
   - Descriptive test names following behavior

3. **Code Organization**
   - Separate page objects from test logic
   - Group related locators and methods
   - Use PageFactory for all page access

4. **Error Handling**
   - BaseTest handles all resource disposal
   - Graceful error handling in teardown methods

5. **Configuration Management**
   - Environment variables for test configuration
   - Flexible browser options in BaseTest

## Comparison: Before vs After

### Before (Manual Instantiation)
```csharp
[TestFixture]
public class FindNewCarTest
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;

    [OneTimeSetUp] // Duplicate setup code
    [SetUp]        // Duplicate setup code
    [Test]
    public async Task Test()
    {
        var homePage = new HomePage(_page); // Manual instantiation
        await homePage.FindNewCars();
    }
    [TearDown]     // Duplicate teardown code
    [OneTimeTearDown] // Duplicate teardown code
}
```

### After (PageFactory + BaseTest)
```csharp
[TestFixture]
public class FindNewCarTest : BaseTest // Just inherit!
{
    [Test]
    public async Task Test()
    {
        await Pages!.HomePage.FindNewCars(); // Clean and simple!
    }
    // No setup/teardown needed!
}
```

## Resources

- [Playwright for .NET Documentation](https://playwright.dev/dotnet/docs/intro)
- [NUnit Documentation](https://docs.nunit.org/)
- [Page Object Model Best Practices](https://www.selenium.dev/documentation/test_practices/encouraged/page_object_models/)

## Contributing

Feel free to contribute to this project by adding more page objects, test cases, or improving existing code.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
