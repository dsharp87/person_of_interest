using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using person_of_interest.Models;
using SessionExtensions;

namespace person_of_interest.Controllers {
    [Route ("[controller]")]
    public class ChatController : Controller {
        private ProjectContext _context;
        public ChatController (ProjectContext context) {
            _context = context;
        }

        [HttpGet ("[action]")]
        public List<User> OnlineUsers() {
            List<User> OnlineUsers = _context.users.Where(u => u.ConnectionID != "").ToList();
            return OnlineUsers;
        }
    }
}