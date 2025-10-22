using System;
using System.Collections.Generic;

namespace MiniBank.Services
{
    public class AccountLog
    {
        private readonly List<string> _records = new();

        public void AddMessage(string message)
        {
            var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] --> {message}";
            _records.Add(line);
        }

        public IReadOnlyList<string> GetRecords()
        {
            return _records.AsReadOnly();
        }

        public void RestoreRecords(IEnumerable<string> records)
        {
            _records.Clear();
            _records.AddRange(records);
        }

        public void PrintRecords()
        {
            foreach (var record in _records)
                Console.WriteLine(record);
        }
    }
}
