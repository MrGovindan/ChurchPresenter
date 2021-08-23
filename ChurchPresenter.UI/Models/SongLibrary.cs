using ChurchPresenter.UI.Presenters;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChurchPresenter.UI.Models
{
    public class SongLibrary : ISongLibrary
    {
        List<Song> currentLibrary;
        
        public SongLibrary()
        {
            currentLibrary = new List<Song>();
            var userDir = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("\\", "/");
            using (var connection = new SqliteConnection("Data Source=" + userDir + "/openlp/data/songs/songs.sqlite"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * from songs";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        currentLibrary.Add(new Song(reader.GetString(1), reader.GetString(3), reader.GetString(9), reader.GetString(10)));
                }
            }
        }

        public Task<IList<Song>> GetAllSongs()
        {
            IList<Song> result = currentLibrary;
            return Task.FromResult(result);
        }

        public Task<IList<Song>> GetMatchingSongs(string pattern)
        {
            Predicate<Song> match = s => s.searchTitle.Contains(pattern.ToLower()) | s.searchLyrics.Contains(pattern.ToLower());
            var result = new List<Song>(currentLibrary.FindAll(match));
            return Task.FromResult((IList<Song>)result);
        }
    }
}
