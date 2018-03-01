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
        public List<SlimUser> OnlineUsers() {
            List<User> OnlineUsersDB = _context.users.Where(u => u.ConnectionID != "").ToList();
            List<SlimUser> OnlineUsers = new List<SlimUser>();
            foreach(var user in OnlineUsersDB)
            {
                SlimUser NewSlimUser = new SlimUser {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ConnectionID = user.ConnectionID,
                    UserID = user.UserID,
                };
                OnlineUsers.Add(NewSlimUser);
            };
            return OnlineUsers;
        }
    }
}