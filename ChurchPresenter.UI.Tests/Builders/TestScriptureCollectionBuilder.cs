using ChurchPresenter.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests.Builders
{
    class TestScriptureCollectionBuilder
    {
        private string version = "";
        private Book book = Book.GENESIS;
        private int chapter = 1;
        private int start = 1;
        private int end = 1;

        internal TestScriptureCollectionBuilder UsingVersion(string version)
        {
            this.version = version;
            return this;
        }
        internal TestScriptureCollectionBuilder Reading(Book book)
        {
            this.book = book;
            return this;
        }
        internal TestScriptureCollectionBuilder Chapter(int chapter)
        {
            this.chapter = chapter;
            return this;
        }

        internal TestScriptureCollectionBuilder FromVerse(int verse)
        {
            this.start = verse;
            return this;
        }

        internal TestScriptureCollectionBuilder To(int verse)
        {
            this.end = verse;
            return this;
        }

        internal ScriptureCollection Build()
        {
            var verseList = new List<string>();
            for (int i = start; i <= end; i++)
                verseList.Add("Verse " + i);
            return new ScriptureCollection(version, new Verse(book, chapter, start), new Verse(book, chapter, end), verseList.ToArray());
        }
    }
}
