using NET_Mentee_Project.Exercises;
using NET_Mentee_Project.PlaylistManager;
using System.Globalization;
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
        public static void SolvingProblem2()
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

        public static void Main(string[] args)
        {
            SolvingProblem2();

        }
    }
}

