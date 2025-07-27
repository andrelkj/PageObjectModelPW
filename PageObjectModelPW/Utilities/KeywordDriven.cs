using Microsoft.Playwright;
using PageObjectModelPW.Core;

namespace PageObjectModelPW.Utilities
{
    internal class KeywordDriven(IPage page)
    {
        public async Task Click(string pageName, string locatorName)
        {
            BaseTest.ExtentTest.Info("Clicking on an Element : " + locatorName);
            await page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).ClickAsync();
        }


        public async Task MouseOver(string pageName, string locatorName)
        {
            BaseTest.ExtentTest.Info("Moving to an Element : " + locatorName);
            await page.HoverAsync(XMLLocatorReader.GetLocatorValue(pageName, locatorName));
        }


        public async Task<string> GetText(string pageName, string locatorName)
        {
            BaseTest.ExtentTest.Info("Getting text of an Element : " + locatorName);
            return await page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).InnerTextAsync();
        }

        public async Task Type(IPage page, string pageName, string locatorName, string value)
        {
            BaseTest.ExtentTest.Info("Typing in an Element : " + locatorName + " entered the value as : " + value);
            await page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).FillAsync(value);
        }
    }
}