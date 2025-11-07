using ReadingList.Domain.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Application.Interfaces
{
    public interface IExportService
    {
        Task<Result<bool>> ExportToCSVAsync(string filepath);
        Task<Result<bool>> ExportToJSONAsync(string filepath);
    }
}
