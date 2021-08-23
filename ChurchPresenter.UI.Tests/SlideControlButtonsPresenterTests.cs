using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Presenters;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests
{
    class SlideControlButtonsPresenterTests
    {
        private static string testLyrics = @"<?xml version='1.0' encoding='UTF-8'?>
<song version='1.0'>
    <lyrics>
        <verse label='1' type='c'>
            <![CDATA[Foo]]>
        </verse>
        <verse label='1' type='c'>
            <![CDATA[Bar]]>
        </verse>
        <verse label='1' type='c'>
            <![CDATA[Baz]]>
        </verse>
    </lyrics>
</song>";

        [Test]
        public void WhenPresenterCreated_ThenPreviousAndNextSlideButtonsAreDisabled()
        {
            // Arrange
            var fixture = CreateFixture();

            // Act
            // Assert
            fixture.view.Received().SetPreviousSlideButtonEnabled(false);
            fixture.view.Received().SetNextSlideButtonEnabled(false);
        }

        [Test]
        public void GivenASelectedSong_WhenTheFirstSlideIsSelected_ThenPreviousSlideButtonIsDisabled()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);

            // Act
            fixture.selectedSlidePublisher.PublishSelectedSlide(testSong.slides[0]);

            // Assert
            fixture.view.Received(2).SetPreviousSlideButtonEnabled(false);
        }

        [Test]
        public void GivenASelectedSongWithTheSecondSlideSelected_WhenGoToPreviousSlideButtonIsClicked_ThenFirstSlideIsSelected()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            Slide currentSlide = new Slide("");
            fixture.selectedSlidePublisher.SelectedSlideChanged += slide => currentSlide = slide;
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);
            fixture.selectedSlidePublisher.PublishSelectedSlide(testSong.slides[1]);

            // Act
            fixture.view.GoToPreviousSlide += Raise.Event<Action>();

            // Assert
            Assert.That(currentSlide, Is.EqualTo(testSong.slides[0]));
        }

        [Test]
        public void GivenASelectedSong_WhenASlideOtherThanTheFirstIsSelected_ThenPreviousSlideButtonIsDisabled()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);

            // Act
            fixture.selectedSlidePublisher.PublishSelectedSlide(testSong.slides[1]);

            // Assert
            fixture.view.Received().SetPreviousSlideButtonEnabled(true);
            fixture.view.Received().SetNextSlideButtonEnabled(true);
        }

        [Test]
        public void GivenASelectedSong_WhenTheSelectedSlideIsTheLastSlide_ThenTheNextSlideButtonIsDisabled()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);

            // Act
            fixture.selectedSlidePublisher.PublishSelectedSlide(testSong.slides[2]);

            // Assert
            fixture.view.Received(2).SetNextSlideButtonEnabled(false);
        }

        [Test]
        public void WhenASongIsSelected_AndASlideOtherThanTheFirstIsSelected_ThenPreviousSlideButtonIsDisabled()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);

            // Act
            fixture.selectedSlidePublisher.PublishSelectedSlide(testSong.slides[1]);

            // Assert
            fixture.view.Received().SetPreviousSlideButtonEnabled(true);
        }

        [Test]
        public void WhenASongIsSelected_NextSlideButtonIsUsed_ThenTheNextSlideIsSelected()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            Slide currentSlide = new Slide("");
            fixture.selectedSlidePublisher.SelectedSlideChanged += slide => currentSlide = slide;
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);

            // Act
            // Assert
            fixture.view.GoToNextSlide += Raise.Event<Action>();
            Assert.That(currentSlide, Is.EqualTo(testSong.slides[1]));

            fixture.view.GoToPreviousSlide += Raise.Event<Action>();
            Assert.That(currentSlide, Is.EqualTo(testSong.slides[0]));
        }

        struct SlideControlButtonsPresenterTestFixture
        {
            public ISelectedSongPublisher selectedSongPublisher;
            internal ISelectedSlidePublisher selectedSlidePublisher;
            internal SlideControlButtonsPresenter sut;
            internal ISlideControlButtonsView view;
        }

        private SlideControlButtonsPresenterTestFixture CreateFixture()
        {
            var fixture = new SlideControlButtonsPresenterTestFixture();
            fixture.selectedSongPublisher = new SelectedSongPublisher();
            fixture.selectedSlidePublisher = new SelectedSlidePublisher();
            fixture.view = Substitute.For<ISlideControlButtonsView>();
            fixture.sut = new SlideControlButtonsPresenter(fixture.view, fixture.selectedSongPublisher, fixture.selectedSlidePublisher);

            return fixture;
        }
    }
    class LiveSlideControlButtonsPresenterTests
    {
        private static string testLyrics = @"<?xml version='1.0' encoding='UTF-8'?>
<song version='1.0'>
    <lyrics>
        <verse label='1' type='c'>
            <![CDATA[Foo]]>
        </verse>
        <verse label='1' type='c'>
            <![CDATA[Bar]]>
        </verse>
        <verse label='1' type='c'>
            <![CDATA[Baz]]>
        </verse>
    </lyrics>
</song>";

        [Test]
        public void WhenPresenterCreated_HideAndShowSlideButtonsAreDisabled()
        {
            // Arrange
            // Act
            var fixture = CreateFixture();

            // Assert
            Assert.That(fixture.view.HideEnabled, Is.False);
            Assert.That(fixture.view.ShowEnabled, Is.False);
        }

        [Test]
        public void GivenASelectedSong_HideSlideButtonIsEnabled()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();

            // Act
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);

            // Assert
            Assert.That(fixture.view.HideEnabled, Is.True);
        }

        [Test]
        public void GivenASelectedSongAndSlideHidden_WhenAnotherSongIsSelected_HideSlideButtonIsNotEnabled()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            var testSong2 = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);
            fixture.view.FireSlideHidden();

            // Act
            fixture.selectedSongPublisher.PublishSelectedSong(testSong2);

            // Assert
            Assert.That(fixture.view.HideEnabled, Is.False);
        }

        [Test]
        public void GivenASelectedSong_WhenSlideHidden_HideSlideButtonIsDisabled()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);

            // Act
            fixture.view.FireSlideHidden();

            // Assert
            Assert.That(fixture.view.HideEnabled, Is.False);
            Assert.That(fixture.view.ShowEnabled, Is.True);
        }

        [Test]
        public void GivenASelectedSong_WhenSlideHidden_SlideNotVisiblePublished()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);

            // Act
            fixture.view.FireSlideHidden();

            // Assert
            fixture.visibilityPublisher.Received(1).PublishSlideVisibility(false);
        }

        [Test]
        public void GivenASelectedSong_WhenSlideShown_ShowSlideButtonIsDisabled()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);

            // Act
            fixture.view.FireSlideShown();

            // Assert
            Assert.That(fixture.view.ShowEnabled, Is.False);
            Assert.That(fixture.view.HideEnabled, Is.True);
        }

        [Test]
        public void GivenASelectedSong_WhenSlideShown_SlideVisiblePublished()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);

            // Act
            fixture.view.FireSlideShown();

            // Assert
            fixture.visibilityPublisher.Received(1).PublishSlideVisibility(true);
        }

        struct LiveSlideControlButtonsPresenterTestFixture
        {
            internal ISelectedSongPublisher selectedSongPublisher;
            internal ISelectedSlidePublisher selectedSlidePublisher;
            internal StubLiveSlideControlButtonsView view;
            internal ISlideVisibilityPublisher visibilityPublisher;
            internal LiveSlideControlButtonsPresenter sut;
        }

        class StubLiveSlideControlButtonsView : ILiveSlideControlButtonsView
        {
            public event Action SlideShown;
            public event Action SlideHidden;
            public event Action GoToPreviousSlide;
            public event Action GoToNextSlide;

            public bool HideEnabled = true;
            public bool ShowEnabled = true;

            public void SetHideSlideButtonEnabled(bool enabled)
            {
                HideEnabled = enabled;
            }

            public void SetShowSlideButtonEnabled(bool enabled)
            {
                ShowEnabled = enabled;
            }

            public void FireSlideShown()
            {
                SlideShown?.Invoke();
            }

            public void FireSlideHidden()
            {
                SlideHidden?.Invoke();
            }

            public void SetPreviousSlideButtonEnabled(bool enabled)
            {
            }

            public void SetNextSlideButtonEnabled(bool enabled)
            {
            }
        }

        private LiveSlideControlButtonsPresenterTestFixture CreateFixture()
        {
            var fixture = new LiveSlideControlButtonsPresenterTestFixture();
            fixture.selectedSongPublisher = new SelectedSongPublisher();
            fixture.selectedSlidePublisher = new SelectedSlidePublisher();
            fixture.view = new StubLiveSlideControlButtonsView();
            fixture.visibilityPublisher = Substitute.For<ISlideVisibilityPublisher>();
            fixture.sut = new LiveSlideControlButtonsPresenter(fixture.view, fixture.selectedSongPublisher, fixture.selectedSlidePublisher, fixture.visibilityPublisher);

            return fixture;
        }
    }

    class PreviewSlideControlButtonsPresenterTests
    {
        private static string testLyrics = @"<?xml version='1.0' encoding='UTF-8'?>
<song version='1.0'>
    <lyrics>
        <verse label='1' type='c'>
            <![CDATA[Foo]]>
        </verse>
        <verse label='1' type='c'>
            <![CDATA[Bar]]>
        </verse>
        <verse label='1' type='c'>
            <![CDATA[Baz]]>
        </verse>
    </lyrics>
</song>";

        [Test]
        public void GivenASelectedSong_WhenSongAddedToService_SeviceManagerIsNotified()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);

            // Act
            fixture.view.SongAddedToService += Raise.Event<Action>();

            // Assert
            fixture.serviceModel.Received(1).AddSongToService(testSong);
        }

        [Test]
        public void GivenASelectedSong_WhenSongAddedToLive_LiveSongSelectionPublisherNotitified()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedSongPublisher.PublishSelectedSong(testSong);

            // Act
            fixture.view.SongShownOnLive += Raise.Event<Action>();

            // Assert
            fixture.liveSongSelectionPublisher.Received(1).PublishSelectedSong(testSong);
        }

        struct PreviewSlideControlButtonsPresenterTestFixture
        {
            internal ISelectedSongPublisher selectedSongPublisher;
            internal ISelectedSongPublisher liveSongSelectionPublisher;
            internal ISelectedSlidePublisher selectedSlidePublisher;
            internal IPreviewSlideControlButtonsView view;
            internal IServiceModel serviceModel;
            internal PreviewSlideControlButtonsPresenter sut;
        }

        private PreviewSlideControlButtonsPresenterTestFixture CreateFixture()
        {
            var fixture = new PreviewSlideControlButtonsPresenterTestFixture();
            fixture.selectedSongPublisher = new SelectedSongPublisher();
            fixture.selectedSlidePublisher = new SelectedSlidePublisher();
            fixture.view = Substitute.For<IPreviewSlideControlButtonsView>();
            fixture.serviceModel = Substitute.For<IServiceModel>();
            fixture.liveSongSelectionPublisher = Substitute.For<ISelectedSongPublisher>();
            fixture.sut = new PreviewSlideControlButtonsPresenter(fixture.view, fixture.selectedSongPublisher, fixture.selectedSlidePublisher, fixture.serviceModel, fixture.liveSongSelectionPublisher);

            return fixture;
        }
    }
}
