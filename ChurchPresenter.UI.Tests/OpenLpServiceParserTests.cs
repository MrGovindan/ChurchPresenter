using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests
{
    class OpenLpServiceParserTests
    {
        string testData;

        [OneTimeSetUp]
        public void ReadTestFile()
        {
            // Live Unit Testing
            // testData = System.IO.File.ReadAllText("..\\..\\..\\..\\..\\..\\..\\..\\..\\ChurchPresenter.UI.Tests\\Resources\\test.osj");
            // Once off testing
            testData = System.IO.File.ReadAllText("..\\..\\..\\..\\ChurchPresenter.UI.Tests\\Resources\\test.osj");
        }

        [Test]
        public void EnsureTestDataIsLoaded()
        {
            Assert.That(testData, Is.Not.Null);
        }

        [Test]
        public void WhenParsingTestData_Returns2Items()
        {
            // Arrange
            var parser = CreateParser();

            // Act
            var actual = parser.Parse(testData);

            // Assert
            Assert.That(actual.Length, Is.EqualTo(2));
        }
        
        [Test]
        public void WhenParsingTestData_ReturnsDataWithCorrectTypes()
        {
            // Arrange
            var parser = CreateParser();

            // Act
            var actual = parser.Parse(testData);

            // Assert
            Assert.That(actual[0].GetFolderType(), Is.EqualTo(FolderType.Lyric));
            Assert.That(actual[1].GetFolderType(), Is.EqualTo(FolderType.Scripture));
        }

        [Test]
        public void WhenParsingTestData_ReturnsCorrectNumberOfLyricSlides()
        {
            // Arrange
            var parser = CreateParser();

            // Act
            var actual = parser.Parse(testData)[0];

            // Assert
            Assert.That(actual.GetSlides().Length, Is.EqualTo(17));
        }

        [Test]
        public void WhenParsingTestData_ReturnsCorrectLyricData()
        {
            // Arrange
            var parser = CreateParser();

            // Act
            var actual = parser.Parse(testData)[0];

            // Assert
            Assert.That(actual.GetTitle(), Is.EqualTo("Oceans - Where feet may fail"));
            Assert.That(actual.GetSlides()[0].ToString(), Is.EqualTo("You call me out upon the waters\nThe great unknown where feet may fail"));
            Assert.That(actual.GetSlides()[0].GetCaption(), Is.EqualTo("Oceans - Where feet may fail"));
        }

        [Test]
        public void WhenParsingTestData_ReturnsCorrectScriptureData()
        {
            // Arrange
            var parser = CreateParser();

            // Act
            var actual = parser.Parse(testData)[1];

            // Assert
            Assert.That(actual.GetTitle(), Is.EqualTo("Matthew 4:9-14 NKJV"));
            Assert.That(actual.GetSlides()[0].ToString(), Is.EqualTo("4:9 And he said to Him, \"All these things I will give You if You will fall down and worship me.\""));
        }

        private static OpenLpServiceParser CreateParser()
        {
            return new OpenLpServiceParser();
        }
    }
}
