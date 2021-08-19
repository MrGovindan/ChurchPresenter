using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChurchPresenter.WebSocketServer
{
    public class WebSocketServer
    {
        private TcpListener server;
        private List<TcpClient> clients = new List<TcpClient>();

        public WebSocketServer(IPEndPoint endPoint)
        {
            server = new TcpListener(endPoint);
            server.Start();
            server.AcceptTcpClientAsync().ContinueWith(HandleAccept, null);
        }

        private void HandleAccept(Task<TcpClient> acceptClientTask, object stateObject)
        {
            if (acceptClientTask.IsCompletedSuccessfully)
            {
                var client = acceptClientTask.Result;
            }
        }
    }
}
