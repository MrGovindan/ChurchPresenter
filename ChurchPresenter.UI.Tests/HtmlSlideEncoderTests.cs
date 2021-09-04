using ChurchPresenter.UI.Models.Folder;
using ChurchPresenter.UI.Services.SlideEncoder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests
{
    class HtmlSlideEncoderTests
    {
        [TestCase("Foo")]
        [TestCase("Bar")]
        [TestCase("Baz")]
        [Test]
        public void GivenASinglePartSlide_OutputHtmlIsTheSame(string part)
        {
            // Arrange
            var slide = new Slide(part);

            // Act
            var actual = new HtmlSlideEncoder().Encode(slide);

            // Assert
            Assert.That(actual, Is.EqualTo(part));
        }

        [Test]
        public void GivenAMultiPartSlide_OutputContainsConcatenatedText()
        {
            // Arrange
            var parts = new List<TextPart>();
            parts.Add(TextPart.AsNormal("Foo"));
            parts.Add(TextPart.AsNormal("Bar"));
            var slide = new Slide(parts, "");

            // Act
            var actual = new HtmlSlideEncoder().Encode(slide);

            // Assert
            Assert.That(actual, Is.EqualTo("FooBar"));
        }

        [Test]
        public void GivenAMultiPartSlideWithSuperscriptPart_OutputContainsTextWithSupTag()
        {
            // Arrange
            var parts = new List<TextPart>();
            parts.Add(TextPart.AsNormal("Foo"));
            parts.Add(TextPart.AsSuperscript("Bar"));
            var slide = new Slide(parts, "");

            // Act
            var actual = new HtmlSlideEncoder().Encode(slide);

            // Assert
            Assert.That(actual, Is.EqualTo("Foo<sup>Bar</sup>"));
        }

        [Test]
        public void GivenAMultiPartSlideWithSuperscriptPartFirst_OutputContainsTextWithSupTag()
        {
            // Arrange
            var parts = new List<TextPart>();
            parts.Add(TextPart.AsSuperscript("Foo"));
            parts.Add(TextPart.AsNormal("Bar"));
            var slide = new Slide(parts, "");

            // Act
            var actual = new HtmlSlideEncoder().Encode(slide);

            // Assert
            Assert.That(actual, Is.EqualTo("<sup>Foo</sup>Bar"));
        }

        [Test]
        public void GivenASlideWithQuotes_OutputHasQuotesReplacedWithHtmlCode()
        {
            // Arrange
            var slide = new Slide("\"test\"");

            // Act
            var actual = new HtmlSlideEncoder().Encode(slide);

            // Assert
            Assert.That(actual, Is.EqualTo("&quot;test&quot;"));
        }
    }
}
