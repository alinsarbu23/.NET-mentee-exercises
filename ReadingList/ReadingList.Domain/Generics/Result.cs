using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Domain.Generics
{
    public class Result<T>
    {
        public bool Ok { get; set; }
        public T? Value { get; set; } //the result if Ok == true
        public string? Error { get; set; }
    }
}
