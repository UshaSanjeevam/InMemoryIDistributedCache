using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryIDistributedCache.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace InMemoryIDistributedCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InMemoryController : ControllerBase
    {
        public IUserService _memoryService;

        private readonly IMemoryCache _cache;
        public InMemoryController(IMemoryCache cache, IUserService memoryService)
        {
            _memoryService = memoryService;         
            _cache = cache;
        }
        //[HttpGet("InMemory")]
        //public string Get(int userid)
        //{
        //    string UserID = "UserID" + Convert.ToString(userid);
        //    string obj;
        //    if (!_cache.TryGetValue<string>(UserID, out obj))
        //    {
        //        MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
        //        cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
        //        cacheExpirationOptions.Priority = CacheItemPriority.Normal;
        //        string output = JsonConvert.SerializeObject(_memoryService.GetCacheData(userid));
        //         _cache.Set<string>(UserID, output, cacheExpirationOptions);
        //      //  obj = DateTime.Now.ToString();
        //      //  _cache.Set<string>(key, obj);
        //    }
        //     return  "Fetched from InMemory cache : " + obj;
        //}
        [HttpGet("InMemory")]
        public string Get(int userid)
        {
            string UserID = "UserID" + Convert.ToString(userid);
            string valueFromRedis = _cache.Get<string>(UserID);
            if (!string.IsNullOrEmpty(valueFromRedis))
            {
                return "Fetched from cache : " + valueFromRedis;
            }
            else
            {
                string output = JsonConvert.SerializeObject(_memoryService.GetCacheData(userid));
                 _cache.Set(UserID, output);
                valueFromRedis = _cache.Get<string>(UserID);
            }
            return valueFromRedis;
        }
        [HttpGet("LogOut")]
        public string Logout(int userid)
        {
            string UserID = "UserID" + Convert.ToString(userid);
            _cache.Remove(UserID);
            return "Logout";
        }
    }
}