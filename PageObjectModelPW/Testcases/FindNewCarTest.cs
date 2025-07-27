using Microsoft.Playwright;
using PageObjectModelPW.Core;
using PageObjectModelPW.Pages;

namespace PageObjectModelPW.Testcases;

[TestFixture]
public class FindNewCarTest : BaseTest
{
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
        await Page!.GotoAsync("https://www.carwale.com/");

        // Act - Using the PageFactory
        var homePage = new HomePage(Page);
        await homePage.SearchCars();

        // Assert
        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*search.*"));
    }
}