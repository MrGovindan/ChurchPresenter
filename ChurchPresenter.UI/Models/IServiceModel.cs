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
        event Action<int[]> ServiceReordered;

        void AddFolder(IFolder folder);
        IFolder ItemAt(int index);
        void RemoveFolder(int index);
        IFolder[] GetFolders();
        void SwapFolderOrder(int first, int second);
        void MakeFolderFirst(int index);
        void MakeFolderLast(int index);
        int ServiceLength();
    }
}
