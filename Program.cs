using ToDoApp.Controllers;
using ToDoApp.Models;
using ToDoApp.Services;

public static class Program
{
    public static void Main(string[] args)
    {
        using var dbContext = new AppDbContext();
        // Service initialize
        var authService = new AuthService(dbContext);
        var toDoService = new ToDoService(dbContext);
        // Controllers initialize
        var userController = new UserController(authService);
        var toDoController = new ToDoController(toDoService);

        // Create an instance of the MenuController with the other controllers
        var menuController = new MenuController(userController, toDoController);

        // Start the application by showing the menu
        menuController.ShowMenu();
    }
}
