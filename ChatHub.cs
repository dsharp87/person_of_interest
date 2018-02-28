
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
 
namespace AspNetCoreSignalr.SignalRHubs
{
    public class ChatHub : Hub
    {
        public Task Send(string data)
        {
            return Clients.All.InvokeAsync("Send", data);
        }

        

        public override Task OnConnectedAsync(){
            //CHECK SESSION FOR ID
            var ConnectionID = Context.ConnectionId;
            //CHANGE CONNECTED FLAG TO TRUE
            //ADD CONNECTION ID
            //SAVE USER CHANGES
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception){
            //CHECK SESSION FOR ID
            //CHANGE CONNECTED FLAG TO FALSE
            //SAVE USER CHANGES
            return base.OnDisconnectedAsync(exception);
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