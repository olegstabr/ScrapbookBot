using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace ScrapbookBot
{
    internal class Bot
    {
        private TelegramBotClient _bot;
        private string _token;

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
        
        private async void OnBotMessageReceived(object sender, MessageEventArgs e) 
        {
            var message = e.Message;

            if (message.Type == MessageType.TextMessage)
            {
                if (message.Text == "/start")
                {
                    await _bot.SendTextMessageAsync(message.Chat.Id, "Приветики из Linux и ASP. NET Core 2.0");

                    var keyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(
                        new Telegram.Bot.Types.InlineKeyboardButtons.InlineKeyboardButton[] {
                            new Telegram.Bot.Types.InlineKeyboardButtons.InlineKeyboardCallbackButton("Привет", "HelloCallback")
                        }
                    );

                    await _bot.SendTextMessageAsync(message.Chat.Id, "Выберите команду", replyMarkup: keyboard);
                }
            }
        }

        private async void OnBotCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            var message = e.CallbackQuery.Message;

            if (e.CallbackQuery.Data == "HelloCallback")
            {
                await _bot.SendTextMessageAsync(message.Chat.Id, "Привет. Ты пидор! :)");
            }
        }
    }
}