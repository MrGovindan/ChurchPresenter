using ChurchPresenter.UI.Models;
using System.Collections.Generic;

namespace ChurchPresenter.UI.Models.Folder
{
    class ScriptureFolder : IFolder
    {
        internal string title = "";
        internal List<ScriptureSlide> slides = new List<ScriptureSlide>();

        public FolderType GetFolderType()
        {
            return FolderType.Scripture;
        }

        public Slide[] GetSlides()
        {
            return slides.ToArray();
        }

        public string GetTitle()
        {
            return title;
        }
    }
}
