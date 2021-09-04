using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchPresenter.WebSocketServer
{
    public class WebSocketServer
    {
        private TcpListener server;
        private readonly List<WebSocketStreamManager> clients = new List<WebSocketStreamManager>();

        public WebSocketServer(IPEndPoint endPoint)
        {
            server = new TcpListener(endPoint);
            server.Start();
            StartAccept();
        }

        private void StartAccept()
        {
            server.AcceptTcpClientAsync().ContinueWith(HandleAccept, null);
        }

        private void HandleAccept(Task<TcpClient> acceptClientTask, object stateObject)
        {
            if (acceptClientTask.IsCompletedSuccessfully)
                CreateNewClient(acceptClientTask.Result);

            Debug.WriteLine("Current client count: " + clients.Count);

            StartAccept();
        }

        private void CreateNewClient(TcpClient client)
        {
            var manager = new WebSocketStreamManager(client.GetStream(),
                                                     manager => { },
                                                     manager => clients.Remove(manager));
            clients.Add(manager);
        }

        public void WriteString(string v)
        {
            foreach (var client in clients)
                client.WriteString(v);
        }
    }
}
