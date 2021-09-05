using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Services;
using ChurchPresenter.UI.Services.SlideEncoder;
using Newtonsoft.Json.Linq;

namespace ChurchPresenter.UI.Models
{
    public class WebBrowserProjector
    {
        readonly IWriter<string> server;
        bool slideVisible = true;
        string currentSlide = "";
        readonly JObject blankSlide = new JObject();

        public WebBrowserProjector(
            [KeyFilter("Live")] IDisplayedSlideService selectedSlidePublisher,
            ISlideVisibilityService slideVisibilityService,
            ISlideEncoder slideEncoder,
            [KeyFilter("WebSocketServer")] IWriter<string> output)
        {
            blankSlide["text"] = "";
            blankSlide["caption"] = "";

            server = output;

            selectedSlidePublisher.DisplayedSlideChanged += slide =>
            {
                var json = new JObject { ["text"] = slideEncoder.Encode(slide), ["caption"] = slide.GetCaption() };
                currentSlide = json.ToString();
                Show();
            };
            slideVisibilityService.SlideVisibilityChanged += visible =>
            {
                slideVisible = visible;
                Show();
            };
        }

        private void Show()
        {
            server.Write(slideVisible ? currentSlide : blankSlide.ToString());
        }
    }
}
