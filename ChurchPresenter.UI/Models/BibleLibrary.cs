using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    class BibleLibrary : IBibleModel
    {
        private readonly List<string> availableBibleVersions = new List<string>();

        private event Action<string[]> BibleVersionsUpdatedImpl;

        public event Action<string[]> BibleVersionsUpdated
        {
            add
            {
                BibleVersionsUpdatedImpl += value;
                value.Invoke(availableBibleVersions.ToArray());
            }
            remove
            {
                BibleVersionsUpdatedImpl -= value;
            }
        }

        private const string bibleDir = "/openlp/data/bibles/";
        private static readonly string userDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("\\", "/");

        public BibleLibrary()
        {
            foreach (var file in Directory.GetFiles(userDir + bibleDir))
            {
                var filename = file[(file.LastIndexOfAny(new char[] { '\\', '/' }) + 1)..];
                if (filename.Contains(".sqlite"))
                {
                    filename = filename.Substring(0, filename.IndexOf("."));
                    availableBibleVersions.Add(filename);
                }
            }

            BibleVersionsUpdatedImpl?.Invoke(availableBibleVersions.ToArray());
        }

        public ScriptureCollection GetVerse(int versionIndex, Verse verse)
        {
            var verses = new List<string>();
            var version = availableBibleVersions[versionIndex];
            var bookId = GetBookId(verse.book, version);
            using (var connection = new SqliteConnection("Data Source=" + userDir + bibleDir + version + ".sqlite"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM verse WHERE book_id == $book AND chapter == $chapter AND verse == $verse";
                command.Parameters.AddWithValue("$book", bookId);
                command.Parameters.AddWithValue("$chapter", verse.chapter);
                command.Parameters.AddWithValue("$verse", verse.verse);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    verses.Add(reader.GetString(4));
                }
            }

            return new ScriptureCollection(availableBibleVersions[versionIndex], verse, verse, verses.ToArray());
        }

        private int GetBookId(Book book, string version)
        {
            using (var connection = new SqliteConnection("Data Source=" + userDir + bibleDir + version + ".sqlite"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id FROM book WHERE book_reference_id == $book";
                command.Parameters.AddWithValue("$book", (int)book);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return reader.GetInt32(0);
                }
            }

            return -1;
        }

        public ScriptureCollection GetVerses(int versionIndex, Verse startVerse, Verse endVerse)
        {
            var verses = new List<string>();
            var version = availableBibleVersions[versionIndex];
            var bookId = GetBookId(startVerse.book, version);
            using (var connection = new SqliteConnection("Data Source=" + userDir + bibleDir + version + ".sqlite"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM verse WHERE book_id == $book AND chapter == $chapter AND verse >= $start_verse AND verse <= $end_verse";
                command.Parameters.AddWithValue("$book", bookId);
                command.Parameters.AddWithValue("$chapter", startVerse.chapter);
                command.Parameters.AddWithValue("$start_verse", startVerse.verse);
                command.Parameters.AddWithValue("$end_verse", endVerse.verse);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    verses.Add(reader.GetString(4));
                }
            }

            return new ScriptureCollection(availableBibleVersions[versionIndex], startVerse, endVerse, verses.ToArray());
        }

        public ScriptureCollection GetWholeChapter(int versionIndex, Book book, int chapter)
        {
            var verses = new List<string>();
            var lastVerse = 1;
            var version = availableBibleVersions[versionIndex];
            var bookId = GetBookId(book, version);
            using (var connection = new SqliteConnection("Data Source=" + userDir + bibleDir + version + ".sqlite"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM verse WHERE book_id == $book AND chapter == $chapter";
                command.Parameters.AddWithValue("$book", bookId);
                command.Parameters.AddWithValue("$chapter", chapter);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lastVerse = reader.GetInt32(3);
                    verses.Add(reader.GetString(4));
                }
            }

            var start = new Verse(book, chapter, 1);
            var end = new Verse(book, chapter, lastVerse);

            return new ScriptureCollection(availableBibleVersions[versionIndex], start, end, verses.ToArray());
        }
    }
}
