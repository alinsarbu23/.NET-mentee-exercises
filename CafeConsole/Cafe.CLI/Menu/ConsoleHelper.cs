namespace Cafe.ConsoleUI.Utils;

public static class ConsoleHelper
{
    public static bool PromptYesNo(string message)
    {
        while (true)
        {
            Console.Write(message);
            var key = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

            if (key is "y" or "yes")
            {
                return true;
            }
            if (key is "n" or "no")
            {
                return false;
            }
            Console.WriteLine("Please answer y/n.");
        }
    }
}
