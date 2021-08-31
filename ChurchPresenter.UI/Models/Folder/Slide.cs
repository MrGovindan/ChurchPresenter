namespace ChurchPresenter.UI.Models.Folder
{
    public class Slide
    {
        protected readonly string text;
        protected string caption;

        public Slide(string text)
        {
            this.text = text;
        }

        public Slide(string text, string caption) : this(text)
        {
            this.caption = caption;
        }

        public override string ToString()
        {
            return new string(text);
        }

        public virtual string ToHtml()
        {
            return new string(text).Replace("\r", "").Replace("\n", "<br>").Replace("\"", "$quot;");
        }

        public virtual string GetCaption()
        {
            return caption;
        }
    }
}
