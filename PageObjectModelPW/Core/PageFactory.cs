using Microsoft.Playwright;
using PageObjectModelPW.Pages;

namespace PageObjectModelPW.Core;

public class PageFactory(IPage page)
{
    // Caching mechanism
    private readonly Dictionary<Type, object> _pages = new();

    // Generic page creation
    private T GetPage<T>() where T : class
    {
        var pageType = typeof(T);

        if (!_pages.TryGetValue(pageType, out var page))
        {
            page = CreatePage<T>();
            _pages[pageType] = page;
        }

        return (T)page;
    }

    private T CreatePage<T>() where T : class
    {
        // This assumes all page objects have a constructor that takes IPage as a parameter
        return Activator.CreateInstance(typeof(T), page) as T 
            ?? throw new InvalidOperationException($"Failed to create instance of {typeof(T).Name}");
    }

    // Convenience properties for common pages
    public HomePage HomePage => GetPage<HomePage>();

    // Add more pages as needed:
    // public SearchResultsPage SearchResultsPage => GetPage<SearchResultsPage>();
    // public CarDetailsPage CarDetailsPage => GetPage<CarDetailsPage>();
}
