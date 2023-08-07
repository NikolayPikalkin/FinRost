using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace FinRost.BL.Infrastructure
{
    public class TelegramBotService
    {
        public enum CallBackCommad
        {
            FeedBack
        }

        public readonly long ChatAdminId = /*------PRIVATE--------*/;
        private readonly TelegramBotClient _botClient;
        private IConfiguration _config;
        private static List<long> Chats = new List<long>();

        public TelegramBotService(IConfiguration config)
        {
            _config = config;
            _botClient = new TelegramBotClient(_config["BotToken"]);
        }

        public async Task Start()
        {
            var hook = $"{_config["Url"]}api/bot/update";
            await _botClient.SetWebhookAsync(hook);
        }

        public async Task<TelegramBotClient> GetBot()
        {
            return _botClient;
        }

        public async Task AddNewUserAsync(long Id)
        {
            if(!Chats.Contains(Id))
                Chats.Add(Id);
        }
        
        public async Task<List<long>> GetChats()
        {
            return Chats;
        }

    }
}
