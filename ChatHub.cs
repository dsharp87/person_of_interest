using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using person_of_interest.Models;
using SessionExtensions;

namespace AspNetCoreSignalr.SignalRHubs {
    public class ChatHub : Hub {
        private ProjectContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ChatHub (ProjectContext context, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task Send (string data) {
            return Clients.All.InvokeAsync ("Send", data);
        }

        public override Task OnConnectedAsync () {
            int CookieUID = int.Parse(CookieGetValue("UserID"));
            string ConnectionID = Context.ConnectionId;
            User UpdateUser = _context.users.SingleOrDefault (user => user.UserID == CookieUID);
            UpdateUser.ConnectionID = ConnectionID;
            _context.SaveChanges ();
            return base.OnConnectedAsync ();
        }

        public override Task OnDisconnectedAsync (System.Exception exception) {
            string ConnectionID = Context.ConnectionId;            
            User UpdateUser = _context.users.SingleOrDefault (user => user.ConnectionID == ConnectionID);
            UpdateUser.ConnectionID = "";    
            _context.SaveChanges ();
            Clients.All.InvokeAsync("Send","${UpdateUser.FirstName} has logged off (${UpdateUser.UserID})");
            return base.OnDisconnectedAsync (exception);
        }

        public string CookieGetValue (string key) {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }

        // public void SendChatMessage(string who, string message)
        //         {
        //             var name = Context.User.Identity.Name;
        //             using (var db = new UserContext())
        //             {
        //                 var user = db.Users.Find(who);
        //                 if (user == null)
        //                 {
        //                     Clients.Caller.showErrorMessage("Could not find that user.");
        //                 }
        //                 else
        //                 {
        //                     db.Entry(user)
        //                         .Collection(u => u.Connections)
        //                         .Query()
        //                         .Where(c => c.Connected == true)
        //                         .Load();

        //                     if (user.Connections == null)
        //                     {
        //                         Clients.Caller.showErrorMessage("The user is no longer connected.");
        //                     }
        //                     else
        //                     {
        //                         foreach (var connection in user.Connections)
        //                         {
        //                             Clients.Client(connection.ConnectionID)
        //                                 .addChatMessage(name + ": " + message);
        //                         }
        //                     }
        //                 }
        //             }
        //         }

    }
}