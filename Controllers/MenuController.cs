using System.Diagnostics;
using System.Globalization;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class MenuController
    {
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

        private void InitialMenu()
        {
            Console.Clear();
            TypeText("Welcome to your to-do app! Please choose an option.");
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

        private void RegisterMenu()
        {
            Console.Clear();
            TypeTextWithCooldown("Welcome! Let's register you into the database.");

            bool registerDone = false;

            bool completedRegistration = false;

            while (!registerDone)
            {
                TypeText("Please write a username, \"exit\" to go back!");

                string username = Console.ReadLine()!;

                if (username == "exit")
                {
                    break;
                }

                if (username.Length > 26)
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
                        }
                        else break;
                    }
                }

                if (completedRegistration) break;
            }

            Console.Clear();
            TypeTextWithCooldown("Going back...");
        }

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

        public void MainMenu(User user)
        {
            while (true)
            {
                Console.Clear();
                TypeTextWithCooldown("Choose what you want to do next!\n[v] View to-do's\n[a] Add a new to-do\n[e] Exit");

                var keyInfo = Console.ReadKey(true);
                switch (keyInfo.KeyChar)
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
                    default:
                        Console.Clear();
                        TypeTextWithCooldown("Whoops, an invalid choice. Let's try that again!");
                        break;
                }
            }
        }

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
                                : $"Due in {diff.Minutes}m";
                    TypeText($"[{i}] {todo.Title} - {todo.Description} - {(todo.IsCompleted ? "Completed!" : "Yet to be completed.")} - {timeLeft}");
                    i++;
                }

                TypeText("\nType in the number of the task you want to set as completed and press enter, or else, go back to the previous menu!\nYou can also type 'clear' to clear out all overdue items, and update to edit a to-do.");
                string prompt = Console.ReadLine()!;

                if (int.TryParse(prompt, out int index) && index <= user.ToDoItems.Count)
                {
                    var todoItem = user.ToDoItems[index - 1];

                    Console.Clear();
                    TypeTextWithCooldown($"Do you want to complete task \"{todoItem.Title}\"? (y/n)");
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
                    switch (prompt)
                    {
                        case "clear":
                            _toDoController.RemoveCompletedItems(user.ID);
                            TypeTextWithCooldown("Completed items have been removed!");
                            break;
                        case "update":
                            Process.Start(new ProcessStartInfo { FileName = "https://www.youtube.com/watch?v=dQw4w9WgXcQ", UseShellExecute = true, });
                            break;
                        default:
                            TypeTextWithCooldown("Going back to main menu...");
                            return;
                    }
                }
            } while (true);
        }

        public void AddToDo(User user)
        {
            Console.Clear();

            try
            {
                string title = ToDoInputHelper.GetTitle();
                string desc = ToDoInputHelper.GetDescription();
                string prio = ToDoInputHelper.GetPriority();
                DateTime? dueDate = ToDoInputHelper.GetDueDate();

                Console.Clear();
                _toDoController.AddToDoItem(user.ID, title, desc, prio, (DateTime)dueDate);
                TypeTextWithCooldown("The to-do has been added!");
            }
            catch (OperationCanceledException ex)
            {
                TypeTextWithCooldown(ex.Message);  // Inform the user about cancellation
            }
        }


        public static void UpdateToDo(int userId, ToDoItem toDo, int index)
        {
            string[] props = new string[] { "Name", "Description", "Priority", "Due date" };
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
                foreach (var property in props)
                {
                    TypeText($"[{i}]: {property}");
                    i++;
                }

                TypeText("Please choose with the index, type 'q' to quit.");
                string prompt = Console.ReadLine()!;

                if (string.Compare(prompt, "q", StringComparison.OrdinalIgnoreCase) == 0) return;

                if (int.TryParse(prompt, out int prop) && prop >= 1 && prop <= props.Length)
                {
                    try
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

                        _toDoController.Updat
                    }
                    catch (OperationCanceledException ex)
                    {
                        Console.Clear();
                        TypeTextWithCooldown(ex.Message);
                    }
                }
                else TypeTextWithCooldown("Invalid input!");

            } while (true);
        }


        private static void TypeText(string text, int delay = 10)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.Write("\n");
        }

        private static void TypeTextWithCooldown(string text, int delay = 10)
        {
            TypeText(text, delay);
            Thread.Sleep(1000);
        }
    }
}