using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ChurchPresenter.UI.Models
{
    public class Slide
    {
        public readonly string text;

        public Slide(string text)
        {
            this.text = text;
        }
    }

    public readonly struct Song
    {
        public readonly string title;
        public readonly string lyrics;
        public readonly Slide[] slides;
        public readonly string searchTitle;
        public readonly string searchLyrics;

        public Song(string title, string lyrics, string searchTitle, string searchLyrics)
        {
            this.title = title;
            this.lyrics = lyrics;
            this.slides = GenerateSlides(lyrics);
            this.searchTitle = searchTitle;
            this.searchLyrics = searchLyrics;
        }

        private static Slide[] GenerateSlides(string xmlLyrics)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlLyrics);
            var songNode = FindFirstNodeWithName(doc, "song");
            var lyricNode = FindFirstNodeWithName(songNode, "lyrics");
            var lyricsList = new List<Slide>();

            foreach (XmlNode verseNode in lyricNode.ChildNodes)
                lyricsList.Add(new Slide(verseNode.FirstChild.InnerText));

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
    }

    public class SongBuilder
    {
        const string EMPTY_LYRICS = "<?xml version='1.0' encoding='UTF-8'?><song><lyrics></lyrics></song>";
        private string title;
        private string lyrics = EMPTY_LYRICS;
        private string searchLyrics;
        private string searchTitle;

        public SongBuilder WithTitle(string title)
        {
            this.title = title;
            return this;
        }
        public SongBuilder WithLyrics(string lyrics)
        {
            this.lyrics = lyrics;
            return this;
        }
        public SongBuilder WithSearchLyrics(string searchLyrics)
        {
            this.searchLyrics = searchLyrics;
            return this;
        }
        public SongBuilder WithSearchTitle(string searchTitle)
        {
            this.searchTitle = searchTitle;
            return this;
        }

        public Song Build()
        {
            return new Song(title, lyrics, searchTitle, searchLyrics);
        }
    }
}
