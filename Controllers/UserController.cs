using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using person_of_interest.Models;
using SessionExtensions;

namespace person_of_interest.Controllers {
    [Route ("[controller]")]
    public class UserController : Controller {
        private ProjectContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController (ProjectContext context, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet ("[action]")]
        public SlimUser CheckSession () {
            SlimUser currentUser = HttpContext.Session.GetObjectFromJson<SlimUser> ("currentUser");
            System.Console.WriteLine (currentUser);
            return currentUser;
        }
        
        [HttpGet ("[action]")]
        public SlimUser CheckSessionCookieMaker () {
            SlimUser currentUser = HttpContext.Session.GetObjectFromJson<SlimUser> ("currentUser");
            CookieSet ("UserID", currentUser.UserID.ToString (), 1);
            System.Console.WriteLine (currentUser);
            return currentUser;
        }

        public string CookieGetValue (string key) {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }

        public void CookieSet (string key, string value, int? expireTime) {
            CookieOptions option = new CookieOptions ();
            if (expireTime.HasValue) { option.Expires = DateTime.Now.AddMinutes (expireTime.Value); } else { option.Expires = DateTime.Now.AddMilliseconds (10); }
            Response.Cookies.Append (key, value, option);
        }

        public void CookieRemove (string key) {
            Response.Cookies.Delete (key);
        }
    }
}