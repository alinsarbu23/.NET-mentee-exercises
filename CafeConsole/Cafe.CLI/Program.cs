using Cafe.ConsoleUI.Menu;
using Cafe.Application.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var service = new OrderService();
        var app = new CafeMenu(service);
        app.Run();
    }
}
