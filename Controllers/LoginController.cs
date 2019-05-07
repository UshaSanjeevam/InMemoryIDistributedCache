using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InMemoryIDistributedCache.Models;
using InMemoryIDistributedCache.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;


namespace InMemoryIDistributedCache.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
     public IUserService _userService;
        private readonly AppSettings _appSettings;
        private readonly IDistributedCache _cache;
        public LoginController(IOptions<AppSettings> appSettings, IDistributedCache cache, IUserService userService)
        { 
   _userService = userService;
            _appSettings = appSettings.Value;
            _cache = cache;
        }
        [HttpGet]
        public async Task<string> getIDistributedRedis(string world)
        {
            var valueFromRedis = _cache.GetString("helloFromRedis");
            await _cache.SetStringAsync("helloFromRedis", world);
            await _cache.RefreshAsync("helloFromRedis");
            return valueFromRedis;

        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody]credentails obj)
        {
            User userObject = new User();
            DataTable user = _userService.Authenticate(obj.UserName, obj.Password);
            if (user.Rows.Count > 0)
            {
                userObject.FirstName = user.Rows[0]["FirstName"].ToString();
                userObject.LastName = user.Rows[0]["LastName"].ToString();
                userObject.Id = Convert.ToInt32(user.Rows[0]["Id"]);
                userObject.UserName = user.Rows[0]["UserName"].ToString();
                userObject.Role = user.Rows[0]["Role"].ToString();

            }
            if (user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,"")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = userObject.Id,
                UserName = userObject.UserName,
                FirstName = userObject.FirstName,
                LastName = userObject.LastName,
                Role = userObject.Role,
                Token = tokenString
            });
        }
        //working
        [HttpGet("GetCacheData")]
  
        public async Task<string> GetCacheData(int userid)
        {
            string UserID ="UserID"+Convert.ToString(userid);
            string valueFromRedis = _cache.GetString(UserID);
            if (!string.IsNullOrEmpty(valueFromRedis))
            {
              return  "Fetched from cache : " + valueFromRedis;  
            }
            else
            {
                string output = JsonConvert.SerializeObject(_userService.GetCacheData(userid));
                await _cache.SetStringAsync(UserID, output);
                 valueFromRedis = _cache.GetString(UserID);
            }
            return valueFromRedis;
        }
        [HttpGet("LogOut")]
        public string Logout(int userid)
        {
            string UserID = "UserID" + Convert.ToString(userid);
            _cache.RemoveAsync(UserID);
            return "Logout";
        }



    }
}