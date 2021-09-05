using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Services;
using System;

namespace ChurchPresenter.UI.Presenters
{
    public interface IImportView
    {
        event Action ImportStarted;
    }

    public interface IOpenFileDialog
    {
        bool Show();

        string[] GetFiles();
        void SetFilter(string title, string extension);
    }

    public interface IDialogFactory
    {
        IOpenFileDialog CreateOpenFileDialog();
    }

    class ImportPresenter
    {
        private readonly IDialogFactory dialogFactory;
        private readonly IWriter<string> fileImporter;

        public ImportPresenter(
            IDialogFactory dialogFactory,
            [KeyFilter("FileImporter")]IWriter<string> fileImporter,
            params IImportView[] views)
        {
            this.dialogFactory = dialogFactory;
            this.fileImporter = fileImporter;

            foreach (var view in views)
                view.ImportStarted += HandleImportStarted;
        }

        private void HandleImportStarted()
        {
            var dialog = dialogFactory.CreateOpenFileDialog();
            dialog.SetFilter("OpenLP Service", "osz");
            if (dialog.Show())
            {
                fileImporter.Write(dialog.GetFiles()[0]);
            }
        }
    }
}
