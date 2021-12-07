using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBot.Abstractions.Handlers
{
    public abstract class CommandHandler: IUpdateHandler
    {
        public abstract string Command { get; }
        public bool CanHandle(Update update)
        {
            return update?.Message?.Text?.ToLower()?.StartsWith(Command.ToLower()) ?? false;
        }

        public async Task<string> HandleAsync(Update update) 
        {
            if (!CanHandle(update))
            {
                return null;
            }

            return await Execute(update);
        }

        protected abstract Task<string> Execute(Update update);
    }
}
