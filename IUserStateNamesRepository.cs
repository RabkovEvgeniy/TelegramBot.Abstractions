using System;
using System.Threading.Tasks;

namespace TelegramBot.Abstractions
{
    public interface IUserStateNamesRepository
    {
        Task<string> GetUserStateName(long userId);
        Task SetUserStateName(long userId, string stateName);
    }
}