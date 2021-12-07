using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBot.Abstractions.Handlers;

namespace TelegramBot.Abstractions.States
{
    public abstract class UserState: IState
    {
        private readonly List<IUpdateHandler> _handlers;

        public IReadOnlyList<IUpdateHandler> Handlers => _handlers;
        public abstract string Name { get; }

        protected UserState()
        {
            _handlers = new List<IUpdateHandler>();
        }
        protected UserState(List<IUpdateHandler> handlers)
        {
            _handlers = handlers ?? new List<IUpdateHandler>();
        }

        public async virtual Task<string> HandleUpdateAsync(Update update)
        {
            foreach (IUpdateHandler handler in Handlers)
            {
                if (handler.CanHandle(update))
                {
                    return await handler.HandleAsync(update);
                }
            }

            return null;
        }
        
        protected void AddHandler(IUpdateHandler handler) => _handlers.Add(handler); 
        protected void AddHandlers(List<IUpdateHandler> handlers) => _handlers.AddRange(handlers); 
    }
}
