using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;
using ToDoAPI.Services;

namespace ToDoAPI.Controllers
{
    // This controller handles API requests related to ToDo items
    [ApiController]
    [Route("api/todo")]
    public class ToDoController : Controller
    {
        // Dependency injection for ToDoService, ensuring modularity and testability
        private IToDoService _todoService;

        public ToDoController(IToDoService toDoService)
        {
            // Injecting the IToDoService instance to access business logic
            _todoService = toDoService;
        }

        // API endpoint to fetch external ToDo data and store it in the local database
        [HttpGet("fetch")]
        public async Task<IActionResult> FetchToDos()
        {
            try
            {
                // Calls service method to collect ToDos and handle data persistence
                await _todoService.CollectToDos();
                return Ok("Fetched and stored ToDo items successfully.");
            }
            catch(DbUpdateException dbEx)
            {
                return BadRequest(new { Message = "Database update failed.", Details = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        // API endpoint to create a new ToDo item
        [HttpPost]
        public async Task<IActionResult> CreateToDoItem([FromBody] ToDoItem item)
        {
            try
            {
                // Inserting a new ToDo item through the service, ensuring data consistency
                ToDoItem toDoItem = await _todoService.InsertToDoItem(item);
                return Ok(toDoItem);
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { Message = "Database update failed.", Details = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        // API endpoint to update an existing ToDo item
        [HttpPut]
        public async Task<IActionResult> UpdateToDoItem([FromBody] ToDoItem updatedItem)
        {
            try
            {
                // Attempts to update the ToDo item, checking if it exists
                ToDoItem toDoItem = await _todoService.UpdateToDoItem(updatedItem);
                if (toDoItem != null)
                    return Ok(toDoItem);
                else
                    return NotFound("Item does not exist.");
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { Message = "Database update failed.", Details = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        // API endpoint to delete a ToDo item by its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            try
            {
                // Calls service to delete the item, receiving a response string for success/failure
                string returnContent = await _todoService.DeleteToDoItem(id);
                if (returnContent != null)
                    return Ok("Item deleted successfully.");
                else
                    return NotFound("Item does not exist.");
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { Message = "Database update failed.", Details = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        // API endpoint to search ToDo items based on title, priority, or due date
        [HttpGet("search")]
        public async Task<IActionResult> SearchToDoItems(string? title, int? priority, DateTime? dueDate)
        {
            try
            {
                // Retrieves matching ToDo items using service method
                var toDoItems = await _todoService.GetToDoItems(title, priority, dueDate);
                if (toDoItems != null)
                    return Ok(toDoItems);
                else
                    return NotFound("Item does not exist.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        // API endpoint to get weather data for a specific ToDo item based on its location
        [HttpGet("{id}/weather")]
        public async Task<IActionResult> GetToDoWeather(int id)
        {
            try
            {
                // Retrieves item details along with current weather if location is set
                var item = await _todoService.GetToDoItemWeather(id);
                if (item != null)
                    return Ok(new { item.Todo, item.WeatherCondition, item.Temperature });
                else
                    return NotFound("Item does not exist.");
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { Message = "Database update failed.", Details = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
