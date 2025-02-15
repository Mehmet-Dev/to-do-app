using ToDoApp.Models;
using ToDoApp.Helpers;
using static ToDoApp.Helpers.TypeTextHelper;
using Microsoft.EntityFrameworkCore.Query.Internal;

//This is the main part of the code where most of the magic happens, the main loop if you may.
//In here, all parts of code interact with each other, from controllers to services, etc.
namespace ToDoApp.Controllers
{
    public class MenuController
    {
        //This part is mostly self-explanatory.
        private readonly UserController _userController; 
        private readonly ToDoController _toDoController;

        public MenuController(UserController userController, ToDoController toDoController)
        {
            _userController = userController;
            _toDoController = toDoController;
        }

        public void ShowMenu()
        {
            bool initialComplete = false;

            while (!initialComplete)
            {
                InitialMenu();
            }
        }
        /// <summary>
        /// The initial menu, in this menu you can log-in, register and essentially, exit.
        /// </summary>
        private void InitialMenu()
        {
            Console.Clear();
            TypeTextHelper.TypeText("Welcome to your to-do app! Please choose an option.");
            TypeText("[r] Register user\n[l] Login user\n[e] Exit");

            var keyInfo = Console.ReadKey(true);
            switch (keyInfo.KeyChar)
            {
                case 'r':
                    RegisterMenu();
                    break;
                case 'l':
                    LoginMenu();
                    break;
                case 'e':
                    Console.Clear();
                    TypeText("Thank you for using the real one and only to-do application!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    TypeTextWithCooldown("Oops! That was not an item in the menu. Let's retry!");
                    break;
            }

        }

        /// <summary>
        /// This is the register menu so you can register to the application.
        /// Essentially, multiple users can use the application in one device... It uses a local database.
        /// </summary>
        private void RegisterMenu()
        {
            Console.Clear();
            TypeTextWithCooldown("Welcome! Let's register you into the database.");

            bool registerDone = false;

            bool completedRegistration = false;

            while (!registerDone)
            {
                Console.Clear();
                TypeText("Please write a username, \"exit\" to go back!");

                string username = Console.ReadLine()!;

                if (username == "exit")
                {
                    break;
                }

                if (username.Length > 26) // Most of the code here is just input validation.
                {
                    Console.Clear();
                    TypeTextWithCooldown("Whoops, that username is too long! Let's retry.");
                }
                else
                {
                    while (true)
                    {
                        Console.Clear();
                        TypeText("Insert a password! Minimum of 8 characters.");
                        string password = Console.ReadLine()!;

                        if (password.Length < 8)
                        {
                            TypeTextWithCooldown("Whoops! That password is too short...");
                            break;
                        }

                        Console.Clear();

                        TypeText($"Here's what you've put in!\nUsername: {username}\nPassword: {password}\nDo you want to register with this? y for yes, n for no!");
                        var keyInfo = Console.ReadKey(true);

                        if (keyInfo.KeyChar == 'y')
                        {
                            if (_userController.Register(username, password))
                            {
                                Console.Clear();

                                TypeTextWithCooldown("You have been registered!");
                                completedRegistration = true;
                                break;
                            }
                            else
                            {
                                Console.Clear();
                                TypeTextWithCooldown("Turns out someone with the same username is already registered... Try again.");
                                break;
                            }
                        }
                        else break;
                    }
                }

                if (completedRegistration) break;
            }

            Console.Clear();
            TypeTextWithCooldown("Going back...");
        }

        /// <summary>
        /// The log-in menu. They log-in.
        /// </summary>
        private void LoginMenu()
        {
            bool loginComplete = false;

            while (!loginComplete)
            {
                Console.Clear();
                TypeText("Write your username down, \"exit\" to exit!");
                string username = Console.ReadLine()!;

                if (username == "exit")
                {
                    Console.Clear();
                    TypeTextWithCooldown("Going back to main menu...");
                    break;
                }

                Console.Clear();
                TypeText("Write down the password!");
                string password = Console.ReadLine()!;

                var user = _userController.Login(username, password);

                if (user != null)
                {
                    Console.Clear();
                    TypeTextWithCooldown("You are logged in!");
                    MainMenu(user);
                }
                else
                {
                    Console.Clear();
                    TypeTextWithCooldown("Uh oh, we couldn't log you in with the provided credentials. Try again!");
                }
            }
        }

        /// <summary>
        /// The main menu where most of the functionality resides. Users can view their to-do's, add new ones, and exit.
        /// </summary>
        /// <param name="user">The user that's logged in.</param>
        public void MainMenu(User user) 
        {
            while (true)
            {
                Console.Clear();
                TypeTextWithCooldown("Choose what you want to do next!\n[v] View to-do's\n[a] Add a new to-do\n[e] Exit");

                var keyInfo = Console.ReadKey(true);
                switch (char.ToLower(keyInfo.KeyChar))
                {
                    case 'v':
                        ViewToDos(user);
                        break;
                    case 'a':
                        AddToDo(user);
                        break;
                    case 'e':
                        Console.Clear();
                        TypeTextWithCooldown("Thanks for using my amazing cool program wow");
                        Environment.Exit(0);
                        break;
                    case 'u':
                        break;
                    default:
                        Console.Clear();
                        TypeTextWithCooldown("Whoops, an invalid choice. Let's try that again!");
                        break;
                }
            }
        }

        /// <summary>
        /// You can view your to-do's here.
        /// </summary>
        /// <param name="user">The user that's logged in.</param>
        public void ViewToDos(User user)
        {
            do
            {
                Console.Clear();
                if (user.ToDoItems == null || user.ToDoItems.Count == 0)
                {
                    TypeTextWithCooldown("Whoops, you don't have any items! Add some in the previous menu!");
                    return;
                }
                int i = 1;
                foreach (ToDoItem todo in user.ToDoItems)
                {
                    TimeSpan diff = todo.DueDate - DateTime.Now;
                    string timeLeft = diff.TotalSeconds < 0
                        ? "Too late!"
                        : diff.Days > 0
                            ? $"Due in {diff.Days}d {diff.Hours}h {diff.Minutes}m"
                            : diff.Hours > 0
                                ? $"Due in {diff.Hours}h {diff.Minutes}m"
                                : $"Due in {diff.Minutes}m"; // This right here is a bunch of other logic that determines the differences in days, hours and minutes.
                    TypeText($"[{i}] {todo.Title} - {(todo.IsCompleted ? "Completed!" : "Yet to be completed.")} - {timeLeft}");
                    i++;
                }

                TypeText("\nType in the number of the task you want to set as completed and press enter, or else, go back to the previous menu!\nYou can also type 'clear' to clear out all overdue items, and update to edit a to-do.");
                string prompt = Console.ReadLine()!;

                // This part of the code checks whether they put in a number, if not it's most likely a character.
                if (int.TryParse(prompt, out int index) && index <= user.ToDoItems.Count && index >= 0)
                {
                    var todoItem = user.ToDoItems[index - 1];

                    Console.Clear();
                    TypeText($"{todoItem}\nDo you want to set this as completed? (y/n)");
                    var keyInfo = Console.ReadKey(true);

                    if (keyInfo.KeyChar == 'y')
                    {
                        _toDoController.CompleteToDoItem(user.ID, index);
                        Console.Clear();

                        TypeTextWithCooldown("To-do has been completed!");
                    }
                }
                else
                {
                    switch (prompt.ToLower())
                    {
                        case "clear": // Clearing overdue and completed items
                            _toDoController.RemoveCompletedAndOverdueItems(user.ID);
                            TypeTextWithCooldown("Completed (and overdue) items have been removed!");
                            break;
                        case "update": // Updating a to-do
                            TypeTextWithCooldown("Which to-do do you want to edit?");
                            if(int.TryParse(Console.ReadLine(), out int todo) && todo <= user.ToDoItems.Count && index >= 0)
                            {
                                UpdateToDo(user.ID, user.ToDoItems[todo - 1], todo);
                            }
                            else
                                TypeText("Well, didn't find an item with that number. Try again!");
                            break;
                        case "delete":
                            TypeText("Which to-do do you want to delete?");
                            if(int.TryParse(Console.ReadLine(), out int to_do) && to_do <= user.ToDoItems.Count && index >= 0)
                            {
                                _toDoController.DeleteToDo(user.ID, to_do);
                                TypeTextWithCooldown("To-do has been deleted!");
                            }
                            else
                                TypeText("Well, didn't find an item with that number. Try again!");
                            break;
                        default: //If they put anything else in, just send them back to the main menu.
                            TypeTextWithCooldown("Going back to main menu...");
                            return;
                    }
                }
            } while (true);
        }

        /// <summary>
        /// Part of the code that adds a new to do.
        /// </summary>
        /// <param name="user">The logged in user.</param>
        public void AddToDo(User user)
        {
            Console.Clear();

            try // Most of the code is handled in the ToDoInputHelper class. If a user exits at any part of the code, it returns a UserOperationCanceledException, the try-catch catches it and just assumes to go back to the main menu.
            {
                string title = ToDoInputHelper.GetTitle();
                string desc = ToDoInputHelper.GetDescription();
                string prio = ToDoInputHelper.GetPriority();
                DateTime? dueDate = ToDoInputHelper.GetDueDate();

                Console.Clear();
                _toDoController.AddToDoItem(user.ID, title, desc, prio, (DateTime)dueDate);
                TypeTextWithCooldown("The to-do has been added!");
            }
            catch (UserOperationCanceledException ex)
            {
                TypeTextWithCooldown(ex.Message);  // Inform the user about cancellation
            }
        }

        /// <summary>
        /// This updates a given to-do.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="toDo">The desired to-do to be updated.</param>
        /// <param name="index">The index of the to-do.</param>
        public void UpdateToDo(int userId, ToDoItem toDo, int index)
        {
            Console.Clear();
            //I don't know if it's obvious, but I like having short code. So I tried to make this as tiny as possible trying to use complex ways. I suppose I managed.
            //Again, it's mostly just handled in ToDoInputHelper class.
            string[] props = new string[] { "Title", "Description", "Priority", "DueDate" };
            Func<object?>[] updateActions =
                    [
                        () => ToDoInputHelper.GetTitle(),
                        () => ToDoInputHelper.GetDescription(),
                        () => ToDoInputHelper.GetPriority(),
                        () => ToDoInputHelper.GetDueDate(),
                    ];
            do
            {
                TypeText("You can change the following:");
                int i = 1;
                foreach (var property in props) // This foreach loop displays what the user can change, and their current values next to it.
                {
                        var propertyInfo = typeof(ToDoItem).GetProperty(property);
                        var currentValue = propertyInfo?.GetValue(toDo)?.ToString() ?? "N/A";

                    TypeText($"[{i}]: {property}, Current value: {currentValue}");
                    i++;
                }

                TypeText("Please choose with the index, type 'q' to quit.");
                string prompt = Console.ReadLine()!;

                if (string.Compare(prompt, "q", StringComparison.OrdinalIgnoreCase) == 0) return; // Quitting, very simple.

                if (int.TryParse(prompt, out int prop) && prop >= 1 && prop <= props.Length)
                {
                    try // Again, in any case if they desire to quit, it throws a special exception, the try-catch catches it and they go back to the previous menu. Smart, right?
                    {
                        object newValue = updateActions[prop - 1]();

                        switch (prop)
                        {
                            case 1:
                                toDo.Title = newValue.ToString();
                                break;
                            case 2:
                                toDo.Description = newValue.ToString();
                                break;
                            case 3:
                                toDo.Priority = newValue.ToString();
                                break;
                            case 4:
                                toDo.DueDate = (DateTime)newValue;
                                break;
                        }

                        _toDoController.UpdateToDo(userId, index, toDo.Title, toDo.Description, toDo.Priority, toDo.DueDate);
                        Console.Clear();
                        TypeTextWithCooldown("To do has been updated!");
                        return;
                    }
                    catch (UserOperationCanceledException ex)
                    {
                        Console.Clear();
                        TypeTextWithCooldown(ex.Message);
                    }
                }
                else TypeTextWithCooldown("Invalid input!");

            } while (true);
        }
    }
}