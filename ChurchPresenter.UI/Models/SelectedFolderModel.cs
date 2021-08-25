using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    class SelectedFolderModel : ISelectedFolderModel
    {
        public event Action<IFolder> SelectedFolderChanged;

        public SelectedFolderModel()
        {
            Debug.WriteLine("SongSelectedPublisher created");
        }

        public void PublishSelectedFolder(IFolder song)
        {
            SelectedFolderChanged?.Invoke(song);
        }
    }
}
