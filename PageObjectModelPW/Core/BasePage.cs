using Microsoft.Playwright;

namespace PageObjectModelPW.Core;

public abstract class BasePage(IPage page)
{
    protected readonly IPage Page = page;
}