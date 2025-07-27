using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using NUnit.Framework.Interfaces;

namespace PageObjectModelPW.Core;

public abstract class BaseTest
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _context;
    protected IPage Page;

    private static ExtentReports _extentReports;
    public static ExtentTest ExtentTest;
    private static readonly ILog Log = LogManager.GetLogger(typeof(BaseTest));
    protected IConfiguration Configuration;
    private static string _fileName;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Log.Info("Test Execution Started !!!");

        Configuration = new ConfigurationBuilder()
            .SetBasePath($"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}/Resources")
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var currentTime = DateTime.Now;
        _fileName = $"Extent_{currentTime:yyyy-MM-dd_HH-mm-ss}.html";
        _extentReports = CreateInstance(_fileName);
    }

    private static ExtentReports CreateInstance(string fileName)
    {
        var htmlReporter =
            new ExtentSparkReporter(
                $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}/Reports/{fileName}")
            {
                Config =
                {
                    Theme = Theme.Standard,
                    DocumentTitle = "Way2Automation Test Suite",
                    ReportName = "Automation Test Results",
                    Encoding = "utf-8"
                }
            };

        _extentReports = new ExtentReports();
        _extentReports.AttachReporter(htmlReporter);

        _extentReports.AddSystemInfo("Automation Tester", "André Kreutzer");
        _extentReports.AddSystemInfo("Organization", "Way2Automation");
        _extentReports.AddSystemInfo("Build No: ", "W2A-1234");

        return _extentReports;
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        try
        {
            _extentReports.Flush();
            Log.Info("Test Execution completed !!!");
        }
        catch (Exception ex)
        {
            Log.Error($"ExtentReports cleanup failed: {ex.Message}");
        }
    }

    [SetUp]
    public async Task BeforeEachTest()
    {
        ExtentTest = _extentReports.CreateTest(
            $"{TestContext.CurrentContext.Test.ClassName} - {TestContext.CurrentContext.Test.Name}");

        _playwright = await Playwright.CreateAsync();
    }

    private static async Task CaptureScreenshot(IPage page)
    {
        DateTime currentTime = DateTime.Now;
        _fileName = currentTime.ToString("yyyy-MM-dd_HH-mm-ss") + ".jpg";

        await page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path =
                $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}/Screenshot/{_fileName}"
        });
    }

    [TearDown]
    public async Task AfterEachTest()
    {
        try
        {
            //Get the test status
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message;

            switch (testStatus)
            {
                case TestStatus.Passed:
                    ExtentTest.Pass("Test Passed");

                    IMarkup markup = MarkupHelper.CreateLabel("PASS", ExtentColor.Green);
                    ExtentTest.Pass(markup);
                    break;

                case TestStatus.Skipped:
                    ExtentTest.Skip($"Test Skipped: {message}");
                    markup = MarkupHelper.CreateLabel("SKIP", ExtentColor.Amber);
                    ExtentTest.Skip(markup);
                    break;

                case TestStatus.Failed:
                    ExtentTest.Fail($"Test Failed: {message}");
                    await CaptureScreenshot(Page);
                    markup = MarkupHelper.CreateLabel("FAIL", ExtentColor.Red);
                    ExtentTest.Fail(markup);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while defining test status: {ex.Message}");
        }

        try
        {
            _playwright.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during playwright disposal: {ex.Message}");
        }
    }

    protected async Task<(IBrowser _browser, IPage Page)> CreateBrowserAndPage(IPlaywright playwrightInstance, string browserType,
        BrowserTypeLaunchOptions? launchOptions = null)
    {
        if (browserType.Equals("chrome", StringComparison.OrdinalIgnoreCase))
        {
            _browser = await playwrightInstance.Chromium.LaunchAsync(launchOptions);
        }
        else if (browserType.Equals("firefox", StringComparison.OrdinalIgnoreCase))
        {
            _browser = await playwrightInstance.Firefox.LaunchAsync(launchOptions);
        }
        else
        {
            Assert.Fail($"Invalid browser type: {browserType}");
            return (null, null);
        }


        Page = await _browser.NewPageAsync();
        await Page.SetViewportSizeAsync(1920, 1080);
        await Page.GotoAsync(Configuration["Appsettings:testsiteurl"] ?? string.Empty);

        return (_browser, Page);
    }
}