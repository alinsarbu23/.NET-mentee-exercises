using NET_Mentee_Project.Exercises;
using NET_Mentee_Project.Library;
using NET_Mentee_Project.PlaylistManager;
using System.Globalization;
using System.IO;


namespace NETMenteeProject
{
    public class Program
    {
        public static void Problem1()
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
        public static void Problem2()
        {
            Console.Write("Insert the name of the playlist:");
            string name = Console.ReadLine() ?? "Default name";
            

            Playlist playlist = new Playlist(name);

            while (true)
            {
                var line = Console.ReadLine();

                if (line is null)
                {
                    break;
                }

                if (line.StartsWith("END", StringComparison.OrdinalIgnoreCase))
                {
                    playlist.End();
                    break;
                }

                if (line.StartsWith("ADD", StringComparison.OrdinalIgnoreCase))
                {
                    var parts = line.Substring(4);
                    var input = parts.Split(';');
                    if (input.Length == 3 && int.TryParse(input[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var secodns))
                    {
                        try
                        {
                            var song = new Song(input[0], input[1], secodns);
                            playlist.AddSong(song);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);


                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input; Song title, artist and duration required");
                    }


                }
                else if (line.StartsWith("REMOVE", StringComparison.OrdinalIgnoreCase))
                {
                    var title = line.Substring(7).Trim();
                    try
                    {
                        playlist.RemoveTitle(title);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else if (line.StartsWith("FIND", StringComparison.OrdinalIgnoreCase))
                {
                    var artist = line.Substring(11).Trim();
                    var songs = playlist.FindByArtist(artist);
                    foreach (var song in songs)
                    {
                        Console.WriteLine(song);
                    }
                }
                else if (line.StartsWith("TOTALDURATION", StringComparison.OrdinalIgnoreCase))
                {
                    var totalDuration = playlist.TotalDuration();
                    Console.WriteLine($"Total Duration: {totalDuration}");
                }
                else if (line.StartsWith("LONGEST", StringComparison.OrdinalIgnoreCase))
                {
                    var longestSong = playlist.GetLongestSong();
                    if (longestSong != null)
                    {
                        Console.WriteLine($"Longest Song: {longestSong}");
                    }
                    else
                    {
                        Console.WriteLine("The playlist is empty.");
                    }
                }
                else if (line.StartsWith("LIST", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("The songs:");
                    var songs = playlist.List();
                    foreach (var song in songs)
                    {
                        Console.WriteLine(song);
                    }
                }
                else
                {
                    Console.WriteLine("Unknown command. Available commands: ADD, REMOVE, FIND, TOTALDURATION, LONGEST, LIST, END");

                }
            }


        }

        public static void Problem3()
        {
            int year;
            string title, author, input, line;

            List<Book> books = new List<Book>();

            while(true)
            {
                PrintMenuProblem3();

                input = Console.ReadLine() ?? string.Empty;

                if(string.IsNullOrEmpty(input))
                {
                    throw new ArgumentNullException("Invalid input. Try again");
                }

                if(input.StartsWith("ADD", StringComparison.OrdinalIgnoreCase))
                {
                    line = input.Substring(4);
                    var context = line.Split(";", 3, StringSplitOptions.TrimEntries);
                    if(context.Length == 3 && int.TryParse(context[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out year))
                    {
                        try
                        {
                            Book book = new Book(context[0].Trim(), context[1].Trim(), year);
                            books.Add(book);
                            Console.WriteLine($"Book '{book.Title}' added successfully.");
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please provide title, author and year separated by semicolons.");
                    }
                }
                else if(input.StartsWith("BORROW", StringComparison.OrdinalIgnoreCase))
                {
                    line = input.Substring(7);
                    var context = line.Split(";");
                    if(context.Length == 1)
                    {
                        try
                        {
                            books.FirstOrDefault(b => b.Title.Equals(context[0])).Borrow();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Book ot found. Try again");
                    }
                }
                else if (input.StartsWith("RETURN", StringComparison.OrdinalIgnoreCase))
                {
                    line = input.Substring(7);
                    var context = line.Split(";");
                    if (context.Length == 1)
                    {
                        try
                        {
                            books.FirstOrDefault(b => b.Title.Equals(context[0])).Return();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Book ot found. Try again");
                    }
                }
                else if(input.StartsWith("FIND", StringComparison.OrdinalIgnoreCase))
                {
                    line = input.Substring(5);
                    var context = line.Split(";");

                    if(context.Length == 1)
                    {
                        var book = books.FirstOrDefault(b => b.Title.Contains(context[0]));
                        if (book != null) 
                        {
                            Console.WriteLine($"Book ID: {book.Id} Name: {book.Title} - {book.Author} ({book.Year}) - Available: {book.IsAvailable}");
                        }
                        else
                        {
                            Console.WriteLine($"Book with the title {context[0]} not found");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid book title. Please try again !");
                    }

                }
                else if(input.StartsWith("LIST", StringComparison.OrdinalIgnoreCase))
                {
                    foreach(var book in books)
                    {
                        Console.WriteLine($"Book ID: {book.Id} Name: {book.Title} - {book.Author} ({book.Year}) - Available: {book.IsAvailable}");
                    }
                }
                else if(input.StartsWith("END", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }


        }

        public static void PrintMenuProblem3()
        {
            Console.WriteLine("========== LIBRARY ==========");
            Console.WriteLine("Select an option from the menu");
            Console.WriteLine("1. ADD title;author;year");
            Console.WriteLine("2. Borrow title");
            Console.WriteLine("3. Find title");
            Console.WriteLine("4. Return title");
            Console.WriteLine("5. List");
            Console.WriteLine("6. End");
        }

        public static void Main(string[] args)
        {
            Problem3();

        }
    }
}

