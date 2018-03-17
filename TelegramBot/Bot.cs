using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ScrapbookBot.Http;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ScrapbookBot.TelegramBot
{
    internal class Bot
    {
        private TelegramBotClient _bot;
        private string _token;
        private HttpClient _httpClient = new HttpClient();
        private LastBotRequest _lastBotRequest = LastBotRequest.None;
        private readonly Order.Order _order = new Order.Order();

        public Bot() {}

        public Bot(string token)
        {
            _token = token;
            
            InitBot();
        }
        
        public async Task Start() 
        {
            InitBot();
            
            Console.WriteLine("Bot is starting...");

            await _bot.SetWebhookAsync();
            _bot.StartReceiving();

            Console.WriteLine("Bot has started");
        }

        public void Stop()
        {
            _bot.StopReceiving();
            Console.WriteLine("Bot has stopped");
        }

        public async Task GetTokenFromFileAsync(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    _token = await reader.ReadLineAsync();
                }
            }
        }

        private void InitBot()
        {
            if (_bot != null)
            {
                return;
            }

            try
            {
                _bot = new TelegramBotClient(_token);
                _bot.OnMessage += OnBotMessageReceived;
                _bot.OnCallbackQuery += OnBotCallbackQuery;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task OrderBuilder(Message message)
        {
            var messageText = message.Text;
            
            switch (_lastBotRequest)
            {
                case LastBotRequest.OrderCustomerName:
                    _lastBotRequest = LastBotRequest.OrderCustomerPhone;
                    _order.CustomerName = messageText;
                    _order.CreatedDate = DateTime.Now.ToShortDateString();
                    await _bot.SendTextMessageAsync(message.Chat.Id, "Введите телефон заказчика");
                    break;
                case LastBotRequest.OrderCustomerPhone:
                    _lastBotRequest = LastBotRequest.OrderDeadlineDate;
                    await _bot.SendTextMessageAsync(message.Chat.Id, "Введите дату окончания (дд/мм/гг)");
                    _order.CustomerPhone = messageText;
                    break;
                case LastBotRequest.OrderCreatedDate:
                    _order.CreatedDate = messageText;
                    break;
                case LastBotRequest.OrderDeadlineDate:
                    _lastBotRequest = LastBotRequest.OrderStatus;
                    await _bot.SendTextMessageAsync(message.Chat.Id, "Введите статус заказа");
                    _order.DeadlineDate = messageText;
                    break;
                case LastBotRequest.OrderStatus:
                    _order.Status = message.Text;
                    break;
                case LastBotRequest.OrderOrderForms:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_order.IsFullInfo)
            {
                await _httpClient.PostOrderAsync(_order);
            }
        }

        private string MakeOrdersMessage(List<Order.Order> orders)
        {
            var builder = new StringBuilder();

            if (orders == null)
            {
                return "На данный момент нет активных заказов";
            }

            foreach (var order in orders)
            {
                builder.AppendLine(order.ToString());
            }
            
            return builder.ToString();
        }
        
        private async void OnBotMessageReceived(object sender, MessageEventArgs e) 
        {
            var message = e.Message;

            if (message.Type != MessageType.TextMessage)
            {
                return;
            }

            if (message.Text == "/start")
            {
                var keyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(
                    new Telegram.Bot.Types.InlineKeyboardButtons.InlineKeyboardButton[]
                    {
                        new Telegram.Bot.Types.InlineKeyboardButtons.InlineKeyboardCallbackButton(
                            "Добавить заказ", "AddOrderCallback"),
                        new Telegram.Bot.Types.InlineKeyboardButtons.InlineKeyboardCallbackButton(
                            "Список заказов", "GetOrdersCallback")
                    }
                );

                await _bot.SendTextMessageAsync(message.Chat.Id, "Выберите необходимое Вам действие",
                    replyMarkup: keyboard);
                return;
            }

            await OrderBuilder(message);
        }

        private async void OnBotCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            var message = e.CallbackQuery.Message;
            
            switch (e.CallbackQuery.Data)
            {
                case "AddOrderCallback":
                    _lastBotRequest = LastBotRequest.OrderCustomerName;
                    await _bot.SendTextMessageAsync(message.Chat.Id, "Введите имя заказчика");
                    break;
                case "GetOrdersCallback":
                    var orders = await _httpClient.GetOrderAsync();
                    var ordersMessage = MakeOrdersMessage(orders);
                    await _bot.SendTextMessageAsync(message.Chat.Id, ordersMessage);
                    break;
            }
        }
    }
}