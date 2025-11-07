using ReadingList.Application.Interfaces;
using ReadingList.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReadingList.Infrastructure.Repositories
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly Dictionary<int, Book> _store = new();

        public bool Add(Book book)
        {
            if (book == null) return false;
            if (_store.ContainsKey(book.Id)) return false;
            _store[book.Id] = Clone(book);
            return true;
        }

        public bool Upsert(Book book)
        {
            if (book == null) return false;
            _store[book.Id] = Clone(book);
            return true;
        }

        public bool TryGet(int id, out Book? book)
        {
            if (_store.TryGetValue(id, out var found))
            {
                book = Clone(found);
                return true;
            }
            book = null;
            return false;
        }

        public IEnumerable<Book> GetAll()
        {
            var list = new List<Book>();
            foreach (var item in _store.Values)
            {
                list.Add(Clone(item));
            }
            return list;
        }

        private static Book Clone(Book b)
        {
            return new Book
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Year = b.Year,
                Pages = b.Pages,
                Genre = b.Genre,
                Finished = b.Finished,
                Rating = b.Rating
            };
        }
    }
}
