using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using ChurchPresenter.UI.Services;
using ChurchPresenter.UI.Services.SlideEncoder;
using ChurchPresenter.UI.Tests.Builders;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;

namespace ChurchPresenter.UI.Tests
{
    class WebBrowserProjectorTests
    {
        [Test]
        public void WhenASlideIsSelected_DataIsWrittenToOutput()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testSlide = new TestSlideBuilder().WithNSlides(3).Build();
            JObject expectedJson = new JObject();
            expectedJson["text"] = "text 0";
            expectedJson["caption"] = "caption 0";

            // Act
            fixture.displayedSlideService.DisplayedSlideChanged += Raise.Event<Action<Slide>>(testSlide[0]);

            // Assert
            fixture.outputWriter.Received().Write(Arg.Is(expectedJson.ToString()));
        }

        [Test]
        public void WhenSlideVisibilityIsHidden_OutputShowsNothing()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testSlide = new TestSlideBuilder().WithSlide("test").Build();
            JObject expectedJson = new JObject();
            expectedJson["text"] = "";
            expectedJson["caption"] = "";

            fixture.displayedSlideService.DisplayedSlideChanged += Raise.Event<Action<Slide>>(testSlide[0]);

            // Act
            fixture.slideVisibilityModel.SlideVisibilityChanged += Raise.Event<Action<bool>>(false);

            // Assert
            fixture.outputWriter.Received().Write(Arg.Is(expectedJson.ToString()));
        }

        [Test]
        public void GivenHiddenSlide_WhenSlideIsMadeVisible_OutputShowsPreviousData()
        {
            // Arrange
            var fixture = CreateTestFixture();
            var testSlide = new TestSlideBuilder().WithSlide("test").Build();
            JObject expectedJson = new JObject();
            expectedJson["text"] = "test";
            expectedJson["caption"] = "";

            fixture.slideVisibilityModel.SlideVisibilityChanged += Raise.Event<Action<bool>>(false);
            fixture.displayedSlideService.DisplayedSlideChanged += Raise.Event<Action<Slide>>(testSlide[0]);

            // Act
            fixture.slideVisibilityModel.SlideVisibilityChanged += Raise.Event<Action<bool>>(true);

            // Assert
            fixture.outputWriter.Received().Write(Arg.Is(expectedJson.ToString()));
        }

        class BasicSlideEncoder : ISlideEncoder
        {
            public string Encode(Slide slide)
            {
                return string.Join(" ", slide.GetParts().Select(p => p.Text));
            }
        }

        struct WebBrowserProjectorTestFixture
        {
            internal IDisplayedSlideService displayedSlideService;
            internal ISlideVisibilityModel slideVisibilityModel;
            internal IWriter<string> outputWriter;
            internal WebBrowserProjector sut;
        }

        private WebBrowserProjectorTestFixture CreateTestFixture()
        {
            var fixture = new WebBrowserProjectorTestFixture();
            fixture.displayedSlideService = Substitute.For<IDisplayedSlideService>();
            fixture.slideVisibilityModel = Substitute.For<ISlideVisibilityModel>();
            fixture.outputWriter = Substitute.For<IWriter<string>>();
            fixture.sut = new WebBrowserProjector(fixture.displayedSlideService, fixture.slideVisibilityModel, new BasicSlideEncoder(), fixture.outputWriter);
            return fixture;
        }
    }
}
