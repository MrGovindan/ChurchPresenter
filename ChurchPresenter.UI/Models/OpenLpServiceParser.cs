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
                var folder = new ScriptureFolder { title = header["title"].ToString() };

                var versePattern = new Regex(@"(\d+):(\d+)");
                var bookPattern = new Regex(@"((\d\s)?([a-z]+))");

                var slides = new List<Slide>();
                foreach (JToken slideData in serviceItem["data"])
                {
                    var raw = slideData["raw_slide"].ToString();
                    var lines = raw.Split("\n");

                    foreach (string l in lines)
                    {
                        var line = l.Trim();
                        if (line == "")
                            continue;

                        var end = line.IndexOf(";");
                        var verseText = line.Substring(0, end + 1);
                        var scripture = line[(end + 1)..];
                        scripture = scripture.Replace("\\\"", "\"");

                        var bookMatches = bookPattern.Match(header["title"].ToString().ToLower());
                        var possibleBooks = BookHelper.FindMatchingBook(bookMatches.Groups[1].Value).GetEnumerator();
                        possibleBooks.MoveNext();
                        var book = BookHelper.FromString(possibleBooks.Current);

                        var verseMatches = versePattern.Match(verseText);
                        if (book != null)
                        {
                            var verse = new Verse((Book)book, int.Parse(verseMatches.Groups[1].Value), int.Parse(verseMatches.Groups[2].Value));
                            var scriptureParts = new List<TextPart>
                            {
                                TextPart.AsSuperscript(string.Format("{0}:{1}", verse.chapter, verse.verse)),
                                TextPart.AsNormal(scripture)
                            };
                            slides.Add(new Slide(scriptureParts, ""));
                        }
                    }
                }
                folder.slides = slides;

                return folder;
            }
        }
    }
}
