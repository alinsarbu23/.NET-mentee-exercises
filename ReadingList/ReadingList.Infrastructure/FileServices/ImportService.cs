using ReadingList.Application.Interfaces;
using ReadingList.Domain.Models;
using System.Globalization;
using System.IO;

namespace ReadingList.Infrastructure.FileServices
{
    public class ImportService : IImportService
    {
        private readonly IBookRepository _bookRepository;
        public ImportService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<(int imported, int duplicates, int malformed)> ImportAsync(
            IEnumerable<string> paths, Action<string>? Warning = null)
        {
            int imported = 0;
            int duplicates = 0;
            int malformed = 0;

            if (paths == null)
            {
                return (0, 0, 0);
            }

            foreach (string path in paths)
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    Warning?.Invoke($"File not found at: {path}");
                    continue;
                }

                string[] lines;
                try
                {
                    lines = await File.ReadAllLinesAsync(path);
                }
                catch
                {
                    Warning?.Invoke($"Can not read from {path}");
                    continue;
                }

                if(lines.Length == 0)
                {
                    continue;
                }

                int startLineIndex = FindHeader(lines[0]) ? 1 : 0;

                for (int i = startLineIndex; i < lines.Length; i++) 
                {
                    var line = lines[i];

                    if (!TryParseBook(line, out var book, out var error))
                    {
                        malformed++;
                        Warning?.Invoke($"{Path.GetFileName(path)} line {i + 1}: {error}");
                        continue;
                    }

                    if (_bookRepository.Add(book!))
                    {
                        imported++;
                    }
                    else
                    {
                        duplicates++;
                    }

                }
            }

            return (imported, duplicates, malformed);
        }
        
        private static bool FindHeader(string line)
        {
            if(string.IsNullOrWhiteSpace(line))
            {
                return false;
            }
            var text = line.ToLowerInvariant();

            return text.Contains("id") && text.Contains("title") && text.Contains("author");
        }

        private static bool TryParseBook(string line, out Book? book, out string error)
        {
            book = null;
            error = string.Empty;

            var parts = SplitCSV(line);

            if(parts.Count < 8)
            {
                error = "Not enough columns (book required 8 columns)";
                return false;
            }

            if(!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture ,out var id))
            {
                error = "Could not find a valid id";
                return false;
            }

            var title = (parts[1] ?? string.Empty).Trim();
            var author = (parts[2] ?? string.Empty).Trim();
            var genre = (parts[5] ?? string.Empty).Trim();

            if (!int.TryParse(parts[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out int year))
            {
                error = "Invalid year";
                return false;
            }

            if(!int.TryParse(parts[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out var pages))
            {
                error = "invalid Pages";
                return false;
            }

            if (string.IsNullOrWhiteSpace(parts[6]))
            {
                parts[6] = null;
            }

            var finished = (parts[6] ?? string.Empty).Trim().ToLowerInvariant();
            bool isFinished = finished is "yes" or "true" or "y" or "1";

            if (finished == "yes" || finished == "true" || finished == "y" || finished == "1")
            {
                isFinished = true;
            }

            if (!double.TryParse(parts[7], NumberStyles.Float, CultureInfo.InvariantCulture, out double rating))
            {
                error = "Invalid rating";
                return false;
            }

            if(rating < 0 || rating > 5)
            {
                error = $"Invalid value for rating: {rating}";
                return false;
            }

            book = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                Year = year,
                Pages = pages,
                Genre = genre,
                Finished = isFinished,
                Rating = rating
            };

            return true;

        }

        private static List<string> SplitCSV(string line)
        {
            var fields = new List<string>();

            if(line == null)
            {
                return fields;
            }

            bool insideQuotes = false;
            string currentField = "";

            foreach(char character in line)
            {
                if(character == '"')
                {
                    insideQuotes = !insideQuotes;
                }
                else if(character == ',' && !insideQuotes)
                {
                    fields.Add(currentField);
                    currentField = "";
                }
                else
                {
                    currentField += character;
                }
            }

            fields.Add(currentField);
            return fields;

        }
    }
}
