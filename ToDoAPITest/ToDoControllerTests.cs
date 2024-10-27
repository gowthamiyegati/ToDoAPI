using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoAPI.Controllers;
using ToDoAPI.Models;
using Newtonsoft.Json;
using Moq;
using Microsoft.Extensions.Configuration;
using ToDoAPI.DAL;
using ToDoAPI.Services;

namespace ToDoAPITest
{
    public class ToDoControllerTests
    {
        private Mock<HttpClient> _httpClientMock;
        private ToDoContext _dbContext;
        private ToDoController _controller;
        private Mock<IConfiguration> _configMock;

        [SetUp]
        public void Setup()
        {
            // Setup in-memory database for testing
            var options = new DbContextOptionsBuilder<ToDoContext>()
                .UseInMemoryDatabase(databaseName: "TestToDoDb")
                .Options;

            _dbContext = new ToDoContext(options);

            // Mock HttpClient for external API calls (like fetching weather)
            _httpClientMock = new Mock<HttpClient>();

            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(c => c["WeatherApiKey"]).Returns("1859068befd74267bb6195716242410");

            // Setup controller with the mock dependencies
            ToDoRepository toDoRepository = new ToDoRepository(_dbContext);
            ToDoService toDoService = new ToDoService(toDoRepository, _httpClientMock.Object, _configMock.Object);
            _controller = new ToDoController(toDoService);
        }
        [Test]
        public async Task AddToDo_ShouldAddNewItem()
        {
            // Arrange
            var newToDo = new ToDoItem { Todo = "New ToDo", Priority = 2 };

            // Act
            var result = await _controller.CreateToDoItem(newToDo);

            // Assert
            var storedToDo = await _dbContext.ToDoItems.FirstOrDefaultAsync(t => t.Todo == "New ToDo");
            Assert.IsNotNull(storedToDo);
            Assert.AreEqual(2, storedToDo.Priority);
        }

        [Test]
        public async Task UpdateToDo_ShouldModifyExistingItem()
        {
            // Arrange
            var existingToDo = new ToDoItem { Todo = "Existing ToDo", Priority = 3 };
            _dbContext.ToDoItems.Add(existingToDo);
            await _dbContext.SaveChangesAsync();

            existingToDo.Priority = 1;

            // Act
            var result = await _controller.UpdateToDoItem(existingToDo);

            // Assert
            var updatedToDo = await _dbContext.ToDoItems.FindAsync(existingToDo.Id);
            Assert.AreEqual(1, updatedToDo.Priority);
        }

        [Test]
        public async Task DeleteToDo_ShouldRemoveItemFromDatabase()
        {
            // Arrange
            var toDo = new ToDoItem { Todo = "To be deleted", Priority = 3 };
            _dbContext.ToDoItems.Add(toDo);
            await _dbContext.SaveChangesAsync();

            // Act
            await _controller.DeleteToDoItem(toDo.Id);

            // Assert
            var deletedToDo = await _dbContext.ToDoItems.FindAsync(toDo.Id);
            Assert.IsNull(deletedToDo);
        }

      

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();  // Clean up in-memory database
        }
    }
}
