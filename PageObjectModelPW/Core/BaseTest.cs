using Microsoft.Playwright;

namespace PageObjectModelPW.Core;

public abstract class BaseTest
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    protected IPage? Page;
    protected PageFactory? Pages;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
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
        Page = await _context.NewPageAsync();

        // Initialize the PageFactory with the current page
        Pages = new PageFactory(Page);
    }

    [TearDown]
    public async Task TearDown()
    {
        try
        {
            await _context?.CloseAsync()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during context disposal: {ex.Message}");
        }
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        try
        {
            await _browser?.CloseAsync()!;
            _playwright?.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during browser/playwright disposal: {ex.Message}");
        }
    }
}