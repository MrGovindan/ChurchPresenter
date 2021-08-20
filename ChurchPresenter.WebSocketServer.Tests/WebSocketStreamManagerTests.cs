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
            var finBit = GetFinBit(memory);
            Assert.That(finBit, Is.True);
        }

        private bool GetFinBit(byte[] memory)
        {
            return (memory[0] >> 7) == 1;
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
            var rsvBits = GetReserveBits(memory);
            Assert.That(rsvBits.Item1, Is.False);
            Assert.That(rsvBits.Item2, Is.False);
            Assert.That(rsvBits.Item3, Is.False);
        }

        private Tuple<bool, bool, bool> GetReserveBits(byte[] memory)
        {
            return new Tuple<bool, bool, bool>(
                (memory[0] & 0b01000000) > 0,
                (memory[0] & 0b00100000) > 0,
                (memory[0] & 0b00010000) > 0);
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
            var opCode = GetOpCode(memory);
            Assert.That(opCode, Is.EqualTo(0x01));
        }

        public int GetOpCode(byte[] memory)
        {
            return memory[0] & 0b00001111;
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
            var maskBit = GetMaskBit(memory);
            Assert.That(maskBit, Is.False);
        }

        public bool GetMaskBit(byte[] memory)
        {
            return (memory[1] >> 7) == 1;
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
            var payloadLength = GetPayloadLength(memory);
            Assert.That(payloadLength, Is.EqualTo(messageLength));
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
            var payloadLength = GetPayloadLength(memory);
            Assert.That(payloadLength, Is.EqualTo(126));
            var extendedPayloadBytes = memory.AsSpan(2, 2).ToArray();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(extendedPayloadBytes);

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
            var payloadLength = GetPayloadLength(memory);
            Assert.That(payloadLength, Is.EqualTo(127));
            var extendedPayloadBytes = memory.AsSpan(2, 8).ToArray();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(extendedPayloadBytes);
            
            Assert.That(BitConverter.ToUInt64(extendedPayloadBytes), Is.EqualTo(messageLength));
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
            var payloadLength = GetPayloadLength(memory);
            Assert.That(payloadLength, Is.EqualTo(expectedMessageLength));
            var decodedPayload = Encoding.UTF8.GetString(memory.AsSpan(2, expectedMessageLength));
            Assert.That(decodedPayload, Is.EqualTo(message));
        }

        private static string CreateStringOfLength(int length)
        {
            return new string('_', length);
        }

        private static int GetPayloadLength(byte[] memory)
        {
            return memory[1] & 0x7F;
        }
    }
}