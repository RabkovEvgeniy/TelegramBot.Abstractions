using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Extension;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Abstractions.Handlers
{
    public abstract class OutputCommandHandler : CommandHandler
    {
        private readonly ITelegramBotClient _botClient;

        protected abstract string ReplyMessageText { get; }
        protected abstract IReplyMarkup ReplyMarkup { get; }

        protected OutputCommandHandler(ITelegramBotClient botClient)
        {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
        }

        protected async override Task<string> Execute(Update update)
        {
            long userId = update.GetUserId();
            await _botClient.SendTextMessageAsync(userId, ReplyMessageText, replyMarkup: ReplyMarkup);
            return null;
        }
    }
}
