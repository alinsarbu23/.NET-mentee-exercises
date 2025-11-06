using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string TitleCase(this string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return string.Empty;
            }

            title = title.Trim();

            if (title.Length == 1)
            {
                return title.ToUpper();
            }

            return char.ToUpper(title[0]) + title.Substring(1).ToLower();
        }


        public static bool? ConvertAnswerToBool(this string? answer)
        {
            if(string.IsNullOrWhiteSpace(answer))
            {
                return null;
            }

            var value = answer.Trim().ToLower();

            if(value is "yes" or "true" or "y")
            {
                return true;
            }
            else if (value is "no" or "false" or "n")
            {
                return false;
            }
            return null;
        }

        public static double? ConvertToDouble(this string? input)
        {
            if(string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            double number;

            if(double.TryParse(input, out number))
            {
                return number;
            }

            return null;
        }
    }
}
