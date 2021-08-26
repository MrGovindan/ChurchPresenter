using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ChurchPresenter.UI.Presenters
{
    public interface IBibleLibraryView
    {
        event Action<string> SearchStringChanged;
        event Action<string, int> SearchStarted;
        event Action<int[]> AddedToService;
        event Action<int[]> ShownOnPreview;

        void ShowSuggestions(string[] suggestions);
        void ShowNoResults();
        void ShowScriptures(string[] scriptures);
        void SetBibleVersions(string[] versions);
    }

    class BibleLibraryPresenter
    {
        private static Regex searchPattern = new Regex(@"((\d?)\s?([a-z]+))\s?(\d+)(:(\d+)(-(\d+))?)?");

        private IBibleLibraryView view;
        private IBibleModel model;
        private IServiceModel serviceModel;
        private ISelectedFolderModel previewFolderModel;
        private ScriptureCollection scriptures;

        public BibleLibraryPresenter(
            IBibleLibraryView view,
            IBibleModel model,
            IServiceModel serviceModel,
            [KeyFilter("Preview")] ISelectedFolderModel previewFolderModel)
        {
            this.view = view;
            this.model = model;
            this.serviceModel = serviceModel;
            this.previewFolderModel = previewFolderModel;

            view.SearchStringChanged += HandleSearchStringChanged;
            view.SearchStarted += HandleSearchStarted;
            view.AddedToService += HandleAddedToService;
            view.ShownOnPreview += HandleShowOnPreview;
            model.BibleVersionsUpdated += view.SetBibleVersions;
        }

        private void HandleSearchStringChanged(string search)
        {
            if (search == "")
                return;

            search = search.ToLower();
            var results = BookHelper.FindMatchingBook(search).Select(b => b + " ");
            view.ShowSuggestions(results.ToArray());
        }


        private void HandleSearchStarted(string search, int versionIndex)
        {
            try
            {
                search = search.ToLower();
                var match = searchPattern.Match(search);

                var bookString = (match.Groups[2].Value + " " + match.Groups[3].Value).Trim();
                var book = (Book)BookHelper.FromString(BookHelper.FindMatchingBook(bookString).First());
                var chapter = int.Parse(match.Groups[4].Value);

                ScriptureCollection folder = null;

                if (match.Groups[8].Success)
                {
                    int start = int.Parse(match.Groups[6].Value);
                    int end = int.Parse(match.Groups[8].Value);
                    if (end >= start)
                        folder = model.GetVerses(versionIndex, new Verse(book, chapter, start), new Verse(book, chapter, end));
                    else
                        view.ShowNoResults();
                }
                else if (match.Groups[6].Success)
                {
                    int verse = int.Parse(match.Groups[6].Value);
                    folder = model.GetVerse(versionIndex, new Verse(book, chapter, verse));
                }
                else
                {
                    folder = model.GetWholeChapter(versionIndex, book, chapter);
                }

                if (folder != null)
                {
                    if (folder.verses.Length == 0)
                        view.ShowNoResults();
                    else
                        ShowScriptureList(folder);
                }
            }
            catch (Exception)
            {
                view.ShowNoResults();
            }
        }

        private void ShowScriptureList(ScriptureCollection scriptures)
        {
            this.scriptures = scriptures;
            var list = new List<string>();
            var book = BookHelper.ToString(scriptures.start.book);
            for (int i = scriptures.start.verse; i <= scriptures.end.verse; i++)
                list.Add(String.Format("{0} {1}:{2} ({3})", book, scriptures.start.chapter, i, scriptures.version));
            view.ShowScriptures(list.ToArray());
        }

        private void HandleShowOnPreview(int[] currentSelection)
        {
            GenerateFolderAndSendTo(currentSelection, folder => previewFolderModel.PublishSelectedFolder(folder));
        }

        private void HandleAddedToService(int[] currentSelection)
        {
            GenerateFolderAndSendTo(currentSelection, folder => serviceModel.AddFolder(folder));
        }

        private void GenerateFolderAndSendTo(int[] currentSelection, Action<IFolder> processFolder)
        {
            var folder = new ScriptureFolder();
            var verseSeriesDescription = GetVerseSeriesFromSelection(currentSelection);
            folder.title = string.Format("{0} {1}:{2} {3}", BookHelper.ToString(scriptures.start.book), scriptures.start.chapter, verseSeriesDescription, scriptures.version);
            var previousIndex = currentSelection[0];
            foreach (var i in currentSelection)
                folder.slides.Add(new ScriptureSlide(
                    scriptures.verses[i],
                    scriptures.version,
                    new Verse(scriptures.start.book, scriptures.start.chapter, scriptures.start.verse + i)));

            processFolder.Invoke(folder);
        }

        private string GetVerseSeriesFromSelection(int[] currentSelection)
        {
            var previousIndex = currentSelection[0];
            string result = (previousIndex + scriptures.start.verse).ToString();
            var lastSet = previousIndex;
            for (int i = 1; i < currentSelection.Length; ++i)
            {
                int currentIndex = currentSelection[i];
                if ((previousIndex + 1) != currentIndex)
                {
                    if (lastSet != previousIndex)
                        result += "-" + (previousIndex + scriptures.start.verse).ToString();

                    result += ", " + (currentIndex + scriptures.start.verse).ToString();
                    lastSet = currentIndex;
                }
                previousIndex = currentIndex;
            }
            if (previousIndex != lastSet)
                result += "-" + (previousIndex + scriptures.start.verse).ToString();

            return result;
        }

        private class ScriptureSlide : Slide
        {
            private string version;
            private Verse verse;

            internal ScriptureSlide(string text, string version, Verse verse) : base(text)
            {
                this.version = version;
                this.verse = verse;
                this.caption = string.Format("{0} {1}:{2} {3}",
                    BookHelper.ToString(verse.book), verse.chapter, verse.verse, version);
            }

            public override string ToString()
            {
                return string.Format("{0}:{1} {2}", verse.chapter, verse.verse, text);
            }

            public override string ToHtml()
            {
                return string.Format("<sup>{0}:{1}</sup> {2}", verse.chapter, verse.verse, text);
            }
        }

        private class ScriptureFolder : IFolder
        {
            internal string title = "";
            internal List<ScriptureSlide> slides = new List<ScriptureSlide>();

            public FolderType GetFolderType()
            {
                return FolderType.Scripture;
            }

            public Slide[] GetSlides()
            {
                return slides.ToArray();
            }

            public string GetTitle()
            {
                return title;
            }
        }
    }
}
