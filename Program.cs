using System;
using System.Threading.Tasks;

namespace ScrapbookBot
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var bot = new Bot();
            await bot.GetTokenFromFileAsync("/home/linuxoid/Desktop/token");
            await bot.Start();
            Console.ReadLine();
            bot.Stop();
        }
    }
}
