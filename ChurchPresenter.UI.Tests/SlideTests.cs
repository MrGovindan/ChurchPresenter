using ChurchPresenter.UI.Models.Folder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests
{
    class SlideTests
    {
        [Test]
        public void WhenCreatedWithJustText_SlideContainsASinglePartAndNoCaption()
        {
            // Arrange
            // Act
            var slide = new Slide("Test text");

            // Assert
            Assert.That(slide.GetParts().Count, Is.EqualTo(1));
            Assert.That(slide.GetParts()[0].Type, Is.EqualTo(TextType.Normal));
            Assert.That(slide.GetParts()[0].Text, Is.EqualTo("Test text"));
            Assert.That(slide.GetCaption(), Is.EqualTo(""));
        }

        [Test]
        public void WhenCreatedWithTextAndCaption_SlideContainsASinglePartAndCaption()
        {
            // Arrange
            // Act
            var slide = new Slide("Test text", "Test caption");

            // Assert
            Assert.That(slide.GetParts().Count, Is.EqualTo(1));
            Assert.That(slide.GetParts()[0].Type, Is.EqualTo(TextType.Normal));
            Assert.That(slide.GetParts()[0].Text, Is.EqualTo("Test text"));
            Assert.That(slide.GetCaption(), Is.EqualTo("Test caption"));
        }
        
        [Test]
        public void Whe()
        {
            // Arrange
            var parts = new List<TextPart>();
            parts.Add(new TextPart("test1", TextType.Normal));
            parts.Add(new TextPart("test2", TextType.Superscript));

            // Act
            var slide = new Slide(parts, "Test caption");

            // Assert
            Assert.That(slide.GetParts().Count, Is.EqualTo(2));
            Assert.That(slide.GetParts()[0].Type, Is.EqualTo(TextType.Normal));
            Assert.That(slide.GetParts()[1].Type, Is.EqualTo(TextType.Superscript));
        }
    }
}
