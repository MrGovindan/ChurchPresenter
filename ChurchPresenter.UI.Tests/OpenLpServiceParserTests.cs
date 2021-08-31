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
            Assert.That(actual.Length, Is.EqualTo(15));
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
            Assert.That(actual[1].GetFolderType(), Is.EqualTo(FolderType.Lyric));
            Assert.That(actual[2].GetFolderType(), Is.EqualTo(FolderType.Lyric));
            Assert.That(actual[3].GetFolderType(), Is.EqualTo(FolderType.Scripture));
            Assert.That(actual[4].GetFolderType(), Is.EqualTo(FolderType.Scripture));
            Assert.That(actual[5].GetFolderType(), Is.EqualTo(FolderType.Scripture));
            Assert.That(actual[6].GetFolderType(), Is.EqualTo(FolderType.Scripture));
            Assert.That(actual[14].GetFolderType(), Is.EqualTo(FolderType.Scripture));
        }

        [Test]
        public void WhenParsingTestData_ReturnsCorrectNumberOfLyricSlides()
        {
            // Arrange
            var parser = CreateParser();

            // Act
            var actual = parser.Parse(testData)[0];

            // Assert
            Assert.That(actual.GetSlides().Length, Is.EqualTo(32));
        }

        [Test]
        public void WhenParsingTestData_ReturnsCorrectLyricData()
        {
            // Arrange
            var parser = CreateParser();

            // Act
            var actual = parser.Parse(testData)[0];

            // Assert
            Assert.That(actual.GetTitle(), Is.EqualTo("Mighty To Save"));
            Assert.That(actual.GetSlides()[0].ToString(), Is.EqualTo("Everyone needs compassion\nLove that's never failing"));
            Assert.That(actual.GetSlides()[0].GetCaption(), Is.EqualTo("Mighty To Save"));
        }

        [Test]
        public void WhenParsingTestData_ReturnsCorrectScriptureData()
        {
            // Arrange
            var parser = CreateParser();

            // Act
            var actual = parser.Parse(testData)[3];

            // Assert
            Assert.That(actual.GetTitle(), Is.EqualTo("1 Kings 19:2-3 CEV"));
            Assert.That(actual.GetSlides().Length, Is.EqualTo(2));
            Assert.That(actual.GetSlides()[0].ToString(), Is.EqualTo("19:2 She sent a message to Elijah: \"You killed my prophets. Now I'm going to kill you! I pray that the gods will punish me even more severely if I don't do it by this time tomorrow.\""));
            Assert.That(actual.GetSlides()[1].ToString(), Is.EqualTo("19:3 Elijah was afraid when he got her message, and he ran to the town of Beersheba in Judah. He left his servant there,"));
        }

        private static OpenLpServiceParser CreateParser()
        {
            return new OpenLpServiceParser();
        }
    }
}
