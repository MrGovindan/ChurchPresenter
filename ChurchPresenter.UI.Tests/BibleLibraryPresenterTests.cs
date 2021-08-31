using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using ChurchPresenter.UI.Presenters;
using ChurchPresenter.UI.Tests.Builders;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests
{
    class BibleLibraryPresenterTests
    {
        [Test]
        public void WhenAvailableBiblesArePublished_ThoseBibleNamesAreShownOnTheView()
        {
            // Arrange
            var fixture = CreateFixture();
            var bibles = new string[] { "KJV", "NIV", "AMP" };

            // Act
            fixture.model.BibleVersionsUpdated += Raise.Event<Action<string[]>>(bibles);

            // Assert
            fixture.view.Received().SetBibleVersions(bibles);
        }

        [TestCase("A", new string[] { "Acts ", "Amos " })]
        [TestCase("a", new string[] { "Acts ", "Amos " })]
        [TestCase("B", new string[] { })]
        [TestCase("c", new string[] { "Colossians " })]
        [TestCase("d", new string[] { "Daniel ", "Deuteronomy " })]
        [TestCase("h", new string[] { "Habakkuk ", "Haggai ", "Hebrews ", "Hosea " })]
        [TestCase("", new string[] {})]
        [Test]
        public void WhenTheNameOfABookIsBeingEntered_PresenterShouldOfferCompletionSuggestions(string search, string[] results)
        {
            // Arrange
            var fixture = CreateFixture();
            var suggestions = new string[] { };
            fixture.view.When(v => v.ShowSuggestions(Arg.Any<string[]>())).Do(Arg => suggestions = Arg.Arg<string[]>());

            // Act
            fixture.view.SearchStringChanged += Raise.Event<Action<string>>(search);

            // Assert
            CollectionAssert.AreEqual(results, suggestions);
        }

        [TestCase("genesis2", Book.GENESIS, 2)]
        [TestCase("john3", Book.JOHN, 3)]
        [TestCase("john3", Book.JOHN, 3)]
        [TestCase("James 3", Book.JAMES, 3)]
        [TestCase("h1", Book.HABAKKUK, 1)]
        [Test]
        public void WhenSearchingOnlyForChapter_ARequestForTheWholeChapterIsMade(string search, Book book, int chapter)
        {
            // Arrange
            var fixture = CreateFixture();
            fixture.model.BibleVersionsUpdated += Raise.Event<Action<string[]>>(new string[] { "KJV", "AMP", "NIV" });

            // Act
            fixture.view.SearchStarted += Raise.Event<Action<string, int>>(search, 2);

            // Assert
            fixture.model.Received().GetWholeChapter(2, book, chapter);
        }

        [TestCase("j2")]
        [TestCase("j2:1")]
        [TestCase("j2:1-3")]
        [Test]
        public void WhenScriptureFolderReceived_SlidesAreShownOnView(string search)
        {
            // Arrange
            var fixture = CreateFixture();

            var scriptureCollection = new TestScriptureCollectionBuilder()
                .UsingVersion("KJV").Reading(Book.ACTS).Chapter(4).FromVerse(1).To(3).Build();

            fixture.model.GetWholeChapter(Arg.Any<int>(), Arg.Any<Book>(), Arg.Any<int>()).Returns(scriptureCollection);
            fixture.model.GetVerse(Arg.Any<int>(), Arg.Any<Verse>()).Returns(scriptureCollection);
            fixture.model.GetVerses(Arg.Any<int>(), Arg.Any<Verse>(), Arg.Any<Verse>()).Returns(scriptureCollection);

            // Act
            string[] actual = null;
            fixture.view.When(v => v.ShowScriptures(Arg.Any<string[]>()))
                .Do(slides => actual = slides.Arg<string[]>());
            fixture.view.SearchStarted += Raise.Event<Action<string, int>>(search, 1);

            // Assert
            CollectionAssert.AreEqual(new string[] { "Acts 4:1 (KJV)", "Acts 4:2 (KJV)", "Acts 4:3 (KJV)" }, actual);
            fixture.view.Received(1).ShowScriptures(Arg.Any<string[]>());
        }

        [TestCase("matth3:8", Book.MATTHEW, 3, 8)]
        [TestCase("john3:16", Book.JOHN, 3, 16)]
        [TestCase("zep2:5", Book.ZEPHANIAH, 2, 5)]
        [TestCase("1 joh2:5", Book.JOHN1, 2, 5)]
        [TestCase("2tim3:6", Book.TIMOTHY2, 3, 6)]
        [Test]
        public void WhenSearchingForChapterAndVerse_ARequestForThatVerseIsMade(string search, Book book, int chapter, int verse)
        {
            // Arrange
            var fixture = CreateFixture();

            // Act
            fixture.view.SearchStarted += Raise.Event<Action<string, int>>(search, 0);

            // Assert
            fixture.model.Received().GetVerse(Arg.Any<int>(), new Verse(book, chapter, verse));
        }

        [TestCase("matth3:8-10", Book.MATTHEW, 3, 8, 10)]
        [TestCase("john3:16-20", Book.JOHN, 3, 16, 20)]
        [TestCase("zep2:5-14", Book.ZEPHANIAH, 2, 5, 14)]
        [TestCase("joh3:16-16", Book.JOHN, 3, 16, 16)]
        [Test]
        public void WhenSearchingForMultipleVersesInChapter_ARequestForTheRangeOfVersesIsMade(string search, Book book, int chapter, int startVerse, int endVerse)
        {
            // Arrange
            var fixture = CreateFixture();

            // Act
            fixture.view.SearchStarted += Raise.Event<Action<string, int>>(search, 0);

            // Assert
            fixture.model.Received().GetVerses(Arg.Any<int>(), new Verse(book, chapter, startVerse), new Verse(book, chapter, endVerse));
        }

        [TestCase("matth3:8-1")]
        [TestCase("john3:16-2")]
        [TestCase("zep2:5-1")]
        [Test]
        public void WhenSearchingForMultipleVersesInChapter_AndEndRangeIsSmallerThanStart_ViewShowsNoResults(string search)
        {
            // Arrange
            var fixture = CreateFixture();

            // Act
            fixture.view.SearchStarted += Raise.Event<Action<string, int>>(search, 0);

            // Assert
            fixture.view.Received().ShowNoResults();
        }

        [TestCase("james")]
        [TestCase("hosea")]
        [Test]
        public void WhenOnlyBookIsSearchedFor_ViewShowsNoResults(string search)
        {
            // Arrange
            var fixture = CreateFixture();

            // Act
            fixture.view.SearchStarted += Raise.Event<Action<string, int>>(search, 0);

            // Assert
            fixture.view.ShowNoResults();
        }

        [Test]
        public void GivenAllItemsSelected_WhenAddedToService_FolderIsAddedToService()
        {
            // Arrange
            var fixture = CreateFixture();
            var scriptureCollection = new TestScriptureCollectionBuilder()
                .UsingVersion("KJV").Reading(Book.REVELATION).Chapter(3).FromVerse(1).To(3).Build();
            fixture.model.GetWholeChapter(Arg.Any<int>(), Arg.Any<Book>(), Arg.Any<int>()).Returns(scriptureCollection);

            fixture.view.SearchStarted += Raise.Event<Action<string, int>>("rev3", 0);
            IFolder addedFolder = null;
            fixture.serviceModel.AddFolder(Arg.Do<IFolder>(arg => addedFolder = arg));

            // Act
            fixture.view.AddedToService += Raise.Event<Action<int[]>>(new int[] { 0, 1, 2 });

            // Assert
            Assert.That(addedFolder.GetFolderType(), Is.EqualTo(FolderType.Scripture));
            Assert.That(addedFolder.GetTitle(), Is.EqualTo("Revelation 3:1-3 KJV"));
            Assert.That(addedFolder.GetSlides().Length, Is.EqualTo(3));
            Assert.That(addedFolder.GetSlides()[0].ToString(), Is.EqualTo("3:1 Verse 1"));
            Assert.That(addedFolder.GetSlides()[0].GetCaption(), Is.EqualTo("Revelation 3:1 KJV"));
            Assert.That(addedFolder.GetSlides()[1].ToString(), Is.EqualTo("3:2 Verse 2"));
            Assert.That(addedFolder.GetSlides()[2].ToString(), Is.EqualTo("3:3 Verse 3"));
        }

        [Test]
        public void GivenAllItemsSelected_WhenAddedToService_FolderIsAddedToService2()
        {
            // Arrange
            var fixture = CreateFixture();
            var scriptureCollection = new TestScriptureCollectionBuilder()
                .UsingVersion("KJV").Reading(Book.REVELATION).Chapter(3).FromVerse(10).To(14).Build();
            fixture.model.GetWholeChapter(Arg.Any<int>(), Arg.Any<Book>(), Arg.Any<int>()).Returns(scriptureCollection);

            fixture.view.SearchStarted += Raise.Event<Action<string, int>>("rev3", 0);
            IFolder addedFolder = null;
            fixture.serviceModel.AddFolder(Arg.Do<IFolder>(arg => addedFolder = arg));

            // Act
            fixture.view.AddedToService += Raise.Event<Action<int[]>>(new int[] { 0, 1, 2, 3, 4 });

            // Assert
            Assert.That(addedFolder.GetFolderType(), Is.EqualTo(FolderType.Scripture));
            Assert.That(addedFolder.GetTitle(), Is.EqualTo("Revelation 3:10-14 KJV"));
            Assert.That(addedFolder.GetSlides()[0].ToString(), Is.EqualTo("3:10 Verse 10"));
            Assert.That(addedFolder.GetSlides()[0].GetCaption(), Is.EqualTo("Revelation 3:10 KJV"));
        }

        [Test]
        public void GivenASubsetOfItemsSelected_WhenAddedToService_FolderIsAddedToService()
        {
            // Arrange
            var fixture = CreateFixture();
            var scriptureCollection = new TestScriptureCollectionBuilder()
                .UsingVersion("KJV").Reading(Book.REVELATION).Chapter(3).FromVerse(1).To(5).Build();
            fixture.model.GetWholeChapter(Arg.Any<int>(), Arg.Any<Book>(), Arg.Any<int>()).Returns(scriptureCollection);

            fixture.view.SearchStarted += Raise.Event<Action<string, int>>("rev3", 0);

            IFolder addedFolder = null;
            fixture.serviceModel.AddFolder(Arg.Do<IFolder>(arg => addedFolder = arg));

            // Act
            fixture.view.AddedToService += Raise.Event<Action<int[]>>(new int[] { 0, 2 });

            // Assert
            Assert.That(addedFolder.GetFolderType, Is.EqualTo(FolderType.Scripture));
            Assert.That(addedFolder.GetTitle(), Is.EqualTo("Revelation 3:1, 3 KJV"));
            Assert.That(addedFolder.GetSlides().Length, Is.EqualTo(2));
            Assert.That(addedFolder.GetSlides()[0].ToString(), Is.EqualTo("3:1 Verse 1"));
            Assert.That(addedFolder.GetSlides()[1].ToString(), Is.EqualTo("3:3 Verse 3"));
        }

        [Test]
        public void GivenADisparateSelectionOfItems_WhenAddedToService_FolderIsAddedToService()
        {
            // Arrange
            var fixture = CreateFixture();
            var scriptureCollection = new TestScriptureCollectionBuilder()
                .UsingVersion("KJV").Reading(Book.REVELATION).Chapter(3).FromVerse(1).To(5).Build();
            fixture.model.GetWholeChapter(Arg.Any<int>(), Arg.Any<Book>(), Arg.Any<int>()).Returns(scriptureCollection);

            fixture.view.SearchStarted += Raise.Event<Action<string, int>>("rev3", 0);

            IFolder addedFolder = null;
            fixture.serviceModel.AddFolder(Arg.Do<IFolder>(arg => addedFolder = arg));

            // Act
            fixture.view.AddedToService += Raise.Event<Action<int[]>>(new int[] { 0, 2, 3, 4 });

            // Assert
            Assert.That(addedFolder.GetFolderType, Is.EqualTo(FolderType.Scripture));
            Assert.That(addedFolder.GetTitle(), Is.EqualTo("Revelation 3:1, 3-5 KJV"));
            Assert.That(addedFolder.GetSlides()[0].ToString(), Is.EqualTo("3:1 Verse 1"));
            Assert.That(addedFolder.GetSlides()[1].ToString(), Is.EqualTo("3:3 Verse 3"));
            Assert.That(addedFolder.GetSlides()[2].ToString(), Is.EqualTo("3:4 Verse 4"));
            Assert.That(addedFolder.GetSlides()[3].ToString(), Is.EqualTo("3:5 Verse 5"));
        }

        [TestCase("j2")]
        [TestCase("j2:1")]
        [TestCase("j2:1-3")]
        [Test]
        public void WhenScriptureFolderContainsNoSlides_ViewShowsNoResults(string search)
        {
            // Arrange
            var fixture = CreateFixture();

            var scriptureCollection = new TestScriptureCollectionBuilder()
                .UsingVersion("KJV").Reading(Book.ACTS).Chapter(4).FromVerse(2).To(1).Build();

            fixture.model.GetWholeChapter(Arg.Any<int>(), Arg.Any<Book>(), Arg.Any<int>()).Returns(scriptureCollection);
            fixture.model.GetVerse(Arg.Any<int>(), Arg.Any<Verse>()).Returns(scriptureCollection);
            fixture.model.GetVerses(Arg.Any<int>(), Arg.Any<Verse>(), Arg.Any<Verse>()).Returns(scriptureCollection);

            // Act
            fixture.view.SearchStarted += Raise.Event<Action<string, int>>(search, 1);

            // Assert
            fixture.view.Received().ShowNoResults();
        }

        [Test]
        public void WhenAScriptureIsSentToPreview_SelectedFolderModelIsUpdated()
        {
            // Arrange
            var fixture = CreateFixture();
            var scriptures = new TestScriptureCollectionBuilder()
                .UsingVersion("NIV").Reading(Book.PSALMS).Chapter(91).FromVerse(1).To(4).Build();
            fixture.model.GetWholeChapter(Arg.Any<int>(), Arg.Any<Book>(), Arg.Any<int>()).Returns(scriptures);

            IFolder addedFolder = null;
            fixture.previewFolderModel.When(m => m.PublishSelectedFolder(Arg.Any<IFolder>())).Do(arg => addedFolder = arg.Arg<IFolder>());

            fixture.view.SearchStarted += Raise.Event<Action<string, int>>("j4", 1);

            // Act
            fixture.view.ShownOnPreview += Raise.Event<Action<int[]>>(new int[] { 0, 1, 2, 3 });


            Assert.That(addedFolder.GetFolderType(), Is.EqualTo(FolderType.Scripture));
            Assert.That(addedFolder.GetTitle(), Is.EqualTo("Psalms 91:1-4 NIV"));
            Assert.That(addedFolder.GetSlides().Length, Is.EqualTo(4));
        }

        struct BibleLibraryPresenterTestFixture
        {
            public IBibleModel model;
            public IBibleLibraryView view;
            public ISelectedFolderModel previewFolderModel;
            public BibleLibraryPresenter sut;
            internal IServiceModel serviceModel;
        }

        BibleLibraryPresenterTestFixture CreateFixture()
        {
            var fixture = new BibleLibraryPresenterTestFixture();
            fixture.model = Substitute.For<IBibleModel>();
            fixture.view = Substitute.For<IBibleLibraryView>();
            fixture.previewFolderModel = Substitute.For<ISelectedFolderModel>();
            fixture.serviceModel = Substitute.For<IServiceModel>();
            fixture.sut = new BibleLibraryPresenter(fixture.view, fixture.model, fixture.serviceModel, fixture.previewFolderModel);
            return fixture;
        }
    }
}
