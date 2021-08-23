using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Presenters
{
    public interface ILibraryView<T>
    {
        event Action SongsSelected;
        event Action BiblesSelected;
        event Action OnLoaded;

        public void ShowContent(LibraryContent contentType, T content);
    }

    public interface ILibraryContentFactory<T>
    {
        public T GetContent(LibraryContent contentType);
    }

    public enum LibraryContent
    {
        Songs,
        Bibles,
    }

    internal class LibraryPresenter<T>
    {
        private readonly ILibraryView<T> view;
        private readonly ILibraryContentFactory<T> contentFactory;

        public LibraryPresenter(ILibraryView<T> view, ILibraryContentFactory<T> contentFactory)
        {
            this.view = view;
            this.contentFactory = contentFactory;

            view.OnLoaded += () => HandleLibrarySelection(LibraryContent.Songs);
            view.SongsSelected += () => HandleLibrarySelection(LibraryContent.Songs);
            view.BiblesSelected += () => HandleLibrarySelection(LibraryContent.Bibles);
        }

        private void HandleLibrarySelection(LibraryContent contentType)
        {
            view.ShowContent(contentType, contentFactory.GetContent(contentType));
        }
    }
}
