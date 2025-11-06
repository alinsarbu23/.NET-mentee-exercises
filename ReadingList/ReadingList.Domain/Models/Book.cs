using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Pages { get; set; }
        public string Genre { get; set; } = string.Empty;
        public bool Finished { get; set; }
        public double Rating { get; set; }
    }
}
