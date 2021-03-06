using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using ChurchPresenter.UI.Presenters;
using ChurchPresenter.UI.Services;
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
            var testSong = new LyricFolderBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedFolderService.SelectFolder(testSong);

            // Act
            fixture.displayedSlideService.DisplaySlide(testSong.GetSlides()[0]);

            // Assert
            fixture.view.Received(2).SetPreviousSlideButtonEnabled(false);
        }

        [Test]
        public void GivenASelectedSongWithTheSecondSlideSelected_WhenGoToPreviousSlideButtonIsClicked_ThenFirstSlideIsSelected()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new LyricFolderBuilder().WithLyrics(testLyrics).Build();
            Slide currentSlide = new Slide("");
            fixture.displayedSlideService.DisplayedSlideChanged += slide => currentSlide = slide;
            fixture.selectedFolderService.SelectFolder(testSong);
            fixture.displayedSlideService.DisplaySlide(testSong.GetSlides()[1]);

            // Act
            fixture.view.GoToPreviousSlide += Raise.Event<Action>();

            // Assert
            Assert.That(currentSlide, Is.EqualTo(testSong.GetSlides()[0]));
        }

        [Test]
        public void GivenASelectedSong_WhenASlideOtherThanTheFirstIsSelected_ThenPreviousSlideButtonIsDisabled()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new LyricFolderBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedFolderService.SelectFolder(testSong);

            // Act
            fixture.displayedSlideService.DisplaySlide(testSong.GetSlides()[1]);

            // Assert
            fixture.view.Received().SetPreviousSlideButtonEnabled(true);
            fixture.view.Received().SetNextSlideButtonEnabled(true);
        }

        [Test]
        public void GivenASelectedSong_WhenTheSelectedSlideIsTheLastSlide_ThenTheNextSlideButtonIsDisabled()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new LyricFolderBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedFolderService.SelectFolder(testSong);

            // Act
            fixture.displayedSlideService.DisplaySlide(testSong.GetSlides()[2]);

            // Assert
            fixture.view.Received(2).SetNextSlideButtonEnabled(false);
        }

        [Test]
        public void WhenASongIsSelected_AndASlideOtherThanTheFirstIsSelected_ThenPreviousSlideButtonIsDisabled()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new LyricFolderBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedFolderService.SelectFolder(testSong);

            // Act
            fixture.displayedSlideService.DisplaySlide(testSong.GetSlides()[1]);

            // Assert
            fixture.view.Received().SetPreviousSlideButtonEnabled(true);
        }

        [Test]
        public void WhenASongIsSelected_NextSlideButtonIsUsed_ThenTheNextSlideIsSelected()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new LyricFolderBuilder().WithLyrics(testLyrics).Build();
            Slide currentSlide = new Slide("");
            fixture.displayedSlideService.DisplayedSlideChanged += slide => currentSlide = slide;
            fixture.selectedFolderService.SelectFolder(testSong);

            // Act
            // Assert
            fixture.view.GoToNextSlide += Raise.Event<Action>();
            Assert.That(currentSlide, Is.EqualTo(testSong.GetSlides()[1]));

            fixture.view.GoToPreviousSlide += Raise.Event<Action>();
            Assert.That(currentSlide, Is.EqualTo(testSong.GetSlides()[0]));
        }

        struct SlideControlButtonsPresenterTestFixture
        {
            public ISelectedFolderService selectedFolderService;
            internal IDisplayedSlideService displayedSlideService;
            internal SlideControlButtonsPresenter sut;
            internal ISlideControlButtonsView view;
        }

        private SlideControlButtonsPresenterTestFixture CreateFixture()
        {
            var fixture = new SlideControlButtonsPresenterTestFixture();
            fixture.selectedFolderService = new SelectedFolderService();
            fixture.displayedSlideService = new DisplayedSlideService();
            fixture.view = Substitute.For<ISlideControlButtonsView>();
            fixture.sut = new SlideControlButtonsPresenter(fixture.view, fixture.selectedFolderService, fixture.displayedSlideService);

            return fixture;
        }
    }
    class LiveSlideControlButtonsPresenterTests
    {
        [Test]
        public void WhenSlideIsVisible_HideButtonIsEnabledShowButtonIsDisabled()
        {
            // Arrange
            var fixture = CreateFixture();

            // Act
            fixture.visibilityService.SetSlideVisible(true);

            // Assert
            Assert.That(fixture.view.HideEnabled, Is.True);
            Assert.That(fixture.view.ShowEnabled, Is.False);
        }

        [Test]
        public void WhenSlideIsHidden_HideButtonIsDisabledShowButtonIsEnabled()
        {
            // Arrange
            var fixture = CreateFixture();

            // Act
            fixture.visibilityService.SetSlideVisible(false);

            // Assert
            Assert.That(fixture.view.HideEnabled, Is.False);
            Assert.That(fixture.view.ShowEnabled, Is.True);
        }

        [Test]
        public void WhenSlideShown_VisibilityModelIsUpdated()
        {
            // Arrange
            var fixture = CreateFixture();

            // Act
            fixture.view.FireSlideHidden();
            fixture.view.FireSlideShown();

            // Assert
            Assert.That(fixture.visibilityService.IsSlideVisible(), Is.True);
        }

        [Test]
        public void WhenSlideHidden_VisibilityModelIsUpdated()
        {
            // Arrange
            var fixture = CreateFixture();

            // Act
            fixture.view.FireSlideShown();
            fixture.view.FireSlideHidden();

            // Assert
            Assert.That(fixture.visibilityService.IsSlideVisible(), Is.False);
        }

        struct LiveSlideControlButtonsPresenterTestFixture
        {
            internal ISelectedFolderService selectedFolderService;
            internal IDisplayedSlideService displayedSlideService;
            internal StubLiveSlideControlButtonsView view;
            internal ISlideVisibilityService visibilityService;
            internal LiveSlideControlButtonsPresenter sut;
        }

        class StubLiveSlideControlButtonsView : ILiveSlideControlButtonsView
        {
            public event Action SlideShown;
            public event Action SlideHidden;
            public bool HideEnabled = true;
            public bool ShowEnabled = true;
#pragma warning disable CS0067
            public event Action GoToPreviousSlide;
            public event Action GoToNextSlide;
#pragma warning restore CS0067

            public void SetHideSlideButtonEnabled(bool enabled) => HideEnabled = enabled;
            public void SetShowSlideButtonEnabled(bool enabled) => ShowEnabled = enabled;
            public void FireSlideShown() => SlideShown?.Invoke();
            public void FireSlideHidden() => SlideHidden?.Invoke();
            public void SetPreviousSlideButtonEnabled(bool enabled) { }
            public void SetNextSlideButtonEnabled(bool enabled) { }
        }

        private LiveSlideControlButtonsPresenterTestFixture CreateFixture()
        {
            var fixture = new LiveSlideControlButtonsPresenterTestFixture();
            fixture.selectedFolderService = new SelectedFolderService();
            fixture.displayedSlideService = new DisplayedSlideService();
            fixture.view = new StubLiveSlideControlButtonsView();
            fixture.visibilityService = new SlideVisibilityService();
            fixture.sut = new LiveSlideControlButtonsPresenter(fixture.view, fixture.selectedFolderService, fixture.displayedSlideService, fixture.visibilityService);

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
            var testSong = new LyricFolderBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedFolderService.SelectFolder(testSong);

            // Act
            fixture.view.SongAddedToService += Raise.Event<Action>();

            // Assert
            fixture.serviceModel.Received(1).AddFolder(testSong);
        }

        [Test]
        public void GivenASelectedSong_WhenSongAddedToLive_LiveSongSelectionPublisherNotitified()
        {
            // Arrange
            var fixture = CreateFixture();
            var testSong = new LyricFolderBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedFolderService.SelectFolder(testSong);

            // Act
            fixture.view.SongShownOnLive += Raise.Event<Action>();

            // Assert
            fixture.liveSongSelectionPublisher.Received(1).SelectFolder(testSong);
        }

        struct PreviewSlideControlButtonsPresenterTestFixture
        {
            internal ISelectedFolderService selectedFolderService;
            internal ISelectedFolderService liveSongSelectionPublisher;
            internal IDisplayedSlideService displayedSlideService;
            internal IPreviewSlideControlButtonsView view;
            internal IServiceModel serviceModel;
            internal PreviewSlideControlButtonsPresenter sut;
        }

        private PreviewSlideControlButtonsPresenterTestFixture CreateFixture()
        {
            var fixture = new PreviewSlideControlButtonsPresenterTestFixture();
            fixture.selectedFolderService = new SelectedFolderService();
            fixture.displayedSlideService = new DisplayedSlideService();
            fixture.view = Substitute.For<IPreviewSlideControlButtonsView>();
            fixture.serviceModel = Substitute.For<IServiceModel>();
            fixture.liveSongSelectionPublisher = Substitute.For<ISelectedFolderService>();
            fixture.sut = new PreviewSlideControlButtonsPresenter(fixture.view, fixture.selectedFolderService, fixture.displayedSlideService, fixture.serviceModel, fixture.liveSongSelectionPublisher);

            return fixture;
        }
    }
}
