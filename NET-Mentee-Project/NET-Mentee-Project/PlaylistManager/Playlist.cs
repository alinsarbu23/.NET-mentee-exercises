using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_Mentee_Project.PlaylistManager
{
    public class Playlist
    {
        public string Name { get; }
        private List<Song> _songs = new List<Song>();

        public Playlist(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name),"Playlist name cannot be null or empty.");
            }
            Name = name.Trim();
        }

        public void AddSong(Song song)
        {
            if(song == null)
            {
                throw new ArgumentNullException(nameof(song), "Song cannot be null.");
            }
            _songs.Add(song);
        }

        public void RemoveTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title), "Title cannot be null or empty.");
            }
            var songToRemove = _songs.FirstOrDefault(s => s.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if(songToRemove != null)
            {
                _songs.Remove(songToRemove);
            }
            else
            {
                throw new ArgumentException($"Song with title '{title}' not found in the playlist.", nameof(title));
            }
        }

        public IEnumerable<Song> FindByArtist(string artist)
        {
            if(string.IsNullOrEmpty(artist))
            {
                return Enumerable.Empty<Song>();
            }
            return _songs.Where(s => s.Artist.Equals(artist.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public TimeSpan TotalDuration()
        {
            var totalSeconds = _songs.Sum(s => s.DurationSeconds);
            return TimeSpan.FromSeconds(totalSeconds);
        }

        public Song GetLongestSong()
        {
            if(_songs.Count == 0)
            {
                throw new InvalidOperationException("The playlist is empty.");
            }
            return _songs.OrderByDescending(s => s.DurationSeconds).FirstOrDefault() 
                ?? throw new InvalidOperationException("The playlist is empty.");
        }

        public IEnumerable<Song> List()
        {
            return _songs.AsReadOnly();
        }

        public void End()
        {
            _songs.Clear();
        }
    }
}