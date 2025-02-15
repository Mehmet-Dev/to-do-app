namespace ToDoApp.Helpers;

// Stupid TypeTextHelper. It's just fancy Console.WriteLine.
public static class TypeTextHelper
{
    public static void TypeText(string text, int delay = 10)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(delay);
        }
        Console.Write("\n");
    }

    public static void TypeTextWithCooldown(string text, int delay = 10)
    {
        TypeText(text, delay);
        Thread.Sleep(1000);
    }
}
