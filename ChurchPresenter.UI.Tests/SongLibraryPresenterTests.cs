using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using ChurchPresenter.UI.Presenters;
using ChurchPresenter.UI.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChurchPresenter.UI.Tests
{
    class SongLibraryPresenterTests
    {
        [Test]
        public void WhenViewIsLoaded_PresenterGivesViewTheListOfAvailableSongs()
        {
            // Arrange
            var expectedSongList = CreateSongListFromTitles("foo", "bar", "baz");
            var songLibrary = Substitute.For<ISongLibrary>();
            songLibrary.GetAllSongs().Returns(expectedSongList);
            var view = Substitute.For<ISongLibraryView>();
            var presenter = CreateSut(view, songLibrary);

            List<string> actual = null;
            view.When(v => v.SetSongList(Arg.Any<IEnumerable<string>>())).Do(args =>
            {
                IEnumerable<string> enumerable = args.Arg<IEnumerable<string>>();
                actual = new List<string>(enumerable);
            });

            // Act
            view.OnLoaded += Raise.Event<Action>();

            // Assert
            Assert.That(actual.Count, Is.EqualTo(3));
        }

        [Test]
        public void WhenSearchingForASong_PresenterGivesViewTheListOfMatchingSongs()
        {
            // Arrange
            var expectedSongList = CreateSongListFromTitles("foo", "bar", "baz");
            var songLibrary = Substitute.For<ISongLibrary>();
            songLibrary.GetMatchingSongs(Arg.Any<string>()).Returns(expectedSongList);
            var view = Substitute.For<ISongLibraryView>();
            var presenter = CreateSut(view, songLibrary);

            List<string> actual = null;
            view.When(v => v.SetSongList(Arg.Any<IEnumerable<string>>())).Do(args =>
            {
                IEnumerable<string> enumerable = args.Arg<IEnumerable<string>>();
                actual = new List<string>(enumerable);
            });

            // Act
            view.SearchStringChanged += Raise.Event<Action<string>>("asdf");

            // Assert
            Assert.That(actual.Count, Is.EqualTo(3));
        }

        [Test]
        public void WhenSearchingForASong_PresenterRequestsForMatchingSongUsingSearchText()
        {
            // Arrange
            var songLibrary = Substitute.For<ISongLibrary>();
            var view = Substitute.For<ISongLibraryView>();
            var presenter = CreateSut(view, songLibrary);

            // Act
            view.SearchStringChanged += Raise.Event<Action<string>>("asdf");

            // Assert
            songLibrary.Received().GetMatchingSongs("asdf");
        }

        [Test]
        public void WhenSearchingForASong_PresenterOnlyRequestsForMatchingSongWhenSearchTextIsLongerThan3Characters()
        {
            // Arrange
            var songLibrary = Substitute.For<ISongLibrary>();
            var view = Substitute.For<ISongLibraryView>();
            var presenter = CreateSut(view, songLibrary);

            // Act
            view.SearchStringChanged += Raise.Event<Action<string>>("asd");

            // Assert
            songLibrary.DidNotReceive().GetMatchingSongs(Arg.Any<string>());
        }


        [Test]
        public void WhenSearchingTextIsEmptied_ViewShowsAllSongs()
        {
            // Arrange
            var expectedSongList = CreateSongListFromTitles("foo", "bar", "baz");
            var songLibrary = Substitute.For<ISongLibrary>();
            songLibrary.GetAllSongs().Returns(expectedSongList);
            var view = Substitute.For<ISongLibraryView>();
            var presenter = CreateSut(view, songLibrary);

            List<string> actual = null;
            view.When(v => v.SetSongList(Arg.Any<IEnumerable<string>>())).Do(args =>
            {
                IEnumerable<string> enumerable = args.Arg<IEnumerable<string>>();
                actual = new List<string>(enumerable);
            });

            // Act
            view.SearchStringChanged += Raise.Event<Action<string>>("");

            // Assert
            Assert.That(actual.Count, Is.EqualTo(3));
        }

        [Test]
        public void WhenViewIsLoadedTwoTimes_PresenterOnlyReactsToTheFirstLoad()
        {
            // Arrange
            var expectedSongList = CreateSongListFromTitles("foo", "bar", "baz");
            var songLibrary = Substitute.For<ISongLibrary>();
            songLibrary.GetAllSongs().Returns(expectedSongList);
            var view = Substitute.For<ISongLibraryView>();
            var presenter = CreateSut(view, songLibrary);

            // Act
            view.OnLoaded += Raise.Event<Action>();
            view.OnLoaded += Raise.Event<Action>();

            // Assert
            view.Received(1).SetSongList(Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void WhenSearchStringIsEmptyAfterViewIsLoaded_PresenterRefreshesTheSongList()
        {
            // Arrange
            var expectedSongList = CreateSongListFromTitles("foo", "bar", "baz");
            var songLibrary = Substitute.For<ISongLibrary>();
            songLibrary.GetAllSongs().Returns(expectedSongList);
            var view = Substitute.For<ISongLibraryView>();
            var presenter = CreateSut(view, songLibrary);

            // Act
            view.OnLoaded += Raise.Event<Action>();
            view.SearchStringChanged += Raise.Event<Action<string>>("");

            // Assert
            view.Received(2).SetSongList(Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void WhenASongIsSelected_TheSongIsPublishedAsSelected()
        {
            // Arrange
            var songList = CreateSongListFromTitles("foo", "bar", "baz");
            var songLibrary = Substitute.For<ISongLibrary>();
            songLibrary.GetAllSongs().Returns(songList);
            var view = Substitute.For<ISongLibraryView>();
            var publisher = Substitute.For<ISelectedFolderService>();
            var presenter = CreateSut(view, songLibrary, publisher);

            // Act
            view.OnLoaded += Raise.Event<Action>();
            view.SelectedSongChanged += Raise.Event<Action<int>>(1);

            // Assert
            publisher.Received().SelectFolder(songList[1]);
        }
        
        [Test]
        public void GivenASongList_WhenASongIsAddedToService_PresenterAddsSongToService()
        {
            // Arrange
            var songList = CreateSongListFromTitles("foo", "bar", "baz");
            var songLibrary = Substitute.For<ISongLibrary>();
            songLibrary.GetAllSongs().Returns(songList);
            var view = Substitute.For<ISongLibraryView>();
            var serviceModel = Substitute.For<IServiceModel>();
            var presenter = CreateSut(view, songLibrary, null, serviceModel);

            // Act
            view.OnLoaded += Raise.Event<Action>();
            view.SongAddedToService += Raise.Event<Action<int>>(2);

            // Assert
            serviceModel.Received().AddFolder(songList[2]);
        }


        private static SongLibraryPresenter CreateSut(
            ISongLibraryView view,
            ISongLibrary songLibrary,
            ISelectedFolderService selectedFolderService = null,
            IServiceModel serviceModel = null)
        {
            if (selectedFolderService == null)
                selectedFolderService = Substitute.For<ISelectedFolderService>();
            if (serviceModel == null)
                serviceModel = Substitute.For<IServiceModel>();

            return new SongLibraryPresenter(view, songLibrary, selectedFolderService, serviceModel);
        }

        private static IList<LyricFolder> CreateSongListFromTitles(params string[] titles)
        {
            var songs = new List<LyricFolder>();
            foreach (var title in titles)
                songs.Add(new LyricFolderBuilder().WithTitle(title).Build());
            return songs;
        }
    }
}
