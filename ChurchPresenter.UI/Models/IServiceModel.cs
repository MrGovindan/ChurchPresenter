using ChurchPresenter.UI.Models.Folder;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{

    public interface IServiceModel
    {
        event Action<IFolder> FolderAdded;
        event Action<int[]> ServiceReordered;
        event Action ServiceCleared;

        void AddFolder(IFolder folder);
        IFolder ItemAt(int index);
        void RemoveFolder(int index);
        IFolder[] GetFolders();
        void SwapFolderOrder(int first, int second);
        void MakeFolderFirst(int index);
        void MakeFolderLast(int index);
        int ServiceLength();
        void ClearService();
    }
}
