using System.Globalization;

public class ToDoInputHelper
{
    private static readonly string[] PrioChoices = { "Low", "Medium", "High" };

    public static string GetTitle()
    {
        string title;
        do
        {
            Console.Clear();
            TypeTextWithCooldown("What should be the title of your new to-do?");
            title = Console.ReadLine()!;

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

    public static string GetDescription()
    {
        string desc;
        do
        {
            Console.Clear();
            TypeTextWithCooldown("Add a description to your new to-do!\nDescription: ");
            desc = Console.ReadLine()!;

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

    public static string GetPriority()
    {
        string prio;
        do
        {
            Console.Clear();
            TypeTextWithCooldown("Add a priority. Here are your possible choices:\n1 for low, 2 for medium and 3 for high!");

            if (int.TryParse(Console.ReadLine(), out int result) && result >= 1 && result <= 3)
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

    public static DateTime GetDueDate()
    {
        DateTime dueDate;
        do
        {
            Console.Clear();
            TypeTextWithCooldown("Add a date and time to this to-do! Write in this format: dd/mm/yyyy hh:mm\nExample: 20/02/2020 15:25");

            if (DateTime.TryParseExact(Console.ReadLine()!.Trim(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDate))
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

    private static void TypeTextWithCooldown(string text)
    {
        Console.WriteLine(text);
        System.Threading.Thread.Sleep(1000); // Simulating a cooldown
    }
}
