using System;
using System.Threading.Tasks;
using ScrapbookBot.TelegramBot;

namespace ScrapbookBot
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var bot = new Bot();
            await bot.GetTokenFromFileAsync("/home/linuxoid/Desktop/token scrapbook");
            await bot.Start();
            Console.ReadLine();
            bot.Stop();
        }
    }
}
