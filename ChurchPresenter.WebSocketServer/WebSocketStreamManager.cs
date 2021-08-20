using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChurchPresenter.WebSocketServer
{
    internal class WebSocketStreamManager
    {
        private Stream stream;

        public WebSocketStreamManager(Stream stream)
        {
            this.stream = stream;
            StartReading();
        }

        private void StartReading()
        {
            var data = new byte[3];
            stream.ReadAsync(data, 0, data.Length).ContinueWith(HandleReadComplete, data);
        }

        private void HandleReadComplete(Task<int> readTask, object buffer)
        {
            if (readTask.IsCompletedSuccessfully)
            {
                var data = new List<byte>();
                data.AddRange((byte[])buffer);
                var mem = new byte[65536];
                stream.Read(mem, 0, mem.Length);
                data.AddRange(mem);

                string s = Encoding.UTF8.GetString(data.ToArray());
                if (Regex.IsMatch(s, "^GET", RegexOptions.IgnoreCase))
                {
                    // 1. Obtain the value of the "Sec-WebSocket-Key" request header without any leading or trailing whitespace
                    // 2. Concatenate it with "258EAFA5-E914-47DA-95CA-C5AB0DC85B11" (a special GUID specified by RFC 6455)
                    // 3. Compute SHA-1 and Base64 hash of the new value
                    // 4. Write the hash back as the value of "Sec-WebSocket-Accept" response header in an HTTP response
                    string swk = Regex.Match(s, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
                    string swka = swk + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                    byte[] swkaSha1 = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(swka));
                    string swkaSha1Base64 = Convert.ToBase64String(swkaSha1);

                    // HTTP/1.1 defines the sequence CR LF as the end-of-line marker
                    byte[] response = Encoding.UTF8.GetBytes(
                        "HTTP/1.1 101 Switching Protocols\r\n" +
                        "Connection: Upgrade\r\n" +
                        "Upgrade: websocket\r\n" +
                        "Sec-WebSocket-Accept: " + swkaSha1Base64 + "\r\n\r\n");

                    stream.Write(response, 0, response.Length);
                }
            }
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

}
