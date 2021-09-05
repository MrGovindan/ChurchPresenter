using ChurchPresenter.UI.Presenters;
using ChurchPresenter.UI.Services;
using NSubstitute;
using NUnit.Framework;
using System;

namespace ChurchPresenter.UI.Tests
{
    class ImportPresenterTests
    {
        [Test]
        public void GivenImportStartedAndFileOpened_WhenFileSelected_()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testFileName = "c:/documents/test";
            fixture.openFileDialog.Show().Returns(true);
            fixture.openFileDialog.GetFiles().Returns(new string[] { testFileName });

            // Act
            fixture.StartImport();

            // Assert
            fixture.fileImporter.Received().Write(testFileName);
        }

        [Test]
        public void WhenCreatingDialog_CorrectFilterIsSet()
        {
            // Arrange
            var fixture = CreateTestFixture();
            fixture.openFileDialog.Show().Returns(false);

            // Act
            fixture.StartImport();

            // Assert
            Received.InOrder(() =>
            {
                fixture.openFileDialog.SetFilter("OpenLP Service", "osz");
                fixture.openFileDialog.Show();
            });
        }

        [Test]
        public void GivenImportStartedAndNoFileOpened_NothingIsDone()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testFileName = "c:/documents/test";
            fixture.openFileDialog.Show().Returns(false);
            fixture.openFileDialog.GetFiles().Returns(new string[] { testFileName });

            // Act
            fixture.StartImport();

            // Assert
            fixture.fileImporter.Received(0).Write(Arg.Any<string>());
        }

        struct ImportPresenterTestFixture
        {
            internal IImportView view1;
            internal IImportView view2;
            internal IImportView view3;
            internal IOpenFileDialog openFileDialog;
            internal IWriter<string> fileImporter;
            internal ImportPresenter sut;

            public void StartImport()
            {
                switch (new Random().Next(0, 3))
                {
                    case 0:
                        view1.ImportStarted += Raise.Event<Action>();
                        break;
                    case 1:
                        view2.ImportStarted += Raise.Event<Action>();
                        break;
                    case 2:
                        view3.ImportStarted += Raise.Event<Action>();
                        break;
                }
            }
        }

        private static ImportPresenterTestFixture CreateTestFixture()
        {
            var fixture = new ImportPresenterTestFixture();
            fixture.view1 = Substitute.For<IImportView>();
            fixture.view2 = Substitute.For<IImportView>();
            fixture.view3 = Substitute.For<IImportView>();
            fixture.fileImporter = Substitute.For<IWriter<string>>();
            fixture.openFileDialog = Substitute.For<IOpenFileDialog>();
            fixture.sut = new ImportPresenter(new StubDialogFactory(fixture.openFileDialog), fixture.fileImporter, fixture.view1, fixture.view2, fixture.view3);
            return fixture;
        }
    }

    class StubDialogFactory : IDialogFactory
    {
        private readonly IOpenFileDialog openFileDialog;

        public StubDialogFactory(IOpenFileDialog openFileDialog)
        {
            this.openFileDialog = openFileDialog;
        }

        public IOpenFileDialog CreateOpenFileDialog()
        {
            return openFileDialog;
        }
    }
}
