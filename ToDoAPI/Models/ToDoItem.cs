using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.Models
{
    public class ToDoItem
    {       
        public int Id { get; set; }
        public string Todo { get; set; }
        public bool Completed { get; set; }
        public int UserId { get; set; }
        public int Priority { get; set; } = 3; // Default priority
        public DateTime? DueDate { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? WeatherCondition { get; set; }
        public double? Temperature { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
