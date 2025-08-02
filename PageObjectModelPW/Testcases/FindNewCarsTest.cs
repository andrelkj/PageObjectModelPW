using Microsoft.Playwright;
using PageObjectModelPW.Core;
using PageObjectModelPW.Pages;
using PageObjectModelPW.Utilities;

namespace PageObjectModelPW.Testcases;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class FindNewCarsTest : BaseTest
{
    [Parallelizable(ParallelScope.Self)]
    [Test, TestCaseSource(nameof(GetTestData)), Category("BVT")]
    public async Task FindCarTest(string carbrand, string browserType, string runmode, string carTitle)
    {
        if (runmode.Equals("N"))
        {
            Assert.Ignore("Ignoring the test once run mode is set to NO");
        }

        // Each test gets a new Playwright instance
        using var playwrightInstance = await Playwright.CreateAsync();

        var (browser, page) = await CreateBrowserAndPage(playwrightInstance, browserType,
            new BrowserTypeLaunchOptions { Headless = false });

        var homePage = new HomePage(Page);
        var newCarsPage = await homePage.FindNewCars();

        var carBrandActions = new Dictionary<string, Func<Task>>
        {
            { "bmw", async () => await newCarsPage.GoToBmw() },
            { "toyota", async () => await newCarsPage.GoToToyota() },
            { "mg", async () => await newCarsPage.GoToMg() }
        };

        try
        {
            // using dictionary
            if (carBrandActions.TryGetValue(carbrand.ToLower(), out var navigateToCar))
            {
                await navigateToCar();
                var receivedCarTitle = await newCarsPage.GetCarTitle();
                Assert.That(receivedCarTitle, Is.EqualTo(carTitle), $"Car title doesn't match");
            }
            else
            {
                Assert.Fail($"Car brand {carbrand} doesn't exist");
            }

            // alternative method
            /*switch (carbrand)
            {
                case "bmw":
                    await newCarsPage.GoToBmw();
                    break;
                case "toyota":
                    await newCarsPage.GoToToyota();
                    break;
                case "mg":
                    await newCarsPage.GoToMg();
                    break;
            }*/

            await Task.Delay(3000);
        }
        catch (Exception ex)
        {
            // capture screenshot
            await CaptureScreenshot(page);
        }
        finally
        {
            await page.CloseAsync();
            await browser.CloseAsync();
        }
    }

    [Test]
    public async Task FindNewCar_ShouldNavigateToNewCarsPage()
    {
        // Arrange
        await Page.GotoAsync("https://www.carwale.com/");

        // Act - Using the PageFactory
        var homePage = new HomePage(Page);
        var newCarsPage = await homePage.FindNewCars();
        await newCarsPage.GoToToyota();

        // Assert
        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*toyota-cars.*"));
    }

    [Test]
    public async Task SearchCar_ShouldNavigateToSearchResultsPage()
    {
        // Arrange
        await Page.GotoAsync("https://www.carwale.com/");

        // Act - Using the PageFactory
        var homePage = new HomePage(Page);
        await homePage.SearchCars();

        // Assert
        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*search.*"));
    }

    public static IEnumerable<TestCaseData> GetTestData()
    {
        var columns = new List<string> { "carbrand", "browserType", "runmode", "carTitle" };

        return DataUtil.GetTestDataFromExcel(
            $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}/Resources/testdata.xlsx",
            "FindCarTest", columns);
    }
}