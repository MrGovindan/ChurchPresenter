using ChurchPresenter.UI.Models.Folder;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Services
{
    public interface ISelectedFolderService
    {
        event Action<IFolder> SelectedFolderChanged;
        void SelectFolder(IFolder song);
    }
}
