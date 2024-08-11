using bot.Models;
using bot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace bot.Handlers
{
    public class TelegramUpdateHandler
    {
        private readonly TelegramBotClient _botClient;
        private readonly LeadApiService _leadApiService;

        public TelegramUpdateHandler(TelegramBotClient botClient, LeadApiService leadApiService) =>
            (_botClient, _leadApiService) = (botClient, leadApiService);

        public void StartReceiving()
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions
            );
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message?.Text != null)
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;

                if (messageText == "/start")
                {
                    await botClient.SendTextMessageAsync(
                        chatId,
                        "Welcome! Please provide your details in the following format:\nName\nQuestion\nContact",
                        cancellationToken: cancellationToken
                    );
                }
                else
                {
                    var parts = messageText.Split('\n');
                    if (parts.Length == 3)
                    {
                        var leadModel = new LeadCreateModel
                        {
                            ClientName = parts[0],
                            ClientQuestion = parts[1],
                            Contact = parts[2]
                        };

                        var success = await _leadApiService.CreateLeadAsync(leadModel);

                        if (success)
                        {
                            await botClient.SendTextMessageAsync(
                                chatId,
                                "Lead created successfully!",
                                cancellationToken: cancellationToken
                            );
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(
                                chatId,
                                "Invalid format. Please provide your details in following format:\nName\nQuestion\nContact",
                                cancellationToken: cancellationToken
                            );
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                            chatId,
                            "Invalid format. Please provide your details in following format:\nName\nQuestion\nContact",
                            cancellationToken: cancellationToken
                        );
                    }
                }
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.ToString());
            return Task.CompletedTask;
        }
    }
}