using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            var db = _redisService.GetDb(0);
            db.StringSet("name", "Gökhan Yaman");
            db.StringSet("visitor", 100);



            return View();
        }
        public IActionResult Show()
        {
            var value = db.StringGet("name");
            db.StringIncrement("visitor",1);
            
            var count=db.StringDecrementAsync("visitor", 1).Result;
            //Sonucun gösterilmesinin önemi yokssa aşağıdaki wait fonksiyonu kullanılabilir
          //  db.StringDecrementAsync("visitor", 1).Wait();

            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
            }
            return View();
        }

        
    }
}
