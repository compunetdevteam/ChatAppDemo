using Microsoft.AspNet.SignalR;
using SignalRExample.Models;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRExample
{
    public class ChatHub : Hub
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private static int _hitCount = 2;
        [Authorize]
        public void Send(string name, string message)
        {

            var userCount = _db.Users.Count();
            var list = _db.Users.ToList();
            Clients.All.addNewMessageToPage(name, message, userCount, list);

        }
        public void GetUser()
        {
            var list = _db.Users.ToList();
            string userimg = "/images/DP/dummy.png";
            var loginDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            Clients.Caller.onGetUser(list, loginDate, userimg);

        }

        public void RegisterUser(string userId, string connectionId)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id.Equals(userId));
            if (user != null)
            {
                user.ConnectionId = connectionId;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
            }
            Clients.Caller.onRegisterUser(user);

        }


        public override Task OnDisconnected(bool stopCalled)
        {
            _hitCount -= 1;
            Clients.All.OnRecordHit(_hitCount);
            //Clients.Users()
            return base.OnDisconnected(stopCalled);
        }

        public void SendPrivateMessage(string toUserId, string message, string sender)
        {

            //string fromUserId = sender;

            var toUser = _db.Users.FirstOrDefault(x => x.Id == toUserId);
            var fromUser = _db.Users.FirstOrDefault(x => x.Id.Equals(sender));

            if (toUser != null && fromUser != null)
            {
                string CurrentDateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                string userimg = "/images/DP/dummy.png";
                var windowId = sender;
                var fromUserName = fromUser.UserName;
                // send to 
                Clients.Client(toUser.ConnectionId).sendPrivateMessage(windowId, fromUserName, message, userimg, CurrentDateTime);
                windowId = toUserId;
                // send to caller user
                Clients.Caller.sendPrivateMessage(windowId, fromUserName, message, userimg, CurrentDateTime);
            }

        }

        #region Old project Code
        //private List<ApplicationUser> _connectedUsers;
        //private static List<Message> _currentMessage;
        //private ApplicationDbContext _db;

        //public ChatHub()
        //{
        //    _db = new ApplicationDbContext();
        //    _connectedUsers = _db.Users.ToList();
        //    _currentMessage = _db.Messages.ToList();
        //}
        //public void Connect(string userName)
        //{
        //    var id = Context.User.Identity.GetUserId();

        //    if (_connectedUsers.Count(x => x.Id == id) == 0)
        //    {
        //        string UserImg = GetUserImage(userName);
        //        string logintime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        //        _connectedUsers.Add(new ApplicationUser { Id = id, UserName = userName, UserImage = UserImg, LoginTime = logintime });

        //        // send to caller
        //        Clients.Caller.onConnected(id, userName, _connectedUsers, _currentMessage);

        //        // send to all except caller client
        //        Clients.AllExcept(id).onNewUserConnected(id, userName, UserImg, logintime);
        //    }
        //}

        //public void SendMessageToAll(string userName, string message, string time)
        //{
        //    string UserImg = GetUserImage(userName);
        //    // store last 100 messages in cache
        //    AddMessageinCache(userName, message, time, UserImg);

        //    // Broad cast message
        //    Clients.All.messageReceived(userName, message, time, UserImg);

        //}

        //private void AddMessageinCache(string userName, string message, string time, string UserImg)
        //{
        //    _currentMessage.Add(new Message { UserName = userName, MessageText = message, Time = time, UserImage = UserImg });

        //    if (_currentMessage.Count > 100)
        //        _currentMessage.RemoveAt(0);

        //    // Refresh();
        //}

        //public string GetUserImage(string username)
        //{
        //    string RetimgName = "images/dummy.png";
        //    //try
        //    //{
        //    //    string query = "select Photo from tbl_Users where UserName='" + username + "'";
        //    //    //string ImageName = ConnC.GetColumnVal(query, "Photo");

        //    //    if (ImageName != "")
        //    //        RetimgName = "images/DP/" + ImageName;
        //    //}
        //    //catch (Exception ex)
        //    //{ }
        //    return RetimgName;
        //}


        //public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        //{
        //    var item = _connectedUsers.FirstOrDefault(x => x.Id == Context.ConnectionId);
        //    if (item != null)
        //    {
        //        _connectedUsers.Remove(item);

        //        var id = Context.ConnectionId;
        //        Clients.All.onUserDisconnected(id, item.UserName);

        //    }
        //    return base.OnDisconnected(stopCalled);
        //} 
        #endregion
    }
}