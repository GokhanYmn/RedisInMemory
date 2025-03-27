using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            //_distributedCache.SetString("name", "Gökhan", cacheEntryOptions);
            Product product = new Product { Id = 1, Name = "Kalem", Price = 100 };

            string jsonProduct = JsonConvert.SerializeObject(product);

            //json formatında binary şeklinde kayıt almak için
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            _distributedCache.Set("product:1", byteProduct, cacheEntryOptions);

            //json formatında kayıt almak için
            //await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);




            return View();
        }

        public IActionResult Show()
        {
            Byte[] byteProduct = _distributedCache.Get("product:1");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);


           //string jsonProduct = _distributedCache.GetString("product:1");
           // string name = _distributedCache.GetString("name");
           Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);


            ViewBag.Product = product;
            // ViewBag.Name = name;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");

            return View();
        }
        public IActionResult ImageUrl()
        {
            byte[] resimByte = _distributedCache.Get("resim");
            return File(resimByte, "image/jpeg");
        }


        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/deep.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("resim", imageByte);

            return View();
        }
    }
}
