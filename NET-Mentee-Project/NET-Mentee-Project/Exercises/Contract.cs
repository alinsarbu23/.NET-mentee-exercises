using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_Mentee_Project.Exercises
{
    public class Contract
    {
        public required string Name { get; set; }
        public int Age { get; set; }
        public string? Email { get; set; } = string.Empty;
        public bool IsStudent { get; set; }

        [SetsRequiredMembers]
        public Contract(string name, int age, string email, bool isStudent)
        {
            Name = name;
            if (Name.Length < 2 || string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException("Name must be at least 2 characters long");
            }

            Age = age;
            if(Age < 0 || Age > 120)
            {
                throw new ArgumentOutOfRangeException($"{Age} must be between 0 and 120");
            }

            Email = email;
            if (!Email.Contains('@') && string.IsNullOrEmpty(Email))
            {
                throw new ArgumentException($"{Email} must contain the character \'@\'");
            }

            IsStudent = isStudent;

        }

        public override string ToString()
        {
            if(string.IsNullOrWhiteSpace(Email))
            {
                return $"{Name} ({Age}) - student = {IsStudent} - email: n/a";
            }
            else
            {
                return $"{Name} ({Age}) - student = {IsStudent} - email: {Email}";
            }
        }
    }
}
