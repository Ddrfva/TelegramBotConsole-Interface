using TelegramBotConsole_Interface.Models;
using TelegramBotConsole_Interface.Exceptions;

namespace TelegramBotConsole_Interface.Services
{
    public interface IUserService
    {
        ToDoUser RegisterUser(long telegramUserId, string telegramUserName);
        ToDoUser? GetUser(long telegramUserId);
    }

    public class UserService : IUserService
    {
        private readonly Dictionary<long, ToDoUser> _users = new();

        public ToDoUser RegisterUser(long telegramUserId, string telegramUserName)
        {
            if (_users.ContainsKey(telegramUserId))
                return _users[telegramUserId];

            var user = new ToDoUser(telegramUserId, telegramUserName);
            _users[telegramUserId] = user;
            return user;
        }

        public ToDoUser? GetUser(long telegramUserId)
        {
            _users.TryGetValue(telegramUserId, out var user);
            return user;
        }
    }
}