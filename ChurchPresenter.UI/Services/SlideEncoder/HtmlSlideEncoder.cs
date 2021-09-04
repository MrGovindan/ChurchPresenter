using ChurchPresenter.UI.Models.Folder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ChurchPresenter.UI.Services.SlideEncoder
{
    class HtmlSlideEncoder : ISlideEncoder
    {
        public string Encode(Slide slide)
        {
            var result = GetHtml(slide.GetParts()[0]);
            for (int i = 1; i < slide.GetParts().Count; ++i)
                result += GetHtml(slide.GetParts()[i]);
            return result;
        }

        private static string GetHtml(TextPart part)
        {
            var html = EncodeAsHtml(part.Text);

            if (part.Type == TextType.Superscript)
                return "<sup>" + html + "</sup>";

            return html;
        }

        private static string EncodeAsHtml(string text)
        {
            return HttpUtility.HtmlEncode(text);
        }
    }
}
