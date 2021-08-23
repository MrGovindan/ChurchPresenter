using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public class LiveProjector : IProjector
    {
        WebSocketServer.WebSocketServer server;
        public LiveProjector()
        {
            server = new WebSocketServer.WebSocketServer(new IPEndPoint(IPAddress.Loopback, 5000));
        }
        public void Show(string content)
        {
            content = content.Replace("\n", "<br>");
            server.WriteString(content);
        }
    }
}
