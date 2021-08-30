using ChurchPresenter.UI.Presenters;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests
{
    class MainWindowPresenterTests
    {
        [Test]
        public void WhenPresenterCreated_ViewIsArrangedForSetup()
        {
            // Arrange
            // Act
            var fixture = CreateTestFixture();

            // Arrange
            fixture.view.Received().ArrangeInMode(WindowMode.SETUP);
        }

        [Test]
        public void WhenLiveModeIsActivated_ViewIsSetToLive()
        {
            // Arrange
            var fixture = CreateTestFixture();

            // Act
            fixture.view.LiveViewSelected += Raise.Event<Action>();

            // Arrange
            fixture.view.Received().ArrangeInMode(WindowMode.LIVE);
        }

        [Test]
        public void WhenSetupModeIsActivated_ViewIsSetToSetup()
        {
            // Arrange
            var fixture = CreateTestFixture();

            // Act
            fixture.view.SetupViewSelected += Raise.Event<Action>();

            // Arrange
            fixture.view.Received(2).ArrangeInMode(WindowMode.SETUP);
        }

        class MainWindowPresenterTestFixture
        {
            internal IMainWindow view;
            internal MainWindowPresenter sut;
        }

        private MainWindowPresenterTestFixture CreateTestFixture()
        {
            var fixture = new MainWindowPresenterTestFixture();
            fixture.view = Substitute.For<IMainWindow>();
            fixture.sut = new MainWindowPresenter(fixture.view);
            return fixture;
        }
    }
}
