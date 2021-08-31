using ChurchPresenter.UI.Models.Folder;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ChurchPresenter.UI.Models
{
    class OpenLpServiceParser
    {
        internal IFolder[] Parse(string serviceData)
        {
            var folders = new List<IFolder>();

            var root = JArray.Parse(serviceData);
            for (int i = 1; i < root.Count; ++i)
            {
                folders.Add(ParseServiceItem(root[i]["serviceitem"]));
            }

            return folders.ToArray();
        }

        private static IFolder ParseServiceItem(JToken serviceItem)
        {
            var header = serviceItem["header"];
            if (header["name"].ToString() == "songs")
            {
                return new LyricFolderBuilder()
                    .WithTitle(header["title"].ToString())
                    .WithLyrics(header["xml_version"].ToString().Replace("<br/>", "\n"))
                    .Build();
            }
            else
            {
                var folder = new ScriptureFolder();
                folder.title = header["title"].ToString();

                var versePattern = new Regex(@"(\d+):(\d+)");
                var bookPattern = new Regex(@"((\d\s)?([a-z]+))");

                var slideData = serviceItem["data"][0]["raw_slide"].ToString();
                var lines = slideData.Split("\n");

                var slides = new List<ScriptureSlide>();
                foreach (string l in lines)
                {
                    var line = l.Trim();
                    if (line == "")
                        continue;

                    var end = line.IndexOf(";");
                    var verseText = line.Substring(0, end + 1);
                    var scripture = line.Substring(end + 1);
                    scripture = scripture.Replace("\\\"", "\"");

                    var bookMatches = bookPattern.Match(header["title"].ToString().ToLower());
                    var possibleBooks = BookHelper.FindMatchingBook(bookMatches.Groups[1].Value).GetEnumerator();
                    possibleBooks.MoveNext();
                    var book = BookHelper.FromString(possibleBooks.Current);

                    var verseMatches = versePattern.Match(verseText);
                    if (book != null)
                    {
                        var verse = new Verse((Book)book, int.Parse(verseMatches.Groups[1].Value), int.Parse(verseMatches.Groups[2].Value));
                        slides.Add(new ScriptureSlide(scripture, header["footer"][1].ToString(), verse));
                    }
                }
                folder.slides = slides;

                return folder;
            }
        }
    }
}
