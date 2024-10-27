# ToDoAPI

ToDoAPI is a RESTful API built with ASP.NET Core, Entity Framework Core, and SQL Server to manage ToDo items. This project includes basic CRUD functionality, filtered search, and integration with a weather API to fetch weather conditions based on each ToDo item’s location. 

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Design and Coding Decisions](#Design-and-Coding-Decisions)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Testing with NUnit](#testing-with-nunit)
- [Configuration](#configuration)
- [Database Migrations](#database-migrations)
- [Contributing](#contributing)

## Features

- **CRUD Operations**: Create, read, update, and delete ToDo items.
- **Filtered Search**: Search for ToDo items by title, priority, or due date.
- **Weather Integration**: Retrieve weather conditions for ToDo items based on location.
- **Layered Architecture**: Organized code with distinct layers for controllers, services, and data access.

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Newtonsoft.Json for JSON handling
- Dependency Injection and Repository Pattern
- Weather API integration

## Design and Coding Decisions

- Layered Architecture: The project follows a layered architecture pattern that separates concerns into distinct layers, including the Controller, Service, and Repository layers. This separation makes the code more maintainable and testable.

- Dependency Injection: ASP.NET Core's built-in dependency injection is utilized to manage the dependencies of services and repositories, enhancing the flexibility of the code and promoting loose coupling.

- In-Memory Database for Testing: The use of an in-memory database during testing ensures that tests can run quickly without the need for a persistent database. This allows for fast iteration during development.

- DTOs and Entity Models: The use of Data Transfer Objects (DTOs) separates the API contract from the database models. This provides more control over the data being sent over the network and helps prevent over-posting attacks.

- Error Handling and Responses: The API returns appropriate HTTP status codes for different outcomes (e.g., 200 OK, 404 Not Found). This adherence to RESTful principles improves the API's usability.

## Project Structure

- **Controllers**: Handles HTTP requests and responses.
- **Services**: Business logic layer (e.g., ToDo operations, weather fetching).
- **Data Access Layer (DAL)**: Manages database operations using Entity Framework Core.
- **Models**: Defines the structure of ToDo items and any external API response models.

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Visual Studio or VS Code

### Installation

1. **Clone the repository**:
    ```bash
    git clone https://github.com/gowthamiyegati/ToDoAPI.git
    cd ToDoAPI
    ```

2. **Configure Database Connection**: In `appsettings.json`, set up your database connection string.

3. **Add Weather API Key**: Configure `WeatherApiKey` in `appsettings.json` to your API key.

4. **Run Database Migrations**:
    ```bash
    dotnet ef database update
    ```

5. **Start the Application**:
    ```bash
    dotnet run
    ```

### Running the Project

After running the above commands, the API should be accessible at `https://localhost:5001`.

## API Endpoints

### ToDo Management

- **GET /api/todo/fetch**: Fetches ToDo items from an external API and stores them in the database.
- **POST /api/todo**: Adds a new ToDo item.
- **PUT /api/todo**: Updates an existing ToDo item.
- **DELETE /api/todo/{id}**: Deletes a specific ToDo item.
- **GET /api/todo/search**: Searches for ToDo items based on filters like title, priority, and due date.

### Weather Integration

- **GET /api/todo/{id}/weather**: Retrieves current weather conditions for the location associated with a ToDo item.

## Testing with NUnit

This project includes unit tests implemented with NUnit to verify the functionality of core API endpoints in the ToDoController. The tests use the Moq library to mock dependencies such as HttpClient and IConfiguration, and they leverage an in-memory database to isolate and test specific functionalities without affecting a production database.

### Key Tests
- AddToDo_ShouldAddNewItem: Verifies that a new ToDoItem is successfully added to the database.
- UpdateToDo_ShouldModifyExistingItem: Ensures that an existing ToDoItem can be updated.
- DeleteToDo_ShouldRemoveItemFromDatabase: Confirms that a ToDoItem is successfully removed from the database.
- Test Environment Setup and Cleanup: Each test is isolated using an in-memory database, which is deleted after each test to maintain a clean state.

## Configuration

Update `appsettings.json` for:

- **Database Connection**: Add your SQL Server connection string.
- **Weather API**: Set the `WeatherApiKey` value with a valid API key from a weather service.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string here"
  },
  "WeatherApiKey": "Your API Key here"
}
