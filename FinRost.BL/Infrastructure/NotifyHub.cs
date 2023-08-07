using FinRost.DAL;
using Microsoft.AspNetCore.SignalR;

namespace FinRost.BL.Infrastructure
{

    public class NotifyHub : Hub
    {
        private readonly ApplicationDbContext _db;

        public NotifyHub(ApplicationDbContext db)
        {
            _db = db;
        }

        static List<DAL.Entities.Web.User> ConnectedUsers = new List<DAL.Entities.Web.User>();

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            var userC = Context.User;
            var userIdStr = httpContext.Request.Query["userId"].ToString();

            var userId = Convert.ToInt32(userIdStr);
            var conId = Context.ConnectionId;
            if (userId == 0)
                return;

            var user = await _db.Users.FindAsync(userId);
            if (user is null)
                return;
            var conUser = ConnectedUsers.FirstOrDefault(u => u.Id == user.Id);

            if (conUser != null)
            {
                if (!conUser.Connections.Contains(conId))
                    conUser.Connections.Add(conId);
            }
            else
            {
                user.Connections = new List<string>() { conId };
                ConnectedUsers.Add(user);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = ConnectedUsers.FirstOrDefault(it => it.Connections.Contains(Context.ConnectionId));

            if (user != null)
            {
                user.Connections.Remove(Context.ConnectionId);
                if (user.Connections.Count == 0)
                    ConnectedUsers.Remove(user);
            }

            await base.OnDisconnectedAsync(exception);
            return;
        }

        public async Task SendNotifyAsync(int userId, string message = "")
        {
            var userConnections = ConnectedUsers.FirstOrDefault(it => it.Id == userId)?.Connections;

            if (userConnections != null)
            {
                foreach (var con in userConnections)
                {
                    await Clients.Client(con).SendAsync("Notify", userId);
                }
            }
        }
    }
}
