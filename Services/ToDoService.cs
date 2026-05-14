using TelegramBotConsole_Interface.Models;
using TelegramBotConsole_Interface.Exceptions;

namespace TelegramBotConsole_Interface.Services
{
    public interface IToDoService
    {
        IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId);
        IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId);
        ToDoItem Add(ToDoUser user, string name);
        void MarkCompleted(Guid id);
        void Delete(Guid id);
    }

    public class ToDoService : IToDoService
    {
        private readonly List<ToDoItem> _tasks = new();
        private readonly int _maxTasks;
        private readonly int _maxTaskLength;

        public ToDoService(int maxTasks, int maxTaskLength)
        {
            _maxTasks = maxTasks;
            _maxTaskLength = maxTaskLength;
        }

        public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
        {
            return _tasks.Where(t => t.User.UserId == userId).ToList();
        }

        public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
        {
            return _tasks.Where(t => t.User.UserId == userId && t.State == ToDoItemState.Active).ToList();
        }

        public ToDoItem Add(ToDoUser user, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название задачи не может быть пустым");

            if (name.Length > _maxTaskLength)
                throw new TaskLengthLimitException(name.Length, _maxTaskLength);

            if (_tasks.Any(t => t.User.UserId == user.UserId && t.Name == name))
                throw new DuplicateTaskException(name);

            var userTasksCount = _tasks.Count(t => t.User.UserId == user.UserId);
            if (userTasksCount >= _maxTasks)
                throw new TaskCountLimitException(_maxTasks);

            var newTask = new ToDoItem(user, name);
            _tasks.Add(newTask);
            return newTask;
        }

        public void MarkCompleted(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                throw new ArgumentException($"Задача с Id '{id}' не найдена");

            task.Complete();
        }

        public void Delete(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                throw new ArgumentException($"Задача с Id '{id}' не найдена");

            _tasks.Remove(task);
        }
    }
}