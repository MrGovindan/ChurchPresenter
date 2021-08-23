using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchPresenter.WebSocketServer
{
    internal class WebSocketStreamManager
    {
        private static string pageString = @"<!doctype html><style>body, html{margin: 0; padding: 0;}#content-area{width: 100%; height: 100vh; background: transparent;}#text{position: absolute; padding: 0; margin: 0; width: 100%; bottom: 0; text-align: center; font-family: sans-serif; font-size: 61px; text-shadow: 2px 0 4px white, -2px 0 4px white, 0 2px 4px white, 0px -2px 4px white;}</style><div id='content-area'> <h1 id='text'></h1></div><script>var wsUri='ws://127.0.0.1:5000/'; function createWebSocket(){console.log('Creating web socket'); var websocket; websocket=new WebSocket(wsUri); websocket.onopen=function (e){}; websocket.onclose=function (e){}; websocket.onmessage=function (e){console.log('Received data'); document.querySelector('#text').innerHTML=e.data;}; websocket.onerror=function (e){websocket.close(); console.log('WebSocket error occured. Reconnection in 1 second'); setTimeout(createWebSocket, 1000); document.querySelector('#text').innerHTML='error detected';};}createWebSocket();</script>";

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
                    Debug.WriteLine("Handshake accepted");
                    Debug.WriteLine("Thread ID: " + Thread.CurrentThread.ManagedThreadId);

                    if (s.Contains("Sec-WebSocket-Key"))
                    {
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

                        mem = new byte[1000];
                        stream.ReadAsync(mem, 0, mem.Length).ContinueWith(HandleReadFrameCompleted, mem);
                    }
                    else
                    {
                        var response = "HTTP/1.1 200 OK\r\n" +
                        "content-type: text/html; charset=utf-8\r\n" +
                        "connection: close\r\n" +
                        "content-length: " + pageString.Length + "\r\n\r\n" + pageString;
                        var bytes = Encoding.UTF8.GetBytes(response);
                        stream.Write(bytes, 0, bytes.Length);
                        disconnectedAction.Invoke(this);
                        stream.Close();
                    }
                }
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
                    Debug.WriteLine("Connection Close OpCode received");
                    disconnectedAction.Invoke(this);
                }
            }
            else
            {
                Debug.WriteLine("Frame Read Failed");
                disconnectedAction.Invoke(this);
            }
        }

        public void WriteString(string message)
        {
            var frame = new WebSocketFrameBuilder().withStringPayload(message).build();
            stream.WriteAsync(frame, 0, frame.Length).ContinueWith(HandleWriteComplete, null);
        }

        private void HandleWriteComplete(Task arg1, object arg2)
        {
            if (arg1.IsFaulted)
            {
                Debug.WriteLine(arg1.Exception);
            }
        }
    }
}
