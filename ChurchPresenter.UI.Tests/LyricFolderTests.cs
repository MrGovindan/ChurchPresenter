using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests
{
    class LyricFolderTests
    {
        private static string testLyrics = @"<?xml version='1.0' encoding='UTF-8'?>
<song version='1.0'>
    <lyrics>
        <verse label='1' type='c'>
            <![CDATA[Bless the Lord, Oh my soul, O my soul
Worship His Holy Name]]>
        </verse>
        <verse label='1' type='c'>
            <![CDATA[Sing like never before, O my soul
I'll worship your Holy Name]]>
        </verse>
        <verse label='1' type='v'>
            <![CDATA[The sun comes up, It's a new day dawning
It's time to sing Your song again]]>
        </verse>
        <verse label='1' type='v'>
            <![CDATA[Whatever may pass and whatever lies before me
Let me be singing when the evening comes]]>
        </verse>
    </lyrics>
</song>";

        [Test]
        public void WhenCreatedWithXmlLyrics_AllSlidesAreGenerated()
        {
            // Act
            var testLyricFolder = new LyricFolderBuilder().WithLyrics(testLyrics).Build();

            // Assert
            Assert.That(testLyricFolder.GetSlides().Length, Is.EqualTo(4));
        }

        [Test]
        public void WhenCreatedWithXmlLyrics_SlidesContainCorrectData()
        {
            // Act
            var testLyricFolder = new LyricFolderBuilder().WithLyrics(testLyrics).Build();

            // Assert
            Assert.That(testLyricFolder.GetSlides()[0].ToString(),
                Is.EqualTo("Bless the Lord, Oh my soul, O my soul\r\nWorship His Holy Name"));
        }

        [Test]
        public void ReturnsASongFolderType()
        {
            // Act
            var testLyricFolder = new LyricFolderBuilder().WithLyrics(testLyrics).Build();

            // Assert
            Assert.That(testLyricFolder.GetFolderType(), Is.EqualTo(FolderType.Lyric));
        }
    }
}
