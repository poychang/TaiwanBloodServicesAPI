using Microsoft.Playwright;

namespace TaiwanBloodServicesAPI.Extensions
{
    internal static class ILocatorExtension
    {
        public static async Task ForEach(this ILocator locator, Func<int, Task> action)
        {
            for (int i = 0; i < await locator.CountAsync(); i++)
            {
                await action(i);
            }
        }

        public static async Task ForEach(this ILocator locator, Func<int, ILocator, Task> action)
        {
            for (int i = 0; i < await locator.CountAsync(); i++)
            {
                await action(i, locator);
            }
        }
    }
}
