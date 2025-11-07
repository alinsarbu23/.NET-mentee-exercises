using ReadingList.Application.Services;
using ReadingList.CLI.Menus;
using ReadingList.Infrastructure.FileServices;
using ReadingList.Infrastructure.Repositories;

namespace ReadingList.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var repo = new InMemoryBookRepository();
            var bookService = new BookService(repo);
            var importService = new ImportService(repo);
            var exportService = new ExportService(repo);

            var menu = new Menu(bookService, importService, exportService);
            await menu.RunAsync();
        }
    }
}
