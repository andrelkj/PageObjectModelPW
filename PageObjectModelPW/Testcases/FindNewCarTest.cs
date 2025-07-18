using Microsoft.Playwright;
using PageObjectModelPW.Pages;

namespace PageObjectModelPW.Testcases;

[TestFixture]
public class FindNewCarTest
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;

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

    [TearDown]
    public async Task TearDown()
    {
        try
        {
            if (_page != null)
                await _page.CloseAsync();
            if (_context != null)
                await _context.CloseAsync();
        }
        catch (Exception ex)
        {
            await TestContext.Out.WriteLineAsync($"Error during teardown: {ex.Message}");
        }
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        try
        {
            if (_browser != null)
                await _browser.CloseAsync();
            _playwright?.Dispose();
        }
        catch (Exception ex)
        {
            await TestContext.Out.WriteLineAsync($"Error during one-time teardown: {ex.Message}");
        }
    }

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
}
