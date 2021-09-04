using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ChurchPresenter.UI.Models.Folder
{
    public class LyricFolder : IFolder
    {
        private readonly string title;
        private readonly Slide[] slides;
        private readonly string searchTitle;
        private readonly string searchLyrics;

        public LyricFolder(string title, string lyrics, string searchTitle, string searchLyrics)
        {
            this.title = title;
            slides = GenerateSlides(lyrics, title);
            this.searchTitle = searchTitle;
            this.searchLyrics = searchLyrics;
        }

        private static Slide[] GenerateSlides(string xmlLyrics, string title)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlLyrics);
            var songNode = FindFirstNodeWithName(doc, "song");
            var lyricNode = FindFirstNodeWithName(songNode, "lyrics");
            var lyricsList = new List<Slide>();

            foreach (XmlNode verseNode in lyricNode.ChildNodes)
                lyricsList.Add(new Slide(verseNode.FirstChild.InnerText, title));

            return lyricsList.ToArray();
        }

        private static XmlNode FindFirstNodeWithName(XmlNode doc, string name)
        {
            foreach(XmlNode node in doc.ChildNodes)
            {
                if (node.Name == name)
                    return node;
            }
            return null;
        }

        public string GetTitle()
        {
            return title;
        }

        public Slide[] GetSlides()
        {
            return slides;
        }

        public string GetSearchTitle()
        {
            return searchTitle;
        }
        public string GetSearchLyrics()
        {
            return searchLyrics;
        }

        public FolderType GetFolderType()
        {
            return FolderType.Lyric;
        }
    }

    public class LyricFolderBuilder
    {
        const string EMPTY_LYRICS = "<?xml version='1.0' encoding='UTF-8'?><song><lyrics></lyrics></song>";
        private string title;
        private string lyrics = EMPTY_LYRICS;
        private string searchLyrics;
        private string searchTitle;

        public LyricFolderBuilder WithTitle(string title)
        {
            this.title = title;
            return this;
        }
        public LyricFolderBuilder WithLyrics(string lyrics)
        {
            this.lyrics = lyrics;
            return this;
        }
        public LyricFolderBuilder WithSearchLyrics(string searchLyrics)
        {
            this.searchLyrics = searchLyrics;
            return this;
        }
        public LyricFolderBuilder WithSearchTitle(string searchTitle)
        {
            this.searchTitle = searchTitle;
            return this;
        }

        public LyricFolder Build()
        {
            return new LyricFolder(title, lyrics, searchTitle, searchLyrics);
        }
    }
}
