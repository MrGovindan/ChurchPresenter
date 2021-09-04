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
            [KeyFilter("Live")] ISelectedSliderPublisher selectedSlidePublisher,
            ISlideVisibilityModel slideVisibilityPublisher,
            ISlideEncoder slideEncoder,
            IWriter<string> output)
        {
            blankSlide["text"] = "";
            blankSlide["caption"] = "";

            server = output;

            selectedSlidePublisher.SelectedSlideChanged += slide =>
            {
                var json = new JObject { ["text"] = slideEncoder.Encode(slide), ["caption"] = slide.GetCaption() };
                currentSlide = json.ToString();
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
            server.Write(slideVisible ? currentSlide : blankSlide.ToString());
        }
    }
}
