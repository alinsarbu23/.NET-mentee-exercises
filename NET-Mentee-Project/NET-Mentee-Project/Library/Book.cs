using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_Mentee_Project.Library
{
    public class Book
    {
        public Guid Id { get; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; } = true;

        public Book(string title, string author, int year)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("Title can not be empty", nameof(title));
            }

            if (string.IsNullOrEmpty(author))
            {
                throw new ArgumentNullException("The author must have a name", nameof(title));
            }

            if (year < 1450 || year > 2100)
            {
                throw new ArgumentOutOfRangeException(nameof(year), "The year is not valid");
            }

            Id = Guid.NewGuid();
            Title = title;
            Year = year;
        }

        public void Borrow()
        {
            if (IsAvailable)
            {
                IsAvailable = false;
            }
            else
            {
                throw new InvalidOperationException("The book is not available");

            }
        }

        public void Return()
        {
            if (IsAvailable == false)
            {
                IsAvailable = true;
            }
            else
            {
                throw new InvalidOperationException("The book is already in the library");
            }
        }
    }
}
