using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.WebSocketServer
{
    internal class WebSocketFrameBuilder
    {
        private byte[] payload;
        private byte opcode = 0;

        public WebSocketFrameBuilder()
        {
        }

        public WebSocketFrameBuilder withStringPayload(string message)
        {
            opcode = 0b10000001;
            var data = new List<byte>(Encoding.UTF8.GetBytes(message));
            payload = data.ToArray();
            return this;
        }

        public byte[] build()
        {
            var data = new List<byte>();
            data.Add(opcode);
            data.AddRange(GetPayloadLengthBytes());
            data.AddRange(payload);
            return data.ToArray();
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

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(mem);

                data.AddRange(mem);
            }
            else
            {
                data.Add(127);
                var mem = new byte[8];
                BitConverter.TryWriteBytes(mem, (ulong)payload.Length);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(mem);

                data.AddRange(mem);
            }

            return data;
        }
    }
}
