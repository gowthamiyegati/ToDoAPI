using ToDoAPI.Models;

namespace ToDoAPI.DAL
{
    public interface IToDoRepository
    {
        Task SaveAllItems(IEnumerable<ToDoItem> toDoItems);
        Task SaveToDo(ToDoItem toDoItem);
        Task UpdateToDo(ToDoItem item);
        Task DeleteToDo(ToDoItem item);
        Task<ToDoItem> GetToDoById(int id);
        Task<IEnumerable<ToDoItem>> GetToDoList(string title, int? priority, DateTime? dueDate);

    }
}
