using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using person_of_interest.Models;
using SessionExtensions;

namespace AspNetCoreSignalr.SignalRHubs {
    public class ChatHub : Hub {
        private readonly IHttpContextAccessor _accessor;
        private ProjectContext _context;
        public ChatHub (ProjectContext context, IHttpContextAccessor accessor) {
            _context = context;
            _accessor = accessor;
        }

        public Task Send (string data) {
            return Clients.All.InvokeAsync ("Send", data);
        }

        public override Task OnConnectedAsync () {
            // SlimUser currentUser = _accessor.HttpContext.Session.GetObjectFromJson<SlimUser> ("currentUser");
            int SessionUserID =(int) _accessor.HttpContext.Session.GetInt32("UserID");
            var ConnectionID = Context.ConnectionId;
            User UpdateUser = _context.users.SingleOrDefault (user => user.UserID == SessionUserID);

            UpdateUser.ConnectionID = ConnectionID;
            _context.SaveChanges ();
            return base.OnConnectedAsync ();
        }

        public override Task OnDisconnectedAsync (System.Exception exception) {
            SlimUser currentUser = _accessor.HttpContext.Session.GetObjectFromJson<SlimUser> ("currentUser");

            User UpdateUser = _context.users.SingleOrDefault (user => user.UserID == currentUser.UserID);
            UpdateUser.ConnectionID = "";    
            _context.SaveChanges ();
            return base.OnDisconnectedAsync (exception);
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