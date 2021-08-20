using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchPresenter.WebSocketServer
{
    internal class WebSocketStreamManager
    {
        private readonly Stream stream;
        private readonly Action<WebSocketStreamManager> connectedAction;
        private readonly Action<WebSocketStreamManager> disconnectedAction;

        public WebSocketStreamManager(
            Stream stream,
            Action<WebSocketStreamManager> connectedAction,
            Action<WebSocketStreamManager> disconnectedAction)
        {
            this.stream = stream;
            this.connectedAction = connectedAction;
            this.disconnectedAction = disconnectedAction;
            StartReadingHandshake();
        }

        private void StartReadingHandshake()
        {
            byte[] data = new byte[3];
            stream.ReadAsync(data, 0, data.Length)
                  .ContinueWith(HandleHandShakeReadCompleted, data);
        }

        private void HandleHandShakeReadCompleted(Task<int> readTask, object buffer)
        {
            if (readTask.IsCompletedSuccessfully)
            {
                var data = new List<byte>();
                data.AddRange((byte[])buffer);
                var mem = new byte[1024];
                stream.Read(mem, 0, mem.Length);
                data.AddRange(mem);

                string s = Encoding.UTF8.GetString(data.ToArray());
                if (Regex.IsMatch(s, "^GET", RegexOptions.IgnoreCase))
                {
                    Console.WriteLine("Handshake accepted");
                    Console.WriteLine("Thread ID: " + Thread.CurrentThread.ManagedThreadId);
                    string swk = Regex.Match(s, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
                    string swka = swk + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                    byte[] swkaSha1 = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(swka));
                    string swkaSha1Base64 = Convert.ToBase64String(swkaSha1);

                    byte[] response = Encoding.UTF8.GetBytes(
                        "HTTP/1.1 101 Switching Protocols\r\n" +
                        "Connection: Upgrade\r\n" +
                        "Upgrade: websocket\r\n" +
                        "Sec-WebSocket-Accept: " + swkaSha1Base64 + "\r\n\r\n");

                    stream.Write(response, 0, response.Length);
                    connectedAction.Invoke(this);
                }

                mem = new byte[1000];
                stream.ReadAsync(mem, 0, mem.Length).ContinueWith(HandleReadFrameCompleted, mem);
            }
            else
            {
                disconnectedAction.Invoke(this);
            }
        }

        private void HandleReadFrameCompleted(Task<int> readTask, object buffer)
        {
            if (readTask.IsCompletedSuccessfully)
            {
                var mem = (byte[])buffer;

                if (mem[0] == 0b10001000)
                {
                    Console.WriteLine("Connection Close OpCode received");
                    disconnectedAction.Invoke(this);
                }
            }
            else
            {
                Console.WriteLine("Frame Read Failed");
                disconnectedAction.Invoke(this);
            }
        }

        public async Task WriteString(string message)
        {
            try
            {
                var frame = new WebSocketFrameBuilder().withStringPayload(message).build();
                await stream.WriteAsync(frame, 0, frame.Length);
            }
            catch (Exception)
            {
            }
        }
    }
}
