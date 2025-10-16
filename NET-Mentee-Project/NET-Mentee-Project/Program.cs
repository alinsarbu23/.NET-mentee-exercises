using NET_Mentee_Project.Exercises;
using System.IO;

namespace NETMenteeProject
{
    public class Program
    {
        public static void SolvingProblem1()
        {
            int age;
            string email;
            bool isStudent;
            string name;

            Console.WriteLine("Name: ");
            name = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Age:");
            age = int.Parse(Console.ReadLine() ?? string.Empty);

            Console.WriteLine("Email:");
            email = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Are you a student? (yes/no)");
            isStudent = (Console.ReadLine() ?? string.Empty).ToLower() == "yes";

            Contract contract = new Contract(name, age, email, isStudent);
            string result = contract.ToString();
            Console.WriteLine(result);
        }

        public static void Main(string[] args)
        {
            

        }
    }
}

