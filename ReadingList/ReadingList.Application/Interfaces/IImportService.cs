using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Application.Interfaces
{
    public interface IImportService
    {
        Task<(int imported, int duplicates, int malformed)> ImportAsync(
            IEnumerable<string> paths, Action<string>? Warning = null);

    }
}
