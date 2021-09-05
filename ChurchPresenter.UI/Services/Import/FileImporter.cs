using ChurchPresenter.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Services.Import
{
    class FileImporter : IWriter<string>
    {
        private IFileSystem fileSystem;
        private IOpenLpServiceParser parser;
        private IServiceModel serviceModel;

        public FileImporter(IFileSystem fileSystem, IOpenLpServiceParser parser, IServiceModel serviceModel)
        {
            this.fileSystem = fileSystem;
            this.parser = parser;
            this.serviceModel = serviceModel;
        }

        public void Write(string filename)
        {
            var fileContents = fileSystem.ExtractAndReadFileFromArchive(filename);
            var folders = parser.Parse(fileContents);
            serviceModel.ClearService();
            foreach (var folder in folders)
                serviceModel.AddFolder(folder);
        }
    }
}
