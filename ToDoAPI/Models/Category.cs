using System.Runtime.InteropServices;

namespace ToDoAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
    }
}
