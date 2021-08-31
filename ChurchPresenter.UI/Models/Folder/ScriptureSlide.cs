using ChurchPresenter.UI.Models;

namespace ChurchPresenter.UI.Models.Folder
{
    class ScriptureSlide : Slide
    {
        private string version;
        private Verse verse;

        internal ScriptureSlide(string text, string version, Verse verse) : base(text)
        {
            this.version = version;
            this.verse = verse;
            this.caption = string.Format("{0} {1}:{2} {3}",
                BookHelper.ToString(verse.book), verse.chapter, verse.verse, version);
        }

        public override string ToString()
        {
            return string.Format("{0}:{1} {2}", verse.chapter, verse.verse, text);
        }

        public override string ToHtml()
        {
            return string.Format("<sup>{0}:{1}</sup> {2}", verse.chapter, verse.verse, text);
        }
    }
}
