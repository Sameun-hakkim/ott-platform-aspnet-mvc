using Microsoft.AspNetCore.SignalR;
using OTTPlatform.DAL;
using OTTPlatform.Models;
using System.Threading.Tasks;

namespace OTTPlatform.Hubs
{
    public class ChatHub : Hub
    {
        private readonly Video_DAL _videoDAL;

        public ChatHub(Video_DAL videoDAL)
        {
            _videoDAL = videoDAL;
        }

        public async Task SendMessage(int userId, int categoryId, string message)
        {
            var chatMessage = new VideoMessage
            {
                UserId = userId,
                CategoryId = categoryId,
                UserMessage = message
            };

            bool isInserted = _videoDAL.InsertChat(chatMessage);

            if (isInserted)
            {

                var user = _videoDAL.GetLoginDetails(userId);
                string username = user?.UserName ?? "Unknown";

                await Clients.All.SendAsync("ReceiveMessage", userId, username, message);
            }
            else
            {
                throw new HubException("Message insertion failed.");
            }
        }
    }
}
