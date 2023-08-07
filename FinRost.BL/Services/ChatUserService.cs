using FinRost.BL.Extensions;
using FinRost.DAL;
using FinRost.DAL.Entities.Web;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Sockets;
using Telegram.Bot.Types;

namespace FinRost.BL.Services
{
    public class ChatUserService
    {
        private readonly ApplicationDbContext _db;

        public ChatUserService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ChatUser?> FindChatUser(long chatId)
        {
            return await _db.ChatUsers.FirstOrDefaultAsync(it => it.ChatId == chatId);
        }

        public async Task AddChatUserAsync(Chat chat, string phone)
        {
            var newChat = new ChatUser
            {
                ChatId = chat.Id,
                FirstName = chat.FirstName,
                LastName = chat.LastName,
                UserName = chat.Username,
                PhoneNumber = phone
            };

            await _db.ChatUsers.AddAsync(newChat);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ChatUser>> GetChatUsersAsync(string name)
        {
            var chatUsers = await _db.ChatUsers.Where(it => (it.UserName.Contains(name) ||
                                                            it.FirstName.Contains(name) ||
                                                            it.LastName.Contains(name) ||
                                                            it.ChatId.ToString().Contains(name) ||
                                                            it.PhoneNumber.Contains(name)) && 
                                                            it.PhoneNumber != null && 
                                                            it.PhoneNumber != ""
                                                     ).Take(30).ToListAsync();

            return chatUsers;
        }

    }
}
