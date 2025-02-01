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
                    TypeText("Oops! That was not an item in the menu. Let's retry!");
                    break;
            }

        }

        private void RegisterMenu()
        {
            Console.Clear();
            TypeText("Welcome! Let's register you into the database.");

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
                    TypeText("Whoops, that username is too long! Let's retry.");
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
                            TypeText("Whoops! That password is too short...");
                        }

                        Console.Clear();

                        TypeText($"Here's what you've put in!\nUsername: {username}\nPassword: {password}\nDo you want to register with this? y for yes, n for no!");
                        var keyInfo = Console.ReadKey(true);

                        if (keyInfo.KeyChar == 'y')
                        {
                            if (_userController.Register(username, password))
                            {
                                Console.Clear();

                                TypeText("You have been registered!");
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
            TypeText("Going back...");
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
                    TypeText("Going back to main menu...");
                    break;
                }

                Console.Clear();
                TypeText("Write down the password!");
                string password = Console.ReadLine()!;

                var user = _userController.Login(username, password);

                if (user != null)
                {
                    Console.Clear();
                    TypeText("You are logged in!");
                    MainMenu(user);
                }
                else
                {
                    Console.Clear();
                    TypeText("Uh oh, we couldn't log you in with the provided credentials. Try again!");
                }
            }
        }

        public void MainMenu(User user)
        {
            while (true)
            {
                Console.Clear();
                TypeText("Choose what you want to do next!\n[v] View to-do's\n[a] Add a new to-do\n[e] Exit");

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
                        TypeText("Thanks for using my amazing cool program wow");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        TypeText("Whoops, an invalid choice. Let's try that again!");
                        break;
                }
            }
        }

        public void ViewToDos(User user)
        {
            Console.Clear();
            if (user.ToDoItems == null || user.ToDoItems.Count == 0)
            {
                TypeText("Whoops, you don't have any items! Add some in the previous menu!");
                return;
            }
            int i = 1;
            foreach (ToDoItem todo in user.ToDoItems)
            {
                TypeText($"[{i}] {todo.Title} - {todo.Description} - {(todo.IsCompleted ? "Completed!" : "Yet to be completed.")}");
            }

            TypeText("\nType in the number of the task you want to set as completed and press enter, or else, go back to the previous menu!");

            if (int.TryParse(Console.ReadLine(), out int index) && index <= user.ToDoItems.Count)
            {
                var todoItem = user.ToDoItems[index - 1];

                Console.Clear();
                TypeText($"Do you want to complete task \"{todoItem.Title}\"? (y/n)");
                var keyInfo = Console.ReadKey(true);

                if(keyInfo.KeyChar == 'y')
                {
                    _toDoController.CompleteToDoItem(user.ID, index);
                    Console.Clear();

                    TypeText("To-do has been completed!");
                }
            }
            else
            {
                TypeText("Uh oh, you didn't write a valid input! Let's try that again.");
            }
        }

        public void AddToDo(User user)
        {
            bool isCompleted = false;
            while (!isCompleted)
            {
                Console.Clear();
                TypeText("What should be the title of your new to-do?");
                do
                {
                    string title = Console.ReadLine()!;

                    if(title == "")
                    {
                        TypeText("Uh oh, that's an empty title! Let's try again.");
                    }
                    break;
                } while(true);

                do
                {
                    
                } while(true);
            }
            Console.Clear();
            TypeText("The to-do has been added!");
        }


        private static void TypeText(string text, int delay = 10)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.Write("\n");
            Thread.Sleep(1000);
        }
    }
}