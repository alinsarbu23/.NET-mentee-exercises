using System.Globalization;
using ReadingList.Application.Interfaces;
using ReadingList.Application.Services;

namespace ReadingList.CLI.Menus
{
    public class Menu
    {
        private readonly BookService _bookService;
        private readonly IImportService _importService;
        private readonly IExportService _exportService;

        public Menu(BookService bookService, IImportService importService, IExportService exportService)
        {
            _bookService = bookService;
            _importService = importService;
            _exportService = exportService;
        }

        public async Task RunAsync()
        {
            PrintHelp();

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var cmd = parts[0].ToLowerInvariant();

                if (cmd == "exit" || cmd == "quit") break;

                switch (cmd)
                {
                    case "help": PrintHelp(); break;
                    case "import": await HandleImport(parts); break;
                    case "list": HandleList(parts); break;
                    case "finished": HandleFinished(); break;
                    case "top": HandleTop(parts); break;
                    case "by": HandleBy(parts); break;
                    case "mark": HandleMark(parts); break;
                    case "rate": HandleRate(parts); break;
                    case "stats": HandleStats(); break;
                    case "export": await HandleExport(parts); break;
                    default: Console.WriteLine("Unknown command. Type 'help' for options."); break;
                }
            }
        }

        private async Task HandleImport(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("usage: import <file1.csv> [file2.csv ...]");
                return;
            }

            var files = parts.Skip(1).ToArray();
            var (imp, dup, bad) = await _importService.ImportAsync(files, w => Console.WriteLine("WARN: " + w));
            Console.WriteLine($"Imported: {imp}, Duplicates: {dup}, Malformed: {bad}");
        }

        private void HandleList(string[] parts)
        {
            if (parts.Length == 1 || (parts.Length == 2 && parts[1].ToLowerInvariant() == "all"))
            {
                foreach (var b in _bookService.ListAll())
                    Console.WriteLine($"{b.Id,3} | {b.Title} | {b.Author} | {b.Year} | {(b.Finished ? "✓" : " ")} | {b.Rating:0.##}");
            }
            else
            {
                Console.WriteLine("usage: list all");
            }
        }

        private void HandleFinished()
        {
            foreach (var b in _bookService.FilterFinished())
                Console.WriteLine($"{b.Id,3} | {b.Title} | {b.Author} | {b.Year} | ✓ | {b.Rating:0.##}");
        }

        private void HandleTop(string[] parts)
        {
            if (parts.Length < 2 || !int.TryParse(parts[1], out var n))
            {
                Console.WriteLine("usage: top <n>");
                return;
            }

            foreach (var b in _bookService.TopRating(n))
                Console.WriteLine($"{b.Id,3} | {b.Title} | {b.Author} | {b.Year} | {(b.Finished ? "✓" : " ")} | {b.Rating:0.##}");
        }

        private void HandleBy(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("usage: by <author text>");
                return;
            }

            var text = string.Join(' ', parts.Skip(1));
            foreach (var b in _bookService.FilterAuthor(text))
                Console.WriteLine($"{b.Id,3} | {b.Title} | {b.Author} | {b.Year} | {(b.Finished ? "✓" : " ")} | {b.Rating:0.##}");
        }

        private void HandleMark(string[] parts)
        {
            if (parts.Length == 3 && parts[1].ToLowerInvariant() == "finished" && int.TryParse(parts[2], out var id))
            {
                Console.WriteLine(_bookService.MarkFinished(id) ? "200 OK" : "404 Not Found");
            }
            else
            {
                Console.WriteLine("usage: mark finished <id>");
            }
        }

        private void HandleRate(string[] parts)
        {
            if (parts.Length == 3 &&
                int.TryParse(parts[1], out var id) &&
                double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var rating))
            {
                Console.WriteLine(_bookService.Rate(id, rating) ? "200 OK" : "400 Bad Request / 404");
            }
            else
            {
                Console.WriteLine("usage: rate <id> <0-5>");
            }
        }

        private void HandleStats()
        {
            var (total, fin, avg) = _bookService.GetStats();
            Console.WriteLine($"Total: {total}");
            Console.WriteLine($"Finished: {fin}");
            Console.WriteLine($"Average rating: {avg:0.00}");
        }

        private async Task HandleExport(string[] parts)
        {
            if (parts.Length < 3)
            {
                Console.WriteLine("usage: export json <path> | export csv <path>");
                return;
            }

            var type = parts[1].ToLowerInvariant();
            var path = string.Join(' ', parts.Skip(2));

            if (File.Exists(path))
            {
                Console.Write($"File exists. Overwrite {path}? (y/n): ");
                var ans = Console.ReadLine();
                if (!string.Equals(ans, "y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Canceled.");
                    return;
                }
            }

            var result = type switch
            {
                "json" => await _exportService.ExportToJSONAsync(path),
                "csv" => await _exportService.ExportToCSVAsync(path),
                _ => new ReadingList.Domain.Generics.Result<bool> { Ok = false, Error = "unknown type" }
            };

            Console.WriteLine(result.Ok ? "Saved." : $"ERROR: {result.Error}");
        }

        private static void PrintHelp()
        {
            Console.WriteLine("""
                Commands:
                  import <file1.csv> [file2.csv ...]
                  list all
                  finished
                  top <n>
                  by <author text>
                  mark finished <id>
                  rate <id> <0-5>
                  stats
                  export json <path> | export csv <path>
                  help
                  exit
                """);
        }
    }
}
