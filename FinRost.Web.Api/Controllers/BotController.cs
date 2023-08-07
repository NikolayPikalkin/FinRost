using FinRost.BL.Extensions;
using FinRost.BL.Infrastructure;
using FinRost.BL.Services;
using FinRost.DAL.Dto;
using FinRost.Web.Api.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Requests;
using FinRost.BL.Statics;

namespace FinRost.Web.Api.Controllers
{
    [Route("api/bot")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly InvestorService _investorService;
        private readonly NotificationService _notificationService;
        private readonly OrderService _orderService;
        private readonly ChatUserService _chatUserService;
        private readonly LogService _logService;
        private readonly LotService _lotService;
        private readonly TelegramBotClient _botClient;
        private readonly TelegramBotService _botService;
        private readonly IHubContext<NotifyHub> _hub;

        public BotController(
            TelegramBotService botService,
            LotService lotService,
            ChatUserService chatUserService,
            OrderService orderService,
            NotificationService notificationService,
            InvestorService investorService,
            IHubContext<NotifyHub> hub)
        {
            _botClient = botService.GetBot().Result;
            _lotService = lotService;
            _botService = botService;
            _chatUserService = chatUserService;
            _orderService = orderService;
            _notificationService = notificationService;
            _investorService = investorService;
            _hub = hub;
        }

        async Task<Chat?> GetChatId(Update upd)
        {
            switch (upd.Type)
            {
                case UpdateType.Message:
                    return upd.Message.Chat;
                case UpdateType.CallbackQuery:
                    return upd.CallbackQuery.Message.Chat;
                default:
                    return null;
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update(object update)
        {
            var upd = JsonConvert.DeserializeObject<Update>(update.ToString());
            var chat = await GetChatId(upd);
            if (chat is null)
                return Ok();
            try
            {
                var chatUser = await _chatUserService.FindChatUser(chat.Id);
                if (chatUser is null || string.IsNullOrEmpty(chatUser.PhoneNumber))
                {
                    if (upd.Message?.Contact != null)
                    {
                        await _chatUserService.AddChatUserAsync(chat, upd.Message.Contact.PhoneNumber);
                        await _botClient.SendTextMessageAsync(chat.Id,
                                                              "Спасибо, Ваш номер телефона добавлен в базу данных для связки с вашей анкетой!",
                                                              replyMarkup: new ReplyKeyboardRemove());
                        return Ok();
                    }
                    else
                    {
                        var request = new KeyboardButton[] { new KeyboardButton("Отправить номер телефона") { RequestContact = true } };
                        var replyMarkup = new ReplyKeyboardMarkup(request);
                        await _botClient.SendTextMessageAsync(chat.Id, "Вас приветствует бот ФинРост! " +
                                                                       "Для регистрации необходимо отправить номер " +
                                                                       "телефона по кнопке ниже 👇", replyMarkup: replyMarkup);
                        return Ok();
                    }
                }

                if (upd.Type == UpdateType.CallbackQuery)
                {
                    var callbackData = upd.CallbackQuery.Data;
                    var paramsCallBack = callbackData.Split(":");
                    var commad = paramsCallBack[0];

                    if (commad == BotCommad.FeedBack.ToString())
                    {
                        var lotId = Convert.ToInt32(paramsCallBack[1]);

                        var lot = await _lotService.GetLotByIdAsync(lotId);
                        if (lot is null)
                            throw new CustomException("Лот не найден");

                        var order = await _orderService.GetOrderByIdAsync(lot.OrderId);
                        if (order is null)
                            throw new CustomException();

                        var investor = await _investorService.GetInvestorByChatIdAsync(chat.Id);
                        if (investor is null)
                            throw new CustomException("Инвестор не найден!");

                        var responseMessage = await _lotService.AddFeedBackAsync(lot.Id, investor.Id, lot.OrderId);
                        await _botClient.SendTextMessageAsync(chat.Id, responseMessage);

                        var desc = $"Отлик по договору № {order.NUMBER} от {investor.FullName}!";

                        foreach (var userId in await _notificationService.GetUsersNotificationAsync(NotifyKeyWords.FeedBack))
                        {
                            await _notificationService.SendNotificationFromRobotAsync(userId, desc, "Новый отлик!");
                            await _hub.Clients.All.SendAsync("Notify", userId);

                        }
                    }
                }
                else
                {
                    if (upd.Type == UpdateType.Message)
                    {
                        await _botClient.SendTextMessageAsync(chat.Id, "Здесь будут отображаться лоты, на которые можно оставить отклик!");
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                await _botClient.SendTextMessageAsync
                    (_botService.ChatAdminId, ex.Message +
                    $"\nChatId: {chat.Id}" +
                    $"\nUser: {chat.Username}" +
                    $"\nФИО: {chat.LastName} {chat.FirstName}"
                    );

                await _botClient.SendTextMessageAsync(chat.Id, "Сервис временно не доступен! Повторите попытку позже!");
                return Ok();
            }
        }

        /// <summary>
        /// Отправка лота в ТГ
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        [HttpPost("sendLot")]
        public async Task<ActionResult> SendLot(int lotId)
        {
            var lot = await _lotService.GetLotByIdAsync(lotId);
            if (lot is null)
                throw new CustomException("Лот не найден!");

            var order = await _orderService.GetOrderByIdAsync(lot.OrderId);
            if (order is null)
                throw new CustomException("Заявка не найдена!");

            var files = await _lotService.GetLotFilesAsync(lotId);

            if (files is null || files?.Count == 0)
                return BadRequest(new ErrorResponse
                {
                    Message = "Не найдены файлы для отправки!"
                });

            // Собираем массив файлов
            var listFiles = new List<InputMediaPhoto>(); // Может добавить обертку ???
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                var fileStream = new FileStream(file, FileMode.Open);
                var inputFile = new InputMediaPhoto(new InputFileStream(fileStream, fileInfo.Name));
                listFiles.Add(inputFile);
            }

            // Загружаем на сервер Telegram
            var listPhoto = await _botClient.SendMediaGroupAsync(
                chatId: _botService.ChatAdminId,
                media: listFiles
                );

            //TODO: ПРОЦЕДУРА ИЗ БД
            var description = string.Empty;
            description += $"✅ Сумма займа : {order.LOANCOSTALL:0,0}\n" +
                "\n" +
                "🏡  г. Красноярск, ул. Батурина 38а\n" +
                "Дом 15.5 кв.м\n" +
                "Участок 403 кв.м\n" +
                "\n" +
                "🔹 Рыночная стоимость : 1 200 000\n" +
                $"🔹 Процентная ставка : {order.PERCENTCOSTALL * 12} %\n" +
                $"🔹 Срок : {order.DAYSQUANT} мес\n" +
                "\n" +
                "💸 Доход инвестора : 230 400 руб за 1 год";

            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData("Связаться по заявке", $"{BotCommad.FeedBack}:{lotId}"),
            });

            await _botClient.SendTextMessageAsync(
                chatId: _botService.ChatAdminId,
                text: description,
                replyMarkup: inlineKeyboard
                );

            //  Получаем все FileId
            var mediaFiles = new List<InputMediaPhoto>();
            foreach (var it in listPhoto)
            {
                if (it.Photo != null && it.Photo.Length > 0)
                {
                    var mediaPhoto = new InputMediaPhoto(InputFile.FromFileId(it.Photo.Last().FileId));
                    mediaFiles.Add(mediaPhoto);
                }
            }

            // Отправляем всем пользователям
            foreach (var investor in await _investorService.GetInvestorsByMinSumAsync((int)order.LOANCOSTALL))
            {
                await _botClient.SendMediaGroupAsync(
                chatId: investor.ChatId,
                media: mediaFiles
                );

                await _botClient.SendTextMessageAsync(
                chatId: investor.ChatId,
                text: description,
                replyMarkup: inlineKeyboard
                );
            }

            listFiles.ForEach(it => (it.Media as InputFileStream)?.Content.Dispose());  // Явно закрываем потоки

            await _lotService.DeleteFeedBacksAsync(lotId);
            return Ok();
        }
    }
}