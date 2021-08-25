using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Presenters;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests
{
    class ServicePanelPresenterTests
    {
        [Test]
        public void WhenServiceModelPublishesSongAdded_ViewIsGivenANewEntry()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testSong = new LyricFolderBuilder().WithTitle("TestTitle").Build();

            // Act
            fixture.model.AddFolder(testSong);

            // Assert
            fixture.view.Received().AddFolder(testSong);
        }

        [Test]
        public void WhenAnEntryIsSelected_SelectedSongPublisherIsNotified()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testSong1 = new LyricFolderBuilder().WithTitle("TestTitle1").Build();
            var testSong2 = new LyricFolderBuilder().WithTitle("TestTitle2").Build();
            fixture.model.AddFolder(testSong1);
            fixture.model.AddFolder(testSong2);

            // Act
            fixture.view.SongSelected += Raise.Event<Action<int>>(1);

            // Assert
            fixture.songSelectedPublisher.Received().PublishSelectedFolder(testSong2);
        }

        [Test]
        public void WhenAnEntryIsRemovedFromService_ViewIsToldToRemoveItem()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testSong1 = new LyricFolderBuilder().WithTitle("TestTitle1").Build();
            var testSong2 = new LyricFolderBuilder().WithTitle("TestTitle2").Build();
            fixture.model.AddFolder(testSong1);
            fixture.model.AddFolder(testSong2);

            // Act
            fixture.view.SongRemoved += Raise.Event<Action<int>>(0);

            // Assert
            fixture.view.Received().RemoveSongTitle(0);
        }

        [Test]
        public void WhenAnEntryIsRemovedFromService_ModelRemovesEntry()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testSong1 = new LyricFolderBuilder().WithTitle("TestTitle1").Build();
            var testSong2 = new LyricFolderBuilder().WithTitle("TestTitle2").Build();
            fixture.model.AddFolder(testSong1);
            fixture.model.AddFolder(testSong2);

            // Act
            fixture.view.SongRemoved += Raise.Event<Action<int>>(0);

            // Assert
            Assert.That(fixture.model.ItemAt(0), Is.EqualTo(testSong2));
        }

        struct ServicePanelPresenterTestFixture
        {
            public readonly ServicePanelPresenter sut;
            public readonly IServicePanelView view;
            public readonly IServiceModel model;
            public readonly ISelectedFolderModel songSelectedPublisher;

            public ServicePanelPresenterTestFixture(
                ServicePanelPresenter sut,
                IServicePanelView view,
                IServiceModel model,
                ISelectedFolderModel songSelectedPublisher)
            {
                this.sut = sut;
                this.view = view;
                this.model = model;
                this.songSelectedPublisher = songSelectedPublisher;
            }
        }

        private ServicePanelPresenterTestFixture CreateTestFixture()
        {
            var model = new ServiceModel();
            var view = Substitute.For<IServicePanelView>();
            var songSelectedPublisher = Substitute.For<ISelectedFolderModel>();
            var sut = new ServicePanelPresenter(view, model, songSelectedPublisher);
            return new ServicePanelPresenterTestFixture(sut, view, model, songSelectedPublisher);
        }
    }
}
