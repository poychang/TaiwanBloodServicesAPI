using Microsoft.Extensions.Caching.Memory;
using Microsoft.Playwright;
using TaiwanBloodServicesAPI.Extensions;

namespace TaiwanBloodServicesAPI.Services
{
    public class BloodInfoService
    {
        private const string CACHE_NAME = "BloodInfo";
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheLifeTime = TimeSpan.FromHours(1);

        public BloodInfoService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IEnumerable<BloodInfo> FetchBloodInfo() =>
            _memoryCache.TryGetValue(CACHE_NAME, out IEnumerable<BloodInfo> value)
                ? value
                : FetchRealTimeBloodInfo().GetAwaiter().GetResult();

        public async Task<IEnumerable<BloodInfo>> FetchRealTimeBloodInfo()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });

            var page = await browser.NewPageAsync();
            const string TaiwanBloodServices_URL = "https://www.blood.org.tw/Internet/main/index.aspx";
            await page.GotoAsync(TaiwanBloodServices_URL);

            // Fetch last update time
            var bloodTable1 = page.Locator("#blood-table > table").Nth(0);
            var lastUpdateTime = await bloodTable1.Locator("tr > th > span").TextContentAsync();

            // Fetch location
            var bloodTable2 = page.Locator("#blood-table > table").Nth(1);
            var location = new List<string>();
            await bloodTable2.Locator("tr").Nth(0).Locator("th:has(> a)").ForEach(async (index, locater) =>
            {
                var text = await locater.Nth(index).Locator("a").TextContentAsync();
                if (text is not null) location.Add(text);
            });

            // Fetch blood info
            const int BLOOD_TYPE_COUNT = 4;
            var bloodInfoList = new List<BloodInfo>();
            for (int bloodTypeIndex = 1; bloodTypeIndex <= BLOOD_TYPE_COUNT; bloodTypeIndex++)
            {
                var bloodType = await bloodTable2.Locator("tr").Nth(bloodTypeIndex).Locator("th").TextContentAsync();
                await bloodTable2.Locator("tr").Nth(bloodTypeIndex).Locator("td:has(> img)").ForEach(async (index, locater) =>
                {
                    bloodInfoList.Add(new BloodInfo
                    {
                        Type = await bloodTable2.Locator("tr").Nth(bloodTypeIndex).Locator("th").Nth(0).TextContentAsync() ?? string.Empty,
                        Status = await locater.Nth(index).Locator("img").GetAttributeAsync("alt") ?? string.Empty,
                        Location = location[index],
                        LastUpdateTime = lastUpdateTime?.Replace("最新更新時間：", string.Empty) ?? string.Empty
                    });
                });
            }

            _memoryCache.Set(CACHE_NAME, bloodInfoList, _cacheLifeTime);

            return bloodInfoList;
        }

        public class BloodInfo
        {
            public string Type { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public string LastUpdateTime { get; set; } = string.Empty;
        }
    }
}
