using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Presenters
{
    public enum WindowMode
    {
        SETUP,
        LIVE,
    }

    public interface IMainWindow : IImportView
    {
        event Action LiveViewSelected;
        event Action SetupViewSelected;

        void ArrangeInMode(WindowMode lIVE);
    }

    class MainWindowPresenter
    {
        public MainWindowPresenter(IMainWindow view)
        {
            view.ArrangeInMode(WindowMode.SETUP);

            view.LiveViewSelected += () => view.ArrangeInMode(WindowMode.LIVE);
            view.SetupViewSelected += () => view.ArrangeInMode(WindowMode.SETUP);
        }
    }
}
