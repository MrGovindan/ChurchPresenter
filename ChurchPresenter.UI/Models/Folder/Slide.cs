using System;
using System.Collections.Generic;

namespace ChurchPresenter.UI.Models.Folder
{
    public enum TextType
    {
        Normal,
        Superscript,
    }

    public readonly struct TextPart
    {
        public readonly TextType Type;
        public readonly string Text;

        public TextPart(string text, TextType type)
        {
            Text = text;
            Type = type;
        }

        internal static TextPart AsSuperscript(string text)
        {
            return new TextPart(text, TextType.Superscript);
        }

        internal static TextPart AsNormal(string text)
        {
            return new TextPart(text, TextType.Normal);
        }
    }

    public class Slide
    {
        private readonly List<TextPart> textParts = new List<TextPart>();
        private readonly string caption;

        public Slide(string text) : this(text, "")
        {
        }

        public Slide(string text, string caption)
        {
            textParts.Add(CreateNormalText(text));
            this.caption = caption;
        }

        public Slide(IReadOnlyList<TextPart> textParts, string caption)
        {
            this.textParts = new List<TextPart>(textParts);
            this.caption = caption;
        }

        public string GetCaption()
        {
            return caption;
        }

        public IReadOnlyList<TextPart> GetParts()
        {
            return textParts;
        }

        private TextPart CreateNormalText(string text)
        {
            return new TextPart(text, TextType.Normal);
        }
    }
}
