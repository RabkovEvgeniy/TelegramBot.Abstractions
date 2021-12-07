using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Extension;
using TelegramBot.Abstractions.States;
using TelegramBot.Abstractions.Handlers;

namespace TelegramBot.Abstractions
{
    public abstract class Bot
    {
        private const string _stateNotFoundErrorMessage = "Не удалось найти состояние с именем ";

        private readonly IEnumerable<IState> _states;
        private readonly IEnumerable<IUpdateHandler> _globalHandlers;
        private readonly IUserStateNamesRepository _statesRepository;

        private bool IsUseStates => _states != null && _statesRepository != null;

        protected Bot(IEnumerable<IUpdateHandler> globalHandlers)
        {
            _globalHandlers = globalHandlers;
        }

        protected Bot(IUserStateNamesRepository statesRepository, IEnumerable<UserState> states,
            IEnumerable<IUpdateHandler> globalHandlers = null) : this(globalHandlers)
        {
            _statesRepository = statesRepository ?? throw new ArgumentNullException(nameof(statesRepository));
            _states = states ?? throw new ArgumentNullException(nameof(states));
        }

        public virtual async Task HandleUpdateAsync(Update update)
        {
            bool updateContainsUserId = update.TryGetUserId(out long userId);
            string newStateName = null;

            IUpdateHandler handler = _globalHandlers?.FirstOrDefault(handler => handler.CanHandle(update));

            if (handler != null)
            {
                newStateName = await handler.HandleAsync(update);
            }
            else if (IsUseStates && updateContainsUserId)
            {
                string stateName = await _statesRepository.GetUserStateName(userId);

                IState state = _states.FirstOrDefault(state => state.Name == stateName)
                    ?? throw new Exception(_stateNotFoundErrorMessage + stateName);

                newStateName = await state.HandleUpdateAsync(update);
            }

            if (newStateName == null)
            {
                return;
            }

            if (updateContainsUserId && IsUseStates)
            {
                IState newState = _states.FirstOrDefault(state => state.Name == newStateName)
                    ?? throw new Exception(_stateNotFoundErrorMessage + newStateName);

                await newState.OnTransactionAsync(userId);
                await _statesRepository.SetUserStateName(userId, newStateName);
            }
        }
    }
}
