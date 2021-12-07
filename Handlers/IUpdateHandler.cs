using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBot.Abstractions.Handlers
{
    public interface IUpdateHandler
    {
        bool CanHandle(Update update);
        Task<string> HandleAsync(Update update);
    }
}
