using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Application.Interfaces;
using ReadingList.Domain;
using ReadingList.Domain.Models;

namespace ReadingList.Application.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IEnumerable<Book> ListAll()
        {
            return _bookRepository.GetAll().OrderBy(x => x.Id);
        }

        public IEnumerable<Book> FilterFinished()
        {
            return _bookRepository.GetAll().Where(x => x.Finished).OrderByDescending(x=> x.Rating);
        }

        public IEnumerable<Book> TopRating(int n)
        {
            return _bookRepository.GetAll().OrderByDescending(x => x.Rating)
                .ThenBy(x => x.Title)
                .Take(Math.Max(0, n));
        }

        public IEnumerable<Book> FilterAuthor(string author)
        {
            if(string.IsNullOrWhiteSpace(author))
            {
                return Enumerable.Empty<Book>();
            }

            return _bookRepository.GetAll().Where(x => x.Author.Contains(author, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.Author)
                .ThenBy(x => x.Title); 
        }

        public bool MarkFinished(int id)
        {
            if(_bookRepository.TryGet(id, out Book? book) || book == null)
            {
                return false;
            }

            book.Finished = true;
            return _bookRepository.Upsert(book);
        }

        public bool Rate(int id, double rating)
        {
            if (rating < 0 || rating > 5) 
            {
                return false;
            }

            if(_bookRepository.TryGet(id, out Book? book) || book == null)
            {
                return false;
            }

            book.Rating = rating;
            return _bookRepository.Upsert(book);
        }

        public (int Total, int Finished, double Average) GetStats()
        {
            var books = _bookRepository.GetAll().ToList();

            int total = books.Count;
            int finished = books.Count(b => b.Finished);
            double average = total == 0 ? 0 : Math.Round(books.Average(b => b.Rating), 2);

            return (total, finished, average);
        }


    }
}
