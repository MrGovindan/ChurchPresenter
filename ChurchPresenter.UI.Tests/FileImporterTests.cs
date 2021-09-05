using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using ChurchPresenter.UI.Services;
using ChurchPresenter.UI.Services.Import;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests
{
    class FileImporterTests
    {
        [Test]
        public void GivenAFile_FileIsExtracted_AndFoldersAreAddedToServiceModel()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var sut = fixture.sut;
            var folders = CreateTestFolders(4);
            fixture.fileSystem.ExtractAndReadFileFromArchive(Arg.Any<string>()).Returns("test");
            fixture.parser.Parse("test").Returns(folders);

            // Act
            sut.Write("testFilename");

            // Assert
            Received.InOrder(() =>
            {
                fixture.serviceModel.ClearService();
                fixture.serviceModel.AddFolder(folders[0]);
                fixture.serviceModel.AddFolder(folders[1]);
                fixture.serviceModel.AddFolder(folders[2]);
                fixture.serviceModel.AddFolder(folders[3]);
            });
        }

        struct FileImporterTestFixture
        {
            internal FileImporter sut;
            internal IOpenLpServiceParser parser;
            internal IFileSystem fileSystem;
            internal IServiceModel serviceModel;
        }

        private FileImporterTestFixture CreateTestFixture()
        {
            var fixture = new FileImporterTestFixture();
            fixture.parser = Substitute.For<IOpenLpServiceParser>();
            fixture.fileSystem = Substitute.For<IFileSystem>();
            fixture.serviceModel = Substitute.For<IServiceModel>();
            fixture.sut = new FileImporter(fixture.fileSystem, fixture.parser, fixture.serviceModel);
            return fixture;
        }

        private static IFolder[] CreateTestFolders(int total)
        {
            var folders = new IFolder[total];
            for (int i = 0; i < total; ++i)
            {
                folders[i] = new LyricFolderBuilder().Build();
            }
            return folders;
        }
    }
}
