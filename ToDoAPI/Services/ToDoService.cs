using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;
using ToDoAPI.DAL;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    // Service class for managing ToDo operations, implementing IToDoService
    public class ToDoService: IToDoService
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ToDoService(IToDoRepository toDoRepository, HttpClient httpClient, IConfiguration config)
        {
            _toDoRepository = toDoRepository;
            _httpClient = httpClient;
            _config = config;
        }

        // Method to fetch ToDo items from a remote API and save them in the local database
        public async Task CollectToDos()
        {
            // Fetches JSON response from the external ToDo API
            var response = await _httpClient.GetStringAsync("https://dummyjson.com/todos");
            var todoResponse = JsonConvert.DeserializeObject<ToDoResponse>(response);
            var toDoItems = todoResponse.Todos;

            //Setting the Id value to 0 for all the items because as a best practice 
            //let SQL Server handle ID generation by using the identity column feature.
            foreach (var item in toDoItems)
            {
                item.Id = 0;
            }

            await _toDoRepository.SaveAllItems(toDoItems);
        }

        // Method to insert a new ToDo item into the database
        public async Task<ToDoItem> InsertToDoItem(ToDoItem item)
        {
            //Setting the Id value of item to 0 because as a best practice 
            //let SQL Server handle ID generation by using the identity column feature.
            if (item.Id != 0)
            {
                item.Id = 0;
            }
            else if(item.Category != null && item.Category.Id != 0)
            {
                item.Category.Id = 0;
            }
            await _toDoRepository.SaveToDo(item);
        
            return item;
        }

        // Method to update an existing ToDo item, if found
        public async Task<ToDoItem> UpdateToDoItem(ToDoItem updatedItem)
        {
            var item = await _toDoRepository.GetToDoById(updatedItem.Id);
            if (item == null)
            {
                return null;
            }

            // Update item with only the non-null properties from the updated item
            UpdateNonNullProperties<ToDoItem>(item, updatedItem);
            await _toDoRepository.UpdateToDo(item);

            return item;
        }

        // Method to delete a ToDo item by ID
        public async Task<string> DeleteToDoItem(int id)
        {
            var item = await _toDoRepository.GetToDoById(id);
            if (item == null)
            {
                return null;
            }

            await _toDoRepository.DeleteToDo(item);

            return "Item deleted successfully.";
        }

        // Retrieves a filtered list of ToDo items based on title or priority or due date
        public async Task<IEnumerable<ToDoItem>> GetToDoItems(string title, int? priority, DateTime? dueDate)
        {
            return await _toDoRepository.GetToDoList(title, priority, dueDate);
        }

        // Method to fetch weather information for a ToDo item based on its location
        public async Task<ToDoItem> GetToDoItemWeather(int id)
        {
            var item = await _toDoRepository.GetToDoById(id);
            if (item == null || item.Latitude == null || item.Longitude == null)
            {
                return null;
            }

            // Prepare API request to fetch weather data using stored latitude and longitude
            string weatherApiKey = _config["WeatherApiKey"];
            string requestUri = $"http://api.weatherapi.com/v1/current.json?key={weatherApiKey}&q={item.Latitude},{item.Longitude}";

            var response = await _httpClient.GetStringAsync(requestUri);
            var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(response);

            // Update item with fetched weather details and save changes
            item.WeatherCondition = weatherData.Current.Condition.Text;
            item.Temperature = weatherData.Current.Temp_C;

            await _toDoRepository.UpdateToDo(item);

            return item;
        }

        // Generic method to update only the properties with non-null values from source to target
        public void UpdateNonNullProperties<T>(T target, T source)
        {
            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Ensure the property can be read and written
                if (property.CanRead && property.CanWrite)
                {
                    if (property.Name != "Id")// Skip updating the ID
                    {
                        var sourceValue = property.GetValue(source);

                        // Set property value only if source has a non-null value
                        if (sourceValue != null)
                        {
                            property.SetValue(target, sourceValue);
                        }
                    }
                }
            }
        }

    }
}
