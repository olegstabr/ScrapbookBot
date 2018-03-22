using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ScrapbookBot.Http;
using ScrapbookBot.Models.Order;
using ScrapbookBot.Models.Template;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;

namespace ScrapbookBot.TelegramBot
{
    internal class Bot
    {
        private TelegramBotClient _bot;
        private string _token;
        private readonly HttpClient _httpClient = new HttpClient();
        private Order _order = new Order();
        private readonly List<string> _callbackNames = new List<string>();
        private LastBotRequest _lastBotRequest = LastBotRequest.None;
        private List<TemplateForm> _templateForms = new List<TemplateForm>();
        private TemplateForm _templateForm;
        private int _templateFieldId;

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
                    await _bot.SendTextMessageAsync(message.Chat.Id, "Новый заказ сформирован");
                    await _httpClient.PostOrderAsync(_order);
                    _templateFieldId = 0;
                    break;
                case LastBotRequest.OrderOrderForms:
                    break;
                case LastBotRequest.TemplateField:
                    var fieldValues = _order.OrderForms?[0].Fields;
                    if (fieldValues != null)
                    {
                        fieldValues[_templateFieldId].Value = message.Text;
                    }
                    
                    if (_templateFieldId++ == _templateForm.Fields?.Count - 1)
                    {
                        await _bot.SendTextMessageAsync(message.Chat.Id, "Введите имя заказчика");
                        _lastBotRequest = LastBotRequest.OrderCustomerName;
                        return;
                    }

                    await _bot.SendTextMessageAsync(message.Chat.Id, _templateForm.Fields?[_templateFieldId].Name);
                    break;
                case LastBotRequest.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private TemplateForm GetTemplateFormById(int id)
        {
            foreach (var form in _templateForms)
            {
                if (form.Id == id)
                {
                    return form;
                }
            }

            return null;
        }

        private string MakeOrdersMessage(List<Order> orders)
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
                    new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton(
                            "Добавить заказ", "AddOrderCallback"),
                        new InlineKeyboardCallbackButton(
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
            var callbackData = e.CallbackQuery.Data;
            var keyboardButtons = new List<InlineKeyboardButton>();
            
            switch (callbackData)
            {
                case "AddOrderCallback":
                    _templateForms = await _httpClient.GetTemplateFormsAsync();

                    foreach (var form in _templateForms)
                    {
                        _callbackNames.Add(form.Id.ToString());
                        keyboardButtons.Add(new InlineKeyboardCallbackButton(form.Name, form.Id.ToString()));
                    }
                    
                    var keyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(keyboardButtons.ToArray());

                    await _bot.SendTextMessageAsync(message.Chat.Id, "Что хотите заказать?",
                        replyMarkup: keyboard);
                    break;
                case "GetOrdersCallback":
                    var orders = await _httpClient.GetOrderAsync();
                    var ordersMessage = MakeOrdersMessage(orders);
                    await _bot.SendTextMessageAsync(message.Chat.Id, ordersMessage);
                    break;
            }

            var indexOfData = _callbackNames.IndexOf(callbackData);

            if (indexOfData > -1)
            {
                var templateFormId = Convert.ToInt32(_callbackNames[indexOfData]);
                _templateForm = GetTemplateFormById(templateFormId);
                if (_templateForm.Id != null)
                {
                    _order = await _httpClient.PostTemplateFormIntoOrderAsync((int) _templateForm.Id);
                }

                await _bot.SendTextMessageAsync(message.Chat.Id, _templateForm.Fields?[_templateFieldId].Name);
                
                _lastBotRequest = LastBotRequest.TemplateField;
            }
            
        }
    }
}