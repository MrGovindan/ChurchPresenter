using Autofac.Features.AttributeFilters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public class WebBrowserProjector
    {
        WebSocketServer.WebSocketServer server;
        public WebBrowserProjector([KeyFilter("Live")] ISelectedSlidePublisher selectedSlidePublisher)
        {
            server = new WebSocketServer.WebSocketServer(new IPEndPoint(IPAddress.Loopback, 5000));

            selectedSlidePublisher.SelectedSlideChanged += slide => Show(slide.text);
        }
        private void Show(string content)
        {
            content = content.Replace("\n", "<br>");
            server.WriteString(content);
        }
    }
}
