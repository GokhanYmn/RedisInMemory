﻿using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "sortedsetnames";
        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            HashSet<string> list=new HashSet<string>();
            if (db.KeyExists(listKey))
            {
                //db.SortedSetScan(listKey).ToList().ForEach(x =>
                //{
                //    list.Add(x.ToString());
                //});

                //Score a göre gösterme
                db.SortedSetRangeByRank(listKey,order:Order.Descending).ToList().ForEach(x=>
                {
                    list.Add(x).ToString();
                 
                });
            }

            return View();
        }

        [HttpPost]
        public IActionResult Add(string name,int score)
        {
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));
            db.SortedSetAdd(listKey,name,score);


            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            db.SortedSetRemoveAsync(listKey, name).Wait();
            return RedirectToAction("Index");
            
        }
    }
}
