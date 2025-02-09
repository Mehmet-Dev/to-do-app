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
                TypeTextWithCooldown("Please write a username, \"exit\" to go back!");

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
                        TypeTextWithCooldown("Insert a password! Minimum of 8 characters.");
                        string password = Console.ReadLine()!;

                        if (password.Length < 8)
                        {
                            TypeTextWithCooldown("Whoops! That password is too short...");
                        }

                        Console.Clear();

                        TypeTextWithCooldown($"Here's what you've put in!\nUsername: {username}\nPassword: {password}\nDo you want to register with this? y for yes, n for no!");
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
            }

            TypeTextWithCooldown("\nType in the number of the task you want to set as completed and press enter, or else, go back to the previous menu!\nYou can also type 'clear' to clear out all overdue items.");
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
            else if(prompt == "clear")
            {
                
            }
            else
            {
                TypeTextWithCooldown("Going back to main menu...");
            }
        }

        public void AddToDo(User user)
        {
            string title = "";
            string desc = "";
            string prio = "";
            string[] prioChoice =
            {
                "Low",
                "Medium",
                "High"
            };
            DateTime? dueDate = null;

            Console.Clear();
            TypeTextWithCooldown("What should be the title of your new to-do?");
            do
            {
                title = Console.ReadLine()!;

                if (string.IsNullOrEmpty(title))
                {
                    Console.Clear();
                    TypeTextWithCooldown("Uh oh, that's an empty title! Let's try again.");
                }
                else break;
            } while (true);

            do
            {
                Console.Clear();
                TypeTextWithCooldown("Add a description to your new to-do!\nDescription: ");
                desc = Console.ReadLine()!;

                if (string.IsNullOrEmpty(desc))
                {
                    TypeTextWithCooldown("An empty description... not so smart!");
                }
                else break;
            } while (true);

            do
            {
                Console.Clear();
                TypeTextWithCooldown("Add a priority. Here are your possible choices:\n1 for low, 2 for medium and 3 for high!");

                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    if (result >= 1 && result <= 3)
                    {
                        prio = prioChoice[result - 1];
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        TypeTextWithCooldown("Uh oh, it was out of the range. Attempt number 2! I think...");
                    }
                }
                else
                {
                    Console.Clear();
                    TypeTextWithCooldown("Not good, you didn't write a valid number! Let's try again.");
                }
            } while (true);

            do
            {
                Console.Clear();
                TypeTextWithCooldown("Add a date and time to this to-do! Write in this format: dd/mm/yyyy hh:mm (with the / and : included!)\nExample: 20/02/2020 15:25");

                if (DateTime.TryParseExact(Console.ReadLine()!.Trim(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    if (parsedDate < DateTime.Now)
                    {
                        Console.Clear();
                        TypeTextWithCooldown("UH oh, that's earlier than today! Try again!");
                        continue;
                    }
                    dueDate = parsedDate;
                    break;
                }
                else
                {
                    Console.Clear();
                    TypeTextWithCooldown("Uh oh, invalid format! Try again.");
                }

            } while (true);
            Console.Clear();
            _toDoController.AddToDoItem(user.ID, title, desc, prio, (DateTime)dueDate);
            TypeTextWithCooldown("The to-do has been added!");
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