using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
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

    partial class BibleLibraryPresenter
    {
        private static readonly Regex searchPattern = new Regex(@"((\d?)\s?([a-z]+))\s?(\d+)(:(\d+)(-(\d+))?)?");

        private readonly IBibleLibraryView view;
        private readonly IBibleModel model;
        private readonly IServiceModel serviceModel;
        private readonly ISelectedFolderModel previewFolderModel;
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
                list.Add(string.Format("{0} {1}:{2} ({3})", book, scriptures.start.chapter, i, scriptures.version));
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
            foreach (var i in currentSelection)
            {
                var parts = new List<TextPart>
                {
                    TextPart.AsSuperscript(string.Format("{0}:{1}", scriptures.start.chapter, scriptures.start.verse + i)),
                    TextPart.AsNormal(scriptures.verses[i])
                };
                var caption = string.Format("{0} {1}:{2} {3}", BookHelper.ToString(scriptures.start.book), scriptures.start.chapter, scriptures.start.verse + i, scriptures.version);
                folder.slides.Add(new Slide(parts, caption));
            }

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
    }
}
