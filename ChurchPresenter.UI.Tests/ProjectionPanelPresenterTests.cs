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
    class ProjectionPanelPresenterTests
    {

        [Test]
        public void WhenAFolderIsSelected_PreviewTitleIsChanged()
        {
            // Arrange
            var fixture = CreateFixture();
            var testFolder = new LyricFolderBuilder().WithTitle("Bombastic").WithLyrics(testLyrics).Build();

            // Act
            fixture.selectedFolderService.SelectedFolderChanged += Raise.Event<Action<IFolder>>(testFolder);

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
            string[] slides = null;
            fixture.view.When(v => v.SetSlides(Arg.Any<string[]>())).Do(arg => slides = arg.Arg<string[]>());
            fixture.selectedFolderService.SelectedFolderChanged += Raise.Event<Action<IFolder>>(testFolder);

            // Assert
            Assert.That(slides.Length, Is.EqualTo(2));
            Assert.That(slides[0], Is.EqualTo("Slide 1 Test Text"));
            Assert.That(slides[1], Is.EqualTo("Foo Bar Baz"));
        }

        [Test]
        public void WhenAFolderIsSelected_TheFirstSlideIsPublishedAsSelected()
        {
            // Arrange
            var fixture = CreateFixture();
            var testFolder = new LyricFolderBuilder().WithLyrics(testLyrics).Build();

            // Act
            fixture.selectedFolderService.SelectedFolderChanged += Raise.Event<Action<IFolder>>(testFolder);

            // Assert
            fixture.displayedSlideService.Received().DisplaySlide(testFolder.GetSlides()[0]);
        }

        [Test]
        public void WhenNotifiedOfASelectedSlide_SlideIsSelectedOnView()
        {
            // Arrange
            var fixture = CreateFixture();
            var testFolder = new LyricFolderBuilder().WithLyrics(testLyrics).Build();
            fixture.selectedFolderService.SelectedFolderChanged += Raise.Event<Action<IFolder>>(testFolder);

            // Act
            fixture.displayedSlideService.DisplayedSlideChanged += Raise.Event<Action<Slide>>(testFolder.GetSlides()[1]);

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
            fixture.selectedFolderService.SelectedFolderChanged += Raise.Event<Action<IFolder>>(testFolder);

            // Act
            fixture.view.SlideSelected += Raise.Event<Action<int>>(1);

            // Assert
            fixture.displayedSlideService.Received().DisplaySlide(testFolder.GetSlides()[1]);
        }

        struct PreviewPanelPresenterTestsFixture
        {
            public IDisplayedSlideService displayedSlideService;
            public ISelectedFolderService selectedFolderService;
            public IProjectionView view;
            public ProjectionPanelPresenter sut;
        }

        private PreviewPanelPresenterTestsFixture CreateFixture()
        {
            var fixture = new PreviewPanelPresenterTestsFixture();
            fixture.displayedSlideService = Substitute.For<IDisplayedSlideService>();
            fixture.selectedFolderService = Substitute.For<ISelectedFolderService>();
            fixture.view = Substitute.For<IProjectionView>();
            fixture.sut = new ProjectionPanelPresenter(fixture.view, fixture.selectedFolderService, fixture.displayedSlideService);
            return fixture;
        }
    }
}
