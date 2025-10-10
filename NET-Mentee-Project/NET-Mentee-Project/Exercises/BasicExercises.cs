using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_Mentee_Project.Exercises
{
    public class BasicExercises
    {


        public BasicExercises()
        {

        }



        public static void Problem1()
        {
            Console.Write("Read a name: ");
            string name = Console.ReadLine();

            Console.Write("Read the age: ");
            int age = int.Parse(Console.ReadLine());

            bool isStudent = false;

            if (age <= 0)
            {
                age = 0;
            }

            string answer = string.Empty;

            Console.Write("Are you a student ? (Y/N): ");
            answer = Console.ReadLine();

            while (answer.ToLower() != "y"  && answer.ToLower() != "n")
            {
                Console.Write("Are you a student ? (Y/N): ");
                answer = Console.ReadLine();

            }


            if (answer.ToLower() == "y")
            {
                isStudent = true;
            }

            else if (answer.ToLower() == "n")
            {
                isStudent = false;
            }



            Console.WriteLine($"Hello, my name is {name}, I am {age} years old. Am I student ? The answer is {isStudent}");
        }

        public static void Problem2()
        {
            double n1, n2;

            Console.Write("Read the first number:");
            while(!double.TryParse(Console.ReadLine(), out n1))
            {
                Console.Write("Invalid number. Write again !");
            }

            Console.Write("Read the second number:");
            while (!double.TryParse(Console.ReadLine(), out n2))
            {
                Console.Write("Invalid number. Write again !");
            }

            Console.WriteLine($"The sum of n1 & n2: {n1+n2}");
            Console.WriteLine($"The difference of n1 & n2: {n1 - n2}");
            Console.WriteLine($"The product of n1 & n2: {n1 * n2}");

            if(n2 == 0)
            {
                Console.WriteLine("N/A. Divide by zero");
            }

            else 
            {
                Console.WriteLine($"The quot of n1 & n2: {n1 / n2}");
                Console.WriteLine($"The module of n1 & n2: {n1 % n2}");
            }

        }

        public static void Problem3()
        {
            int number;

            Console.Write("Read the number: ");
            while(!int.TryParse(Console.ReadLine(), out number) || number == 0)
            {
                Console.Write("Invalid number, try again! n: ");
            }

            if (number % 2 == 0) 
            {
                Console.Write($"Odd, ");
            }

            else if(number % 2 != 0)
            {
                Console.Write($"Even, ");
            }

            if (number % 3 == 0 || number % 5 == 0) 
            {
                if(number % 3 == 0 && number % 5 == 0)
                {
                    Console.Write("multiple of 3 and 5, ");
                }

                else if (number % 3 == 0)
                {
                    Console.Write($"multiply of 3, ");
                }

                else if (number % 5 == 0)
                {
                    Console.Write($"multiply of 5, ");
                }
            }

            if(number < 0)
            {
                Console.Write("negative");
            }
            else
            {
                Console.WriteLine("positive");
            }
        }

        public static void Problem4()
        {
            int grade;

            Console.Write("Read the grade: ");
            while(!int.TryParse(Console.ReadLine(),out grade) || grade > 10 || grade < 1)
            {
                Console.Write("Invalid degree. Try again: ");
            }

            switch (grade)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    Console.WriteLine("Fail");
                    break;

                case 6:
                    Console.WriteLine("Enough");
                    break;
                case 7:
                case 8:
                    Console.WriteLine("Good");
                    break;
                case 9:
                    Console.WriteLine("Awesome!");
                    Console.Write("Do you have bonus ? ");
                    string bonus = Console.ReadLine();

                    while(string.IsNullOrEmpty(bonus) && (bonus != "yes" || bonus != "no"))
                    {
                        Console.Write("Invalid answer, try again. Do you have a bonus ? ");
                        bonus = Console.ReadLine();
                    }

                    Console.WriteLine($"Bonus (yes/no): {bonus}");
                    break;
                case 10:
                    Console.WriteLine("Excelent!");
                    Console.Write("Do you have bonus ? ");
                    bonus = Console.ReadLine();

                    while (string.IsNullOrEmpty(bonus) || bonus != "yes" || bonus != "no")
                    {
                        Console.Write("Invalid answer, try again. Do you have a bonus ? ");
                    }

                    Console.WriteLine($"Bonus (yes/no): {bonus}");
                    break;
            }
            


        }

        public static void Problem5()
        {
            string workday = "Workday";
            string weekend = "Weekend";

            List<string> days = new List<string>() 
            {
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
                "Sunday"
            };

            Console.Write("Read a number");
            int n;

            while(!int.TryParse(Console.ReadLine(), out n) || n <= 0 || n > 7)
            {
                Console.Write("Invalid number ! Try again: ");
            }

            switch (n)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    Console.WriteLine($"{days[n - 1]} - {workday}");
                    break;

                case 6:
                case 7:
                    Console.WriteLine($"{days[n - 1]} - {weekend}");
                    break;
            }

        }

        public static bool isDigit3(int n)
        {
            int lastDigit;
            while (n != 0)
            {
                lastDigit = n % 10;
                n /= 10;

                if( lastDigit == 3)
                {
                    return true;
                }
            }
            return false;
        }

        public static void Problem6()
        {
            int n;
            bool isDigit;

            while (!int.TryParse(Console.ReadLine(), out n) || n < 1 || n > 1000)
            {
                Console.Write("Invalid number ! Try again: ");
            }

            for (int i = 1; i <= n; i++)
            {
                if(i % 3 == 0 || i % 5 == 0)
                {
                    if (i % 3 == 0)
                    {
                        Console.Write("Fizz");
                    }

                    if (i % 5 == 0)
                    {
                        Console.Write("Buzz");
                    }

                    if (isDigit3(i))
                    {
                        Console.Write("*");
                    }
                }

                else
                {
                    Console.Write($"{i}");
                    if (isDigit3(i))
                    {
                        Console.Write("*");
                    }
                }

                Console.WriteLine();

            }
        }

        public static int NumberDigits(int n)
        {
            int size = 0;
            while(n !=0)
            {
                size++;
                n /= 10;
            }
            return size;
        }

        public static int SumOfDigits(int n)
        {
            int lastDigit = 0;
            int sum = 0;
            while (n != 0)
            {
                lastDigit = n % 10;
                sum += lastDigit;
                n /= 10;
            }
            return sum;
        }

        public static void Problem7()
        {
            int n;

            while (!int.TryParse(Console.ReadLine(), out n))
            {
                Console.Write("Invalid number ! Try again: ");
            }

            n = Math.Abs(n);
            int sum = SumOfDigits(n);
            string parity;

            if(sum %2 == 0)
            {
                parity = "even";
            }
            else
            {
                parity = "odd";
            }

                Console.WriteLine($"Sum = {sum}, Digits = {NumberDigits(n)}, SumParity = {parity}");
        }

        public static void Problem8()
        {
            int n;

            Console.Write("Read the size of the n: ");
            
            while(!int.TryParse(Console.ReadLine(), out n) || n < 3 || n > 15)
            {
                Console.Write("Invalid number, try again ! ");
            }

            int[] list = new int[n];

            Console.Write($"Read the array ({n} elements):");
            for (int i = 0; i < list.Length; i++)
            {
                while(! int.TryParse(Console.ReadLine(),out list[i]))
                {
                    Console.WriteLine("Invalid number. Try again!");
                }
            }

            Console.Write("The array elements: ");
            for (int i = 0; i < list.Length; i++)
            {
                Console.Write($"{list[i]} ");
            }
            Console.WriteLine();

            var sortedList = list.Distinct().OrderBy(x => x).ToArray();

            int sizeSortedList = sortedList.Length;
            double median;

            if(sizeSortedList % 2 != 0)
            {
                median = sortedList[sizeSortedList / 2];
            }
            else
            {
                int mid = sizeSortedList / 2;
                median = (sortedList[mid - 1] + sortedList[mid + 1]) / 2.00;
            }

            Console.Write($"Distinct sorted: ");

            foreach(var item in sortedList)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine($"Min : {sortedList.Min()}, Max: {sortedList.Max()}, Median: {median}");
            
        }

        public static void Problem9()
        {
            int k = 7;
            int option, diff, result;

            Random rand = new Random();
            result = rand.Next(1, 101);


            Console.WriteLine($"Game begins! {k} tries");


            while(k > 0)
            {
                Console.Write("Guess the number: ");

                while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 100)
                {
                    Console.WriteLine("Invalid number. Write again: ");
                }

                diff = Math.Abs(option - result);
                
                if(option == result)
                {
                    Console.WriteLine($"Congratulations ! The number was {result}. You guessed it from {7-k} tries");
                    break;
                }
                else
                {
                    k--;
                    if (option > result)
                    {
                        if(diff > 50)
                        {
                            Console.WriteLine("Higher, diff too high. Try again");
                        }
                        else
                        {
                            Console.WriteLine("Higher, diff smaller. Try again");
                        }
                       
                    }
                    else if (option < result)
                    {
                        if (diff > 50)
                        {
                            Console.WriteLine("Low, diff too high. Try again");
                        }
                        else
                        {
                            Console.WriteLine("Low, diff smaller. Try again");
                        }
                        Console.WriteLine("Too low. Try again");
                    }
                }
            }
            Console.WriteLine($"💀 You failed. The number was {result}");


        }

        public static void Problem10()
        {

        }


        public static void RunAllProblems()
        {
            //Problem1();
            //Problem2();
            //Problem3();
            //Problem4();
            //Problem5();
            //Problem6();
            //Problem7();
            //Problem8();
            //Problem9();
            //Problem10();
        }
    }
}
