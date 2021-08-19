using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChurchPresenter.WebSocketServer.Tests
{
    public class WebSocketStreamManagerTests
    {
        [Test]
        public async Task whenWritingAString_messageContainsFinBit()
        {
            // Arrange
            var memStream = new MemoryStream(100);
            WebSocketStreamManager sut = new WebSocketStreamManager(memStream);

            // Act
            await sut.WriteString("test");

            // Assert
            var memory = memStream.ToArray();
            Assert.That(memory[0] & 0b0001, Is.EqualTo(1));
        }

        [Test]
        public async Task whenWritingAString_RsvBitsAreLeftAs0()
        {
            // Arrange
            var memStream = new MemoryStream(100);
            WebSocketStreamManager sut = new WebSocketStreamManager(memStream);

            // Act
            await sut.WriteString("test");


            // Assert
            var memory = memStream.ToArray();
            Assert.That(memory[0] & 0b1110, Is.EqualTo(0b0000));
        }

        [Test]
        public async Task whenWritingAString_FrameOpCodeIsSetToStringAsync()
        {
            // Arrange
            var memStream = new MemoryStream(100);
            WebSocketStreamManager sut = new WebSocketStreamManager(memStream);

            // Act
            await sut.WriteString("test");

            // Assert
            var memory = memStream.ToArray();
            Assert.That((memory[0] >> 4) & 0b1111, Is.EqualTo(0x01));
        }

        [Test]
        public async Task whenWritingAString_MaskBitIsNotSet()
        {
            // Arrange
            var memStream = new MemoryStream(100);
            WebSocketStreamManager sut = new WebSocketStreamManager(memStream);

            // Act
            await sut.WriteString("test");

            // Assert
            var memory = memStream.ToArray();
            Assert.That(memory[1] >> 7, Is.EqualTo(0));
        }

        [Test]
        public async Task whenWritingAString_MaskKeyIsNotSet()
        {
            // Arrange
            var memStream = new MemoryStream(100);
            WebSocketStreamManager sut = new WebSocketStreamManager(memStream);

            // Act
            await sut.WriteString("test");

            // Assert
            var memory = memStream.ToArray();
            Assert.That(memory[2], Is.EqualTo(0));
            Assert.That(memory[3], Is.EqualTo(0));
            Assert.That(memory[4], Is.EqualTo(0));
            Assert.That(memory[5], Is.EqualTo(0));
        }

        [TestCase(4)]
        [TestCase(64)]
        [TestCase(125)]
        [Test]
        public async Task whenWritingAShortString_PayloadLengthIsMessageLength(int messageLength)
        {
            // Arrange
            var memStream = new MemoryStream(100);
            WebSocketStreamManager sut = new WebSocketStreamManager(memStream);
            var message = CreateStringOfLength(messageLength);

            // Act
            await sut.WriteString(message);

            // Assert
            var memory = memStream.ToArray();
            Assert.That(memory[1], Is.EqualTo(messageLength));
        }

        [TestCase(126)]
        [TestCase(10000)]
        [TestCase(65535)]
        [Test]
        public async Task whenWritingAMediumString_PayloadLengthIs126AndExtendedPayloadContainsMessageLength(int messageLength)
        {
            // Arrange
            var memStream = new MemoryStream(100);
            WebSocketStreamManager sut = new WebSocketStreamManager(memStream);
            var message = CreateStringOfLength(messageLength);

            // Act
            await sut.WriteString(message);

            // Assert
            var memory = memStream.ToArray();
            Assert.That(memory[1], Is.EqualTo(126));
            var extendedPayloadBytes = memory.AsSpan(2, 2).ToArray();
            Assert.That(BitConverter.ToUInt16(extendedPayloadBytes), Is.EqualTo(messageLength));
        }

        [TestCase(65536)]
        [TestCase(66000)]
        [TestCase(80000)]
        [Test]
        public async Task whenWritingALongString_PayloadLengthIs127AndExtendedPayloadContainsMessageLength(int messageLength)
        {
            // Arrange
            var memStream = new MemoryStream(100);
            WebSocketStreamManager sut = new WebSocketStreamManager(memStream);
            var message = CreateStringOfLength(messageLength);

            // Act
            await sut.WriteString(message);

            // Assert
            var memory = memStream.ToArray();
            Assert.That(memory[1], Is.EqualTo(127));
            var extendedPayloadBytes = memory.AsSpan(2, 4).ToArray();
            Assert.That(BitConverter.ToUInt32(extendedPayloadBytes), Is.EqualTo(messageLength));
        }

        [Test]
        public async Task whenAStringWithNonAsciiCharacters_PayloadContainsUt8EncodedText()
        {
            // Arrange
            var memStream = new MemoryStream(100);
            WebSocketStreamManager sut = new WebSocketStreamManager(memStream);
            var message = "κόσμε";
            var expectedMessageLength = 11;

            // Act
            await sut.WriteString(message);

            // Assert
            var memory = memStream.ToArray();
            Assert.That(memory[1], Is.EqualTo(expectedMessageLength));
            var decodedPayload = Encoding.UTF8.GetString(memory.AsSpan(6, 11));
            Assert.That(decodedPayload, Is.EqualTo(message));
        }

        private static string CreateStringOfLength(int length)
        {
            return new string('_', length);
        }
    }
}