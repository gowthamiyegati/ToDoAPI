using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;

namespace ToDoAPI.DAL
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }

        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
