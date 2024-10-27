namespace ToDoAPI.Models
{
    public class ToDoResponse
    {
        public List<ToDoItem> Todos { get; set; }
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
