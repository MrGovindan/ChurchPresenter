using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Services
{
    public interface IFileSystem
    {
        string ExtractAndReadFileFromArchive(string filename);
    }
}
