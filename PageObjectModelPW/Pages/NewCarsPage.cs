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
        // await ToyotaLink.ClickAsync();
        await Click("NewCarsPage", "toyotacar");
        return new ToyotaCarsPage(Page);
    }

    public async Task<BmwCarsPage> GoToBmw()
    {
        // await BmwLink.ClickAsync();
        await Click("NewCarsPage", "bmwcar");
        return new BmwCarsPage(Page);
    }

    public async Task<HondaCarsPage> GoToHonda()
    {
        // await HondaLink.ClickAsync();
        await Click("NewCarsPage", "hondacar");
        return new HondaCarsPage(Page);
    }

    public async Task<MgCarsPage> GoToMg()
    {
        // await MgLink.ClickAsync();
        await Click("NewCarsPage", "mgcar");
        return new MgCarsPage(Page);
    }
}