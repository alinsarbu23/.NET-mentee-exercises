using ReadingList.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Application.Interfaces
{
    public interface IBookRepository
    {
        bool Add(Book book);
        bool Upsert(Book book);
        bool TryGet(int id, out Book? book);
        IEnumerable<Book> GetAll();
    }
}
