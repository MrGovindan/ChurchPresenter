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
        bool slideVisible = true;
        string currentSlide = "";

        public WebBrowserProjector(
            [KeyFilter("Live")] ISelectedSliderPublisher selectedSlidePublisher,
            ISlideVisibilityModel slideVisibilityPublisher)
        {
            server = new WebSocketServer.WebSocketServer(new IPEndPoint(IPAddress.Loopback, 5000));

            selectedSlidePublisher.SelectedSlideChanged += slide =>
            {
                currentSlide = slide.ToHtml();
                Show();
            };
            slideVisibilityPublisher.SlideVisibilityChanged += visible =>
            {
                slideVisible = visible;
                Show();
            };
        }
        private void Show()
        {
            server.WriteString(slideVisible ? currentSlide : "");
        }
    }
}
