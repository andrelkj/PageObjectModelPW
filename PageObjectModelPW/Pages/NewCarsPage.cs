using Microsoft.Playwright;
using PageObjectModelPW.Core;

namespace PageObjectModelPW.Pages;

public class NewCarsPage(IPage page) : BasePage(page)
{
    // Locators
    private ILocator AllBrandList => Page.GetByRole(AriaRole.List);

    private ILocator ToyotaLink =>
        AllBrandList.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = "Toyota" });

    private ILocator BmwLink => AllBrandList.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = "BMW" });
    private ILocator HondaLink => AllBrandList.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = "Honda" });
    private ILocator MgLink => AllBrandList.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = "MG" });

    // Methods
    public async Task<ToyotaCarsPage> GoToToyota()
    {
        await ToyotaLink.ClickAsync();
        return new ToyotaCarsPage(Page);
    }

    public async Task<BmwCarsPage> GoToBmw()
    {
        await BmwLink.ClickAsync();
        return new BmwCarsPage(Page);
    }

    public async Task<HondaCarsPage> GoToHonda()
    {
        await HondaLink.ClickAsync();
        return new HondaCarsPage(Page);
    }

    public async Task<MgCarsPage> GoToMg()
    {
        await MgLink.ClickAsync();
        return new MgCarsPage(Page);
    }
}