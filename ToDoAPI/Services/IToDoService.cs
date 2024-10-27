using Microsoft.AspNetCore.Mvc;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    public interface IToDoService
    {
        Task CollectToDos();
        Task<ToDoItem> InsertToDoItem(ToDoItem toDoItem);
        Task<ToDoItem> UpdateToDoItem(ToDoItem updatedToDoItem);
        Task<string> DeleteToDoItem(int id);
        Task<IEnumerable<ToDoItem>> GetToDoItems(string title, int? priority, DateTime? dueDate);
        Task<ToDoItem> GetToDoItemWeather(int id);
    }
}
