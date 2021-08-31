using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using ChurchPresenter.UI.Presenters;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChurchPresenter.UI.Tests
{
    class ServicePanelPresenterTests
    {
        [Test]
        public void WhenAFolderIsAddedToTheService_ViewIsGivenANewEntry()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testFolder = new LyricFolderBuilder().WithTitle("TestTitle").Build();

            // Act
            fixture.serviceModel.AddFolder(testFolder);

            // Assert
            fixture.view.Received().AddFolder(testFolder);
        }

        [Test]
        public void WhenAFolderIsSelectedAndActivated_SelectedFolderModelIsNotified()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testFolder1 = new LyricFolderBuilder().WithTitle("TestTitle1").Build();
            var testFolder2 = new LyricFolderBuilder().WithTitle("TestTitle2").Build();
            fixture.serviceModel.AddFolder(testFolder1);
            fixture.serviceModel.AddFolder(testFolder2);
            fixture.view.FolderSelected += Raise.Event<Action<int>>(1);

            // Act
            fixture.view.FolderActivated += Raise.Event<Action>();

            // Assert
            fixture.selectedFolderModel.Received().PublishSelectedFolder(testFolder2);
        }

        [Test]
        public void WhenAFolderIsSelected_SelectedFolderModelIsNotNotified()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testFolder1 = new LyricFolderBuilder().WithTitle("TestTitle1").Build();
            var testFolder2 = new LyricFolderBuilder().WithTitle("TestTitle2").Build();
            fixture.serviceModel.AddFolder(testFolder1);
            fixture.serviceModel.AddFolder(testFolder2);

            // Act
            fixture.view.FolderSelected += Raise.Event<Action<int>>(1);

            // Assert
            fixture.selectedFolderModel.Received(0).PublishSelectedFolder(Arg.Any<IFolder>());
        }

        [Test]
        public void WhenAnEntryIsRemovedFromService_ViewHasItemRemoved()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testFolder1 = new LyricFolderBuilder().WithTitle("TestTitle1").Build();
            var testFolder2 = new LyricFolderBuilder().WithTitle("TestTitle2").Build();
            fixture.serviceModel.AddFolder(testFolder1);
            fixture.serviceModel.AddFolder(testFolder2);

            // Act
            fixture.view.FolderRemoved += Raise.Event<Action<int>>(0);

            // Assert
            fixture.view.Received().RemoveFolder(0);
        }

        [Test]
        public void WhenAFolderIsRemovedByTheView_ModelRemovesFolder()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testFolder1 = new LyricFolderBuilder().Build();
            var testFolder2 = new LyricFolderBuilder().Build();
            fixture.serviceModel.AddFolder(testFolder1);
            fixture.serviceModel.AddFolder(testFolder2);

            // Act
            fixture.view.FolderRemoved += Raise.Event<Action<int>>(0);

            // Assert
            Assert.That(fixture.serviceModel.ItemAt(0), Is.EqualTo(testFolder2));
        }

        [Test]
        public void GivenASelectedFolder_WhenAFolderIsShiftedUp_TheServiceModelIsUpdated()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var folders = CreateTestFolders(3);
            foreach (var folder in folders) fixture.serviceModel.AddFolder(folder);
            fixture.view.FolderSelected += Raise.Event<Action<int>>(2);

            // Act
            fixture.view.FolderShitedUp += Raise.Event<Action>();

            // Arrange
            var actual = fixture.serviceModel.GetFolders();
            CollectionAssert.AreEqual(new IFolder[] { folders[0], folders[2], folders[1] }, actual);
        }

        [Test]
        public void GivenASelectedFolder_WhenAFolderIsMadeTopmost_TheServiceModelIsUpdated()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var folders = CreateTestFolders(3);
            foreach (var folder in folders) fixture.serviceModel.AddFolder(folder);
            fixture.view.FolderSelected += Raise.Event<Action<int>>(2);

            // Act
            fixture.view.FolderMadeTopmost += Raise.Event<Action>();

            // Arrange
            var actual = fixture.serviceModel.GetFolders();
            CollectionAssert.AreEqual(new IFolder[] { folders[2], folders[0], folders[1] }, actual);
        }

        [Test]
        public void GivenASelectedFolder_WhenAFolderIsMadeBottommost_TheServiceModelIsUpdated()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var folders = CreateTestFolders(3);
            foreach (var folder in folders) fixture.serviceModel.AddFolder(folder);
            fixture.view.FolderSelected += Raise.Event<Action<int>>(0);

            // Act
            fixture.view.FolderMadeBottommost += Raise.Event<Action>();

            // Arrange
            var actual = fixture.serviceModel.GetFolders();
            CollectionAssert.AreEqual(new IFolder[] { folders[1], folders[2], folders[0] }, actual);
        }

        [Test]
        public void GivenASetOfFoldersWithOneSelected_WhenAFolderIsShiftedDown_TheServiceModelIsUpdated()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var folders = CreateTestFolders(3);
            foreach (var folder in folders) fixture.serviceModel.AddFolder(folder);
            fixture.view.FolderSelected += Raise.Event<Action<int>>(1);

            // Act
            fixture.view.FolderShitedDown += Raise.Event<Action>();

            // Arrange
            var actual = fixture.serviceModel.GetFolders();
            CollectionAssert.AreEqual(new IFolder[] { folders[0], folders[2], folders[1] }, actual);
        }

        [Test]
        public void GivenASetOfFoldersWithTopmostSelected_WhenThatFolderIsShiftedUp_NoChangesToServiceOrderAreMade()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var folders = CreateTestFolders(3);
            foreach (var folder in folders) fixture.serviceModel.AddFolder(folder);
            fixture.view.FolderSelected += Raise.Event<Action<int>>(0);

            // Act
            fixture.view.FolderShitedUp += Raise.Event<Action>();

            // Arrange
            var actual = fixture.serviceModel.GetFolders();
            CollectionAssert.AreEqual(new IFolder[] { folders[0], folders[1], folders[2] }, actual);
        }

        [Test]
        public void GivenASetOfFoldersWithBottommostSelected_WhenThatFolderIsShiftedDown_NoChangesToServiceOrderAreMade()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var folders = CreateTestFolders(3);
            foreach (var folder in folders) fixture.serviceModel.AddFolder(folder);
            fixture.view.FolderSelected += Raise.Event<Action<int>>(2);

            // Act
            fixture.view.FolderShitedDown += Raise.Event<Action>();

            // Arrange
            var actual = fixture.serviceModel.GetFolders();
            CollectionAssert.AreEqual(new IFolder[] { folders[0], folders[1], folders[2] }, actual);
        }

        [Test]
        public void WhenTheSelectedFolderIsBottommost_ShiftDownIsDisabled()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var folders = CreateTestFolders(3);

            foreach (var folder in folders) fixture.serviceModel.AddFolder(folder);

            // Act
            fixture.view.FolderSelected += Raise.Event<Action<int>>(2);

            // Assert
            fixture.view.Received().EnableShiftDown(false);
        }

        [TestCase(4, 2)]
        [TestCase(3, 1)]
        [Test]
        public void WhenTheSelectedFolderIsInTheMiddleOfTheList_ShiftDownAndUpAreEnabled(int folderCount, int selectedIndex)
        {
            // Arrange
            var fixture = CreateTestFixture();
            var folders = CreateTestFolders(folderCount);
            foreach (var folder in folders) fixture.serviceModel.AddFolder(folder);

            // Act
            fixture.view.FolderSelected += Raise.Event<Action<int>>(selectedIndex);

            // Assert
            fixture.view.Received().EnableShiftUp(true);
            fixture.view.Received().EnableShiftDown(true);
        }

        [TestFixture]
        public class TestServiceReordering
        {
            private static IEnumerable<Tuple<Action<IServiceModel>, int[]>> testCases
            {
                get
                {
                    yield return Tuple.Create<Action<IServiceModel>, int[]>(m => m.MakeFolderFirst(1), new int[]{ 1, 0, 2});
                    yield return Tuple.Create<Action<IServiceModel>, int[]>(m => m.MakeFolderLast(1), new int[]{ 0, 2, 1});
                    yield return Tuple.Create<Action<IServiceModel>, int[]>(m => m.SwapFolderOrder(2, 0), new int[]{ 2, 1, 0});
                }
            }

            [TestCaseSource(nameof(testCases))]
            public void WhenServiceReordered_ViewIsUpdated(Tuple<Action<IServiceModel>, int[]> reordering)
            {
                // Arrange
                var fixture = CreateTestFixture();
                var folders = CreateTestFolders(3);
                foreach (var folder in folders) fixture.serviceModel.AddFolder(folder);

                // Act
                int[] updatedOrder = null;
                fixture.view.When(v => v.UpdateServiceOrder(Arg.Any<int[]>())).Do(arg => updatedOrder = arg.Arg<int[]>());
                reordering.Item1.Invoke(fixture.serviceModel);

                // Arrange
                CollectionAssert.AreEqual(reordering.Item2, updatedOrder);
            }
        }

        [Test]
        public void WhenPresenterCreated_ViewHasShiftUpAndDownDisabled()
        {
            // Arrange
            // Act
            var fixture = CreateTestFixture();

            // Assert
            fixture.view.Received().EnableShiftDown(false);
            fixture.view.Received().EnableShiftUp(false);
        }

        // Test view selected index updated after reorder;

        private static IFolder[] CreateTestFolders(int total)
        {
            var folders = new IFolder[total];
            for (int i = 0; i < total; ++i)
            {
                folders[i] = new LyricFolderBuilder().Build();
            }
            return folders;
        }

        struct ServicePanelPresenterTestFixture
        {
            public readonly ServicePanelPresenter sut;
            public readonly IServicePanelView view;
            public readonly IServiceModel serviceModel;
            public readonly ISelectedFolderModel selectedFolderModel;

            public ServicePanelPresenterTestFixture(
                ServicePanelPresenter sut,
                IServicePanelView view,
                IServiceModel model,
                ISelectedFolderModel selectedFolderModel)
            {
                this.sut = sut;
                this.view = view;
                this.serviceModel = model;
                this.selectedFolderModel = selectedFolderModel;
            }
        }

        private static ServicePanelPresenterTestFixture CreateTestFixture()
        {
            var serviceModel = new ServiceModel();
            var view = Substitute.For<IServicePanelView>();
            var selectedFolderModel = Substitute.For<ISelectedFolderModel>();
            var sut = new ServicePanelPresenter(view, serviceModel, selectedFolderModel);
            return new ServicePanelPresenterTestFixture(sut, view, serviceModel, selectedFolderModel);
        }
    }
}
