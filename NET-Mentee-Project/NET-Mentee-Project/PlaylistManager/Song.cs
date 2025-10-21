using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_Mentee_Project.PlaylistManager
{
    public class Song
    {
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public int DurationSeconds { get; set; }

        public Song(string title, string artist, int durationSeconds)
        {
            if(string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException(nameof(title), "Title cannot be null or empty.");
            }
            if(string.IsNullOrWhiteSpace(artist))
            {
                throw new ArgumentNullException(nameof(artist), "Artist cannot be null or empty.");
            }
            if (durationSeconds < 1)
            {
                throw new ArgumentException("Duration must be a positive integer.", nameof(durationSeconds));
            }

            Title = title.Trim();
            Artist = artist.Trim();
            DurationSeconds = durationSeconds;
        }

        public override string ToString()
        {
            TimeSpan duration = TimeSpan.FromSeconds(DurationSeconds);
            return $"{Title} by {Artist}, Duration: {duration:mm\\:ss}";
        }
    }
}
