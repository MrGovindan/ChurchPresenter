using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public enum FolderType
    {
        Lyric,
        Scripture
    }

    public interface IFolder
    {
        FolderType GetFolderType();
        string GetTitle();
        Slide[] GetSlides();
    }

    public interface IServiceModel
    {
        event Action<IFolder> ItemAdded;
        void AddFolder(IFolder folder);

        IFolder ItemAt(int index);
        void RemoveSongAt(int index);
    }
}
