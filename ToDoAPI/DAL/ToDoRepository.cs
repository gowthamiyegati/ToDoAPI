using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;

namespace ToDoAPI.DAL
{
    // This repository handles CRUD operations for the ToDo items using the DbContext.
    public class ToDoRepository: IToDoRepository
    {
        private readonly ToDoContext _context;

        // Constructor that injects the ToDoContext dependency to interact with the database.
        public ToDoRepository(ToDoContext context)
        {
            _context = context;
        }

        // Saves a collection of ToDo items in a single batch operation.
        public async Task SaveAllItems(IEnumerable<ToDoItem> toDoItems)
        {
            _context.ToDoItems.AddRange(toDoItems);
            await _context.SaveChangesAsync();
        }

        // Saves a single ToDo item to the database.
        public async Task SaveToDo(ToDoItem toDoItem)
        {
            _context.ToDoItems.Add(toDoItem);
            await _context.SaveChangesAsync();
        }

        // Retrieves a ToDo item by its unique ID.
        public async Task<ToDoItem> GetToDoById(int id) => await _context.ToDoItems.FindAsync(id);

        // Updates an existing ToDo item by applying changes from the input model.
        public async Task UpdateToDo(ToDoItem updatedItem)
        { 
            _context.ToDoItems.Update(updatedItem);
            await _context.SaveChangesAsync();
        }

        // Deletes a specific ToDo item from the database.
        public async Task DeleteToDo(ToDoItem item)
        {
            _context.ToDoItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        // Retrieves a filtered list of ToDo items based on optional search parameters.
        public async Task<IEnumerable<ToDoItem>> GetToDoList(string title, int? priority, DateTime? dueDate)
        {
            var query = _context.ToDoItems.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(t => t.Todo.Contains(title));

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            if (dueDate.HasValue)
                query = query.Where(t => t.DueDate == dueDate.Value);
            
            return await query
                .Include(item => item.Category)
                .ToListAsync();
        }

    }
}
