using bot.Handlers;
using bot.Services;
using Telegram.Bot;

namespace bot
{
    class Program
    {
        private static readonly string BotToken = "";
        private static readonly string ApiBaseAddress = "http://localhost:5124/Lead";

        static void Main(string[] args)
        {
            var botClient = new TelegramBotClient(BotToken);
            var leadApiService = new LeadApiService(ApiBaseAddress);
            var telegramUpdateHandler = new TelegramUpdateHandler(botClient, leadApiService);

            telegramUpdateHandler.StartReceiving();

            Console.WriteLine("Bot is up and running...");
            Console.ReadLine();
        }
    }
}
