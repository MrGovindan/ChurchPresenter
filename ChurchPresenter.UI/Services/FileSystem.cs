using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ChurchPresenter.UI.Services
{
    class FileSystem : IFileSystem
    {
        public string ExtractAndReadFileFromArchive(string filename)
        {
            var archive = ZipFile.Open(filename, ZipArchiveMode.Read);
            var stream = archive.Entries[0].Open();
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
