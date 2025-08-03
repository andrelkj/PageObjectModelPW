using Microsoft.Playwright;
using PageObjectModelPW.Core;
using PageObjectModelPW.Pages;
using PageObjectModelPW.Utilities;

namespace PageObjectModelPW.Testcases
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    internal class CarNameAndPriceTest : BaseTest
    {
        [Parallelizable(ParallelScope.Self)]
        [Test, TestCaseSource(nameof(GetTestData)), Category("SMOKE")]
        public async Task CarNameAndPrice(string carbrand, string browserType, string runmode)
        {
            if (runmode.Equals("N"))
            {
                Assert.Ignore("Ignoring the test as the run mode is NO");
            }

            //Each test gets a new Playwright Instance
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
                if (carBrandActions.TryGetValue(carbrand.ToLower(), out var navigateToCar))
                {
                    await navigateToCar();
                    await newCarsPage.GetCarNameAndPrices();
                }
                else
                {
                    Assert.Fail($"Car brand '{carbrand}' does not exists");
                }

                await Task.Delay(2000);
            }
            catch (Exception ex)
            {
                //capturing screenshot
                await CaptureScreenshot(page);
            }
            finally
            {
                await page.CloseAsync();
                await browser.CloseAsync();
            }
        }

        public static IEnumerable<TestCaseData> GetTestData()
        {
            var columns = new List<string> { "carbrand", "browserType", "runmode" };

            return DataUtil.GetTestDataFromExcel(
                $"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName}/Resources/testdata.xlsx",
                "CarNameAndPrice", columns);
        }
    }
}