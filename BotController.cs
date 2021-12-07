using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace TelegramBot.Abstractions
{
    public abstract class BotController : Controller
    {
        private const string _updateIsNullErrorMessage = "Обрабатываемый Update не может равняться null";

        private readonly Bot _bot;
        private readonly ILogger<BotController> _logger;

        public BotController(Bot bot, ILogger<BotController> logger)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<OkResult> GetUpdate([FromBody] Update update)
        {
            if (update == null)
            {
                throw new ArgumentException(_updateIsNullErrorMessage);
            }

            try
            {
                await _bot.HandleUpdateAsync(update);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }

            return new OkResult();
        }
    }
}
