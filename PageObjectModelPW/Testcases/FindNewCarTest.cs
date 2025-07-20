using Microsoft.Playwright;
using PageObjectModelPW.Core;

namespace PageObjectModelPW.Testcases;

[TestFixture]
public class FindNewCarTest : BaseTest
{
    [Test]
    public async Task FindNewCar_ShouldNavigateToNewCarsPage()
    {
        // Arrange
        await Page!.GotoAsync("https://www.carwale.com/");

        // Act - Using the PageFactory
        await Pages!.HomePage.FindNewCars();
        await Pages!.NewCarsPage.GoToToyota();

        // Assert
        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*toyota-cars.*"));
    }

    [Test]
    public async Task SearchCar_ShouldNavigateToSearchResultsPage()
    {
        // Arrange
        await Page!.GotoAsync("https://www.carwale.com/");

        // Act - Using the PageFactory
        await Pages!.HomePage.SearchCars();

        // Assert
        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*search.*"));
    }
}