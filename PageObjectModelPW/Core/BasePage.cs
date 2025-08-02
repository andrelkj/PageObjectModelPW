using Microsoft.Playwright;
using PageObjectModelPW.Utilities;

namespace PageObjectModelPW.Core;

public abstract class BasePage(IPage page)
{
    protected readonly IPage Page = page;
    
    public async Task<string> GetCarTitle()
    {
        var title = await GetText("CarBase", "cartitle");
        Console.WriteLine(title);
        return title;
        // return await Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Level = 1 }).InnerTextAsync();
    }
    
    public async Task Click(string pageName, string locatorName)
    {
        BaseTest.ExtentTest.Info($"Clicking on an Element: {locatorName}");
        await Page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).ClickAsync();
    }

    public async Task MouseOver(string pageName, string locatorName)
    {
        BaseTest.ExtentTest.Info($"Moving to an Element: {locatorName}");
        await Page.HoverAsync(XMLLocatorReader.GetLocatorValue(pageName, locatorName));
    }

    public async Task<string> GetText(string pageName, string locatorName)
    {
        BaseTest.ExtentTest.Info($"Getting text of an Element: {locatorName}");
        return await Page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).InnerTextAsync();
    }

    public async Task Type(string pageName, string locatorName, string value)
    {
        BaseTest.ExtentTest.Info($"Typing in an Element: {locatorName} entered the value as: {value}");
        await Page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).FillAsync(value);
    }
}