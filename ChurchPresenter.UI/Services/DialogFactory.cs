using ChurchPresenter.UI.Presenters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Services
{
    class DialogFactory : IDialogFactory
    {
        public IOpenFileDialog CreateOpenFileDialog()
        {
            return new OpenFileDialog();
        }
    }
}
