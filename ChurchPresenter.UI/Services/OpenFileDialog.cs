using ChurchPresenter.UI.Presenters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Services
{
    class OpenFileDialog : IOpenFileDialog
    {
        private Microsoft.Win32.OpenFileDialog dialog;

        public OpenFileDialog()
        {
            dialog = new Microsoft.Win32.OpenFileDialog();
        }

        public string[] GetFiles()
        {
            return dialog.FileNames;
        }

        public void SetFilter(string title, string extension)
        {
            dialog.Filter = string.Format("{0}|*.{1}", title, extension);
        }

        public bool Show()
        {
            return (bool)dialog.ShowDialog();
        }
    }
}
