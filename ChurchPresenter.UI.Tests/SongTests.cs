using ChurchPresenter.UI.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests
{
    class SongTests
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
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();

            // Assert
            Assert.That(testSong.slides.Length, Is.EqualTo(4));
        }

        [Test]
        public void WhenCreatedWithXmlLyrics_SlidesContainCorrectData()
        {
            // Act
            var testSong = new SongBuilder().WithLyrics(testLyrics).Build();

            // Assert
            Assert.That(testSong.slides[0].text,
                Is.EqualTo("Bless the Lord, Oh my soul, O my soul\r\nWorship His Holy Name"));
        }
    }
}
