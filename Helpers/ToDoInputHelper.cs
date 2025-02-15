namespace ToDoApp.Helpers;
using System.Globalization;
using static TypeTextHelper;

// The all-mighty ToDoInputHelper. I wanted to be able to re-use a lot of my code instead of writing repetitive code, so I made this class.
// At any given moment, during execution of any method in here, when a user inputs a "q", it throws a UserOperationCanceledException.
// In every instance where this class is used, it's wrapped in a try-catch, that'll catch the error safely and return normal execution.
public class ToDoInputHelper
{
    private static readonly string[] PrioChoices = { "Low", "Medium", "High" };

    public static string? GetTitle()
    {
        string title;
        do
        {
            Console.Clear();
            TypeText("What should be the title of your new to-do? Q to exit.");
            title = Console.ReadLine()!;

            if (CheckForQuit(title))
                throw new UserOperationCanceledException("Quitting...");

            if (string.IsNullOrEmpty(title))
            {
                TypeTextWithCooldown("Uh oh, that's an empty title! Let's try again.");
            }
            else
            {
                break;
            }
        } while (true);

        return title;
    }

    public static string? GetDescription()
    {
        string desc;
        do
        {
            Console.Clear();
            TypeText("Add a description to your new to-do!\nDescription: ");
            desc = Console.ReadLine()!;

            if(CheckForQuit(desc))
                throw new UserOperationCanceledException("Quitting...");

            if (string.IsNullOrEmpty(desc))
            {
                TypeTextWithCooldown("An empty description... not so smart!");
            }
            else
            {
                break;
            }
        } while (true);

        return desc;
    }

    public static string? GetPriority()
    {
        string prio;
        do
        {
            Console.Clear();
            TypeText("Add a priority. Here are your possible choices:\n1 for low, 2 for medium and 3 for high!");

            string prompt = Console.ReadLine()!;
            if(CheckForQuit(prompt))
                throw new UserOperationCanceledException("Quitting...");

            if (int.TryParse(prompt, out int result) && result >= 1 && result <= 3)
            {
                prio = PrioChoices[result - 1];
                break;
            }
            else
            {
                TypeTextWithCooldown("Uh oh, invalid choice! Try again.");
            }
        } while (true);

        return prio;
    }

    public static DateTime? GetDueDate()
    {
        DateTime dueDate;
        do
        {
            Console.Clear();
            TypeText("Add a date and time to this to-do! Write in this format: dd/mm/yyyy hh:mm\nExample: 20/02/2020 15:25");

            string prompt = Console.ReadLine()!;
            if(CheckForQuit(prompt))
                throw new UserOperationCanceledException("Quitting...");

            if (DateTime.TryParseExact(prompt.Trim(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDate))
            {
                if (dueDate < DateTime.Now)
                {
                    TypeTextWithCooldown("Uh oh, that's earlier than today! Try again!");
                }
                else
                {
                    break;
                }
            }
            else
            {
                TypeTextWithCooldown("Invalid format! Try again.");
            }
        } while (true);

        return dueDate;
    }

    private static bool CheckForQuit(string input)
    {
        if(string.Equals(input, "q", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
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
}
