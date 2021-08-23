using ChurchPresenter.UI.Presenters;
using NSubstitute;
using NUnit.Framework;
using System;

namespace ChurchPresenter.UI.Tests
{
    public class LibraryPresenterTests
    {
        [Test]
        public void WhenSongsCategoryIsSelected_TheViewIsUpdatedToShowTheSongLibrary()
        {
            // Arrange
            var fixture = new LibraryPresenterFixtureBuilder<string>()
                .WithContentFactoryReturning(LibraryContent.Songs, "SongContent")
                .Build();

            // Act
            fixture.view.SongsSelected += Raise.Event<Action>();

            // Assert
            fixture.view.Received().ShowContent(LibraryContent.Songs, "SongContent");
        }

        [Test]
        public void WhenBiblesCategoryIsSelected_TheViewIsUpdatedToShowTheBibleLibrary()
        {
            // Arrange
            var fixture = new LibraryPresenterFixtureBuilder<string>()
                .WithContentFactoryReturning(LibraryContent.Bibles, "BibleContent")
                .Build();

            // Act
            fixture.view.BiblesSelected += Raise.Event<Action>();

            // Assert
            fixture.view.Received().ShowContent(LibraryContent.Bibles, "BibleContent");
        }

        [Test]
        public void WhenViewIsDoneLoading_TheViewIsUpdatedToShowTheSongLibrary()
        {
            // Arrange
            var fixture = new LibraryPresenterFixtureBuilder<string>()
                .WithContentFactoryReturning(LibraryContent.Songs, "SongContent")
                .Build();

            // Act
            fixture.view.OnLoaded += Raise.Event<Action>();

            // Assert
            fixture.view.Received().ShowContent(LibraryContent.Songs, "SongContent");
        }

        class LibraryPresenterFixture<T>
        {
            public ILibraryContentFactory<T> contentFactory = Substitute.For<ILibraryContentFactory<T>>();
            public ILibraryView<T> view = Substitute.For<ILibraryView<T>>();
            public LibraryPresenter<T> presenter;
        }

        class LibraryPresenterFixtureBuilder<T>
        {
            LibraryPresenterFixture<T> fixture = new LibraryPresenterFixture<T>();

            public LibraryPresenterFixtureBuilder<T> WithContentFactoryReturning(LibraryContent contentType, T value)
            {
                fixture.contentFactory.GetContent(contentType).Returns(value);
                return this;
            }

            public LibraryPresenterFixture<T> Build()
            {
                fixture.presenter = new LibraryPresenter<T>(fixture.view, fixture.contentFactory);
                return fixture;
            }
        }
    }
}