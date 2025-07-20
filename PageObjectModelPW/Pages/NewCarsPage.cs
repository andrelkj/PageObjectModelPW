using Microsoft.Playwright;

namespace PageObjectModelPW.Pages;

public class NewCarsPage(IPage page)
{
    // Locators
    private ILocator AllBrandList => page.GetByRole(AriaRole.List);

    private ILocator ToyotaLink =>
        AllBrandList.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = "Toyota" });

    private ILocator BmwLink => AllBrandList.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = "BMW" });
    private ILocator HondaLink => AllBrandList.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = "Honda" });
    private ILocator MgLink => AllBrandList.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = "MG" });

    // Methods
    public async Task GoToToyota()
    {
        await ToyotaLink.ClickAsync();
    }

    public async Task GoToBmw()
    {
        await BmwLink.ClickAsync();
    }

    public async Task GoToHonda()
    {
        await HondaLink.ClickAsync();
    }

    public async Task GoToMg()
    {
        await MgLink.ClickAsync();
    }
}