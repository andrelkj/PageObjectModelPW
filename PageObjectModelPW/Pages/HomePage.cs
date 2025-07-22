using Microsoft.Playwright;
using PageObjectModelPW.Core;

namespace PageObjectModelPW.Pages;

public class HomePage(IPage page) : BasePage(page)
{
    // Locators
    private ILocator Banner => Page.GetByRole(AriaRole.Banner);
    private ILocator NewCarsLink => Banner.GetByText("NEW CARS");
    private ILocator FindNewCarsLink => Banner.GetByText("Find New Cars");
    private ILocator SearchBar => Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions { Name = "Input field" });


    // Methods
    public async Task<NewCarsPage> FindNewCars()
    {
        await NewCarsLink.HoverAsync();
        await FindNewCarsLink.ClickAsync();

        return new NewCarsPage(page);
    }

    public async Task SearchCars()
    {
        await SearchBar.FillAsync("Ford");
        await SearchBar.PressAsync("Enter");
    }

    public async Task GoToPopularCars()
    {
    }

    public async Task GoToUpcomingCars()
    {
    }
}