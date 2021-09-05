using ChurchPresenter.UI.Models.Folder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ChurchPresenter.UI.Services
{
    class SelectedFolderService : ISelectedFolderService
    {
        public event Action<IFolder> SelectedFolderChanged;

        public SelectedFolderService()
        {
            Debug.WriteLine("SongSelectedPublisher created");
        }

        public void SelectFolder(IFolder song)
        {
            SelectedFolderChanged?.Invoke(song);
        }
    }
}
