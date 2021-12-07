using System;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.Extension
{
    public static class UpdateExtension
    {
        public static bool TryGetUserId(this Update update, out long userId) 
        {
            userId = 0;
            try
            {
                userId = update.GetUserId();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static long GetUserId(this Update update) => update.Type switch
        {
            UpdateType.Message => update.Message.From.Id,
            UpdateType.InlineQuery => update.InlineQuery.From.Id,
            UpdateType.ChosenInlineResult => update.ChosenInlineResult.From.Id,
            UpdateType.CallbackQuery => update.CallbackQuery.From.Id,
            UpdateType.EditedMessage => update.EditedMessage.From.Id,
            UpdateType.ChannelPost => update.ChannelPost.From.Id,
            UpdateType.EditedChannelPost => update.EditedChannelPost.From.Id,
            UpdateType.ShippingQuery => update.ShippingQuery.From.Id,
            UpdateType.PreCheckoutQuery => update.PreCheckoutQuery.From.Id,
            UpdateType.PollAnswer => update.PollAnswer.User.Id,
            UpdateType.MyChatMember => update.MyChatMember.From.Id,
            UpdateType.ChatMember => update.ChatMember.From.Id,
            _ => throw new ArgumentException(),
        };
    }
}
