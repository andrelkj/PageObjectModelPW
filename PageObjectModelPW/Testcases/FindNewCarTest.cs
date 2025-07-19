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

        // Act - Using the PageFactory instead of direct instantiation
        await Pages!.HomePage.FindNewCars();

        // Assert
        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*new-cars.*"));
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