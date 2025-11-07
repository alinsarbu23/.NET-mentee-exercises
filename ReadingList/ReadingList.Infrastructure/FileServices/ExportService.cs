using ReadingList.Application.Interfaces;
using ReadingList.Domain.Generics;
using System.Text;
using System.Text.Json;

namespace ReadingList.Infrastructure.FileServices
{
    public class ExportService: IExportService
    {
        private readonly IBookRepository _bookRepository;

        public ExportService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Result<bool>> ExportToJSONAsync(string filepath)
        {
            try
            {
                var books = _bookRepository.GetAll().ToArray();

                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(books, jsonOptions);
                await File.WriteAllTextAsync(filepath, json, Encoding.UTF8);

                return new Result<bool> { Ok = true, Value = true };
            }
            catch (Exception ex)
            {
                return new Result<bool> { Ok = false, Error = ex.Message };
            }
        }

        public async Task<Result<bool>> ExportToCSVAsync(string filepath)
        {
            try
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine("Id,Title,Author,Year,Pages,Genre,Finished,Rating");

                foreach (var b in _bookRepository.GetAll())
                {
                    stringBuilder.Append(b.Id).Append(',')
                      .Append(Quote(b.Title)).Append(',')
                      .Append(Quote(b.Author)).Append(',')
                      .Append(b.Year).Append(',')
                      .Append(b.Pages).Append(',')
                      .Append(Quote(b.Genre)).Append(',')
                      .Append(b.Finished ? "yes" : "no").Append(',')
                      .Append(b.Rating.ToString(System.Globalization.CultureInfo.InvariantCulture))
                      .AppendLine();
                }

                await File.WriteAllTextAsync(filepath, stringBuilder.ToString(), Encoding.UTF8);

                return new Result<bool> { Ok = true, Value = true };
            }
            catch (Exception ex)
            {
                return new Result<bool> { Ok = false, Error = ex.Message };
            }
        }

        private static string Quote(string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "\"\"";
            }

            var needsQuotes = text.Contains(',') || text.Contains('"') || text.Contains('\n') || text.Contains('\r');
            var escaped = text.Replace("\"", "\"\"");

            return needsQuotes ? $"\"{escaped}\"" : escaped;
        }
    }
}
