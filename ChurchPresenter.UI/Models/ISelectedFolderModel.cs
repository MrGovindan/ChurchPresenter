using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public interface ISelectedFolderModel
    {
        event Action<IFolder> SelectedFolderChanged;
        void PublishSelectedFolder(IFolder song);
    }
}
