using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChurchPresenter.WebSocketServer
{
    internal class WebSocketStreamManager
    {
        private Stream stream;

        public WebSocketStreamManager(Stream stream)
        {
            this.stream = stream;
        }

        public async Task WriteString(string message)
        {
            var frame = new WebSocketFrameBuilder().withStringPayload(message).build();
            await stream.WriteAsync(frame, 0, frame.Length);
        }

        internal class WebSocketFrameBuilder
        {
            private byte[] payload;
            private byte opcode = 0;

            public WebSocketFrameBuilder()
            {
            }

            public WebSocketFrameBuilder withStringPayload(string message)
            {
                opcode = 0b00010001;
                payload = Encoding.UTF8.GetBytes(message);
                return this;
            }

            public byte[] build()
            {
                var data = new List<byte>();
                data.Add(opcode);
                data.AddRange(GetPayloadLengthBytes());
                data.AddRange(GetMaskingKey());
                data.AddRange(payload);
                return data.ToArray();
            }

            private static byte[] GetMaskingKey()
            {
                return new byte[4];
            }

            private IEnumerable<byte> GetPayloadLengthBytes()
            {
                var data = new List<byte>();
                if (payload.Length < 126)
                {
                    data.Add((byte)payload.Length);
                }
                else if (payload.Length < 65536)
                {
                    data.Add(126);
                    var mem = new byte[2];
                    BitConverter.TryWriteBytes(mem, (ushort)payload.Length);
                    data.AddRange(mem);
                }
                else
                {
                    data.Add(127);
                    var mem = new byte[4];
                    BitConverter.TryWriteBytes(mem, (uint)payload.Length);
                    data.AddRange(mem);
                }

                return data;
            }
        }
    }

}
