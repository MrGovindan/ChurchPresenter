using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public class ScriptureCollection
    {
        public string version;
        public readonly Verse start;
        public readonly Verse end;
        public readonly string[] verses;

        public ScriptureCollection(string version, Verse start, Verse end, string[] verses)
        {
            this.version = version;
            this.start = start;
            this.end = end;
            this.verses = verses;
        }
    }

    public interface IBibleModel
    {
        event Action<string[]> BibleVersionsUpdated;

        ScriptureCollection GetWholeChapter(int versionIndex, Book book, int chapter);
        ScriptureCollection GetVerse(int versionIndex, Verse verse);
        ScriptureCollection GetVerses(int versionIndex, Verse startVerse, Verse endVerse);
    }
}
