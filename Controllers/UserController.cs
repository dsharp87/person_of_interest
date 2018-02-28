using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using person_of_interest.Models;

namespace person_of_interest.Controllers {
    [Route ("[controller]")]
    public class UserController : Controller {
        private ProjectContext _context;
        public UserController (ProjectContext context) {
            _context = context;
        }

        [HttpGet ("[action]")]
        public User CheckSession () {
            User SlimUser = HttpContext.Session.GetObjectFromJson<User> ("SlimUser");
            return SlimUser;
        }

    }
}