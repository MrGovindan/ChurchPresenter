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
        List<LyricFolder> currentLibrary;
        
        public SongLibrary()
        {
            currentLibrary = new List<LyricFolder>();
            var userDir = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("\\", "/");
            using (var connection = new SqliteConnection("Data Source=" + userDir + "/openlp/data/songs/songs.sqlite"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * from songs";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        currentLibrary.Add(new LyricFolder(reader.GetString(1), reader.GetString(3), reader.GetString(9), reader.GetString(10)));
                }
            }

            currentLibrary.Sort((a, b) => a.GetTitle().CompareTo(b.GetTitle()));
        }

        public Task<IList<LyricFolder>> GetAllSongs()
        {
            IList<LyricFolder> result = currentLibrary;
            return Task.FromResult(result);
        }

        public Task<IList<LyricFolder>> GetMatchingSongs(string pattern)
        {
            Predicate<LyricFolder> match = s => s.GetSearchTitle().Contains(pattern.ToLower()) | s.GetSearchLyrics().Contains(pattern.ToLower());
            var result = new List<LyricFolder>(currentLibrary.FindAll(match));
            return Task.FromResult((IList<LyricFolder>)result);
        }
    }
}
