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
        public void WhenAFolderIsSelected_PreviewTitleIsChanged()
        {
            // Arrange
            var fixture = CreateFixture();
            var testFolder = new LyricFolderBuilder().WithTitle("Bombastic").WithLyrics(testLyrics).Build();

            // Act
            fixture.selectedFolderPublisher.SelectedFolderChanged += Raise.Event<Action<IFolder>>(testFolder);

            // Assert
            fixture.view.Received().SetTitle("Bombastic");
        }

        [Test]
        public void WhenAFolderIsSelected_PreviewSlidesAreChanged()
        {
            // Arrange
            var fixture = CreateFixture();
            var testFolder = new LyricFolderBuilder().WithLyrics(testLyrics).Build();

            // Act
            fixture.selectedFolderPublisher.SelectedFolderChanged += Raise.Event<Action<IFolder>>(testFolder);

            // Assert
            fixture.view.Received().SetSlides(Arg.Any<Slide[]>());
        }

        [Test]
        public void WhenAFolderIsSelected_TheFirstSlideIsPublishedAsSelected()
        {
            // Arrange
            var fixture = CreateFixture();
            var testFolder = new LyricFolderBuilder().WithLyrics(testLyrics).Build();

            // Act
            fixture.selectedFolderPublisher.SelectedFolderChanged += Raise.Event<Action<IFolder>>(testFolder);

            // Assert
            fixture.selectedSlidePublisher.Received().PublishSelectedSlide(testFolder.GetSlides()[0]);
        }

        [Test]
        public void WhenNotifiedOfASelectedSlide_SlideIsSelectedOnView()
        {
            // Arrange
            var fixture = CreateFixture();
            var testFolder = new LyricFolderBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedFolderPublisher.SelectedFolderChanged += Raise.Event<Action<IFolder>>(testFolder);

            // Act
            fixture.selectedSlidePublisher.SelectedSlideChanged += Raise.Event<Action<Slide>>(testFolder.GetSlides()[1]);

            // Assert
            fixture.view.Received().SelectSlide(1);
            fixture.view.Received().SetPreviewText(testFolder.GetSlides()[1]);
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
        public void GivenASelectedFolder_WhenASlideIsSelected_SlideSelectionIsPublished()
        {
            // Arrange
            var fixture = CreateFixture();
            var testFolder = new LyricFolderBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedFolderPublisher.SelectedFolderChanged += Raise.Event<Action<IFolder>>(testFolder);

            // Act
            fixture.view.SlideSelected += Raise.Event<Action<int>>(1);

            // Assert
            fixture.selectedSlidePublisher.Received().PublishSelectedSlide(testFolder.GetSlides()[1]);
        }

        struct PreviewPanelPresenterTestsFixture
        {
            public ISelectedSliderPublisher selectedSlidePublisher;
            public ISelectedFolderModel selectedFolderPublisher;
            public IProjectionView view;
            public ProjectionPanelPresenter sut;
        }

        private PreviewPanelPresenterTestsFixture CreateFixture()
        {
            var fixture = new PreviewPanelPresenterTestsFixture();
            fixture.selectedSlidePublisher = Substitute.For<ISelectedSliderPublisher>();
            fixture.selectedFolderPublisher = Substitute.For<ISelectedFolderModel>();
            fixture.view = Substitute.For<IProjectionView>();
            fixture.sut = new ProjectionPanelPresenter(fixture.view, fixture.selectedFolderPublisher, fixture.selectedSlidePublisher);
            return fixture;
        }
    }
}
