using TelegramBotConsole_Interface.Handlers;
using TelegramBotConsole_Interface.Services;
using Otus.ToDoList.ConsoleBot;

namespace TelegramBotConsole_Interface
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите максимальное количество задач (1-100): ");
            int maxTasks = int.Parse(Console.ReadLine());

            Console.Write("Введите максимальную длину задачи (1-100): ");
            int maxTaskLength = int.Parse(Console.ReadLine());

            var userService = new UserService();
            var todoService = new ToDoService(maxTasks, maxTaskLength);
            var botClient = new ConsoleBotClient();
            var updateHandler = new UpdateHandler(userService, todoService);

            botClient.StartReceiving(updateHandler);

            Console.WriteLine("Бот запущен. Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}