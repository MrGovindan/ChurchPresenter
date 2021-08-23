using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Presenters;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests
{
    class ProjectionPanelPresenterTests
    {

        [Test]
        public void WhenASongIsSelected_PreviewTitleIsChanged()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithTitle("Bombastic").WithLyrics(testLyrics).Build();

            // Act
            fixture.selectedSongPublisher.SelectedSongChanged += Raise.Event<Action<Song>>(testSong);

            // Assert
            fixture.view.Received().SetTitle("Bombastic");
        }

        [Test]
        public void WhenASongIsSelected_PreviewSlidesAreChanged()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();

            // Act
            fixture.selectedSongPublisher.SelectedSongChanged += Raise.Event<Action<Song>>(testSong);

            // Assert
            fixture.view.Received().SetSlides(Arg.Any<Slide[]>());
        }

        [Test]
        public void WhenASongIsSelected_TheFirstSlideIsPublishedAsSelected()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();

            // Act
            fixture.selectedSongPublisher.SelectedSongChanged += Raise.Event<Action<Song>>(testSong);

            // Assert
            fixture.selectedSlidePublisher.Received().PublishSelectedSlide(testSong.slides[0]);
        }

        [Test]
        public void WhenNotifiedOfASelectedSlide_SlideIsSelectedOnView()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.SelectedSongChanged += Raise.Event<Action<Song>>(testSong);

            // Act
            fixture.selectedSlidePublisher.SelectedSlideChanged += Raise.Event<Action<Slide>>(testSong.slides[1]);

            // Assert
            fixture.view.Received().SelectSlide(1);
            fixture.view.Received().SetPreviewText("Foo Bar Baz");
        }

        private static string testLyrics = @"<?xml version='1.0' encoding='UTF-8'?>
<song version='1.0'>
    <lyrics>
        <verse label='1' type='c'>
            <![CDATA[Slide 1 Test Text]]>
        </verse>
        <verse label='1' type='c'>
            <![CDATA[Foo Bar Baz]]>
        </verse>
    </lyrics>
</song>";

        [Test]
        public void GivenASelectedSong_WhenASlideIsSelected_SlideSelectionIsPublished()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.SelectedSongChanged += Raise.Event<Action<Song>>(testSong);

            // Act
            fixture.view.SlideSelected += Raise.Event<Action<int>>(1);

            // Assert
            fixture.selectedSlidePublisher.Received().PublishSelectedSlide(testSong.slides[1]);
        }

        struct PreviewPanelPresenterTestsFixture
        {
            public ISelectedSlidePublisher selectedSlidePublisher;
            public ISelectedSongPublisher selectedSongPublisher;
            public IProjectionView view;
            public ProjectionPanelPresenter sut;
        }

        private PreviewPanelPresenterTestsFixture CreateFixture()
        {
            var fixture = new PreviewPanelPresenterTestsFixture();
            fixture.selectedSlidePublisher = Substitute.For<ISelectedSlidePublisher>();
            fixture.selectedSongPublisher = Substitute.For<ISelectedSongPublisher>();
            fixture.view = Substitute.For<IProjectionView>();
            fixture.sut = new ProjectionPanelPresenter(fixture.view, fixture.selectedSongPublisher, fixture.selectedSlidePublisher);
            return fixture;
        }
    }
}
