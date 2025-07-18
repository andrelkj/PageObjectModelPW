using Microsoft.Playwright;

namespace PageObjectModelPW.Pages;

public class HomePage(IPage page)
{
    // Locators
    private ILocator Banner => page.GetByRole(AriaRole.Banner);
    private ILocator NewCarsLink => Banner.GetByText("NEW CARS");
    private ILocator FindNewCarsLink => Banner.GetByText("Find New Cars");
    private ILocator SearchBar => page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions { Name = "Input field" });


    // Methods
    public async Task FindNewCars()
    {
        await NewCarsLink.HoverAsync();
        await FindNewCarsLink.ClickAsync();
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
