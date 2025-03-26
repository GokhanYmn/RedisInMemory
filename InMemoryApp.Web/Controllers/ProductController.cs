using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }



        public IActionResult Index()
        {
            //1.yol
            //if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}

            //2.yol
            if (_memoryCache.TryGetValue("zaman", out string zamancache))
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(30);

                options.SlidingExpiration = TimeSpan.FromSeconds(10);
                options.Priority=CacheItemPriority.High;


                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _memoryCache.Set("callback", $"{key}-{value}->sebep :{reason}");
                });


                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

                Product p= new Product {Id=1,Name="Kalem",Price=200 };
                _memoryCache.Set<Product>("product:1", p);
            }





            return View();
        }

        public IActionResult Show()
        {
            //_memoryCache.GetOrCreate<string>("zaman", entry =>
            //{

            //    entry.SlidingExpiration = TimeSpan.FromSeconds(10);
            //    return DateTime.Now.ToString();
            //});
            _memoryCache.TryGetValue("zaman", out string zamancache);
            _memoryCache.TryGetValue("callback", out string callback);

            ViewBag.zaman = zamancache;
            ViewBag.callback = callback;
            ViewBag.product = _memoryCache.Get<Product>("product:1");
            return View();
        }

    }
}
