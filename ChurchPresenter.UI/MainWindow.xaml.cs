using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Presenters;
using ChurchPresenter.UI.Services.Import;
using ChurchPresenter.UI.WpfViews;
using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;

namespace ChurchPresenter.UI
{
    public partial class MainWindow : Window, IMainWindow
    {
        private readonly Panel previewPanel;
        private readonly Panel livePanel;
        private readonly IServiceModel serviceModel;
        private readonly Panel libraryPanel;
        private readonly Panel servicePanel;

        public event Action LiveViewSelected;
        public event Action SetupViewSelected;
        public event Action ImportStarted;

        public MainWindow(
            LibraryView libraryView,
            PreviewProjectionView previewPanelView,
            LiveProjectionView livePanelView,
            IServiceModel serviceModel,
            ServiceView serviceView)
        {
            libraryPanel = libraryView;
            previewPanel = previewPanelView;
            livePanel = livePanelView;
            this.serviceModel = serviceModel;
            servicePanel = serviceView;

            InitializeComponent();

            libraryPanel.MinWidth = 200;

            Grid.SetColumn(previewPanel, 0);

            CenterPanel.Children.Add(livePanel);
            Grid.SetColumn(livePanel, 1);

            RightPanel.Children.Add(servicePanel);

            ShowLiveView.Click += (o, e) => LiveViewSelected?.Invoke();
            ShowSetupView.Click += (o, e) => SetupViewSelected?.Invoke();
            Import.Click += (o, e) => ImportStarted?.Invoke();
        }

        public void ArrangeInMode(WindowMode mode)
        {
            switch (mode)
            {
                case WindowMode.SETUP:
                    ArrangeForSetup();
                    break;
                case WindowMode.LIVE:
                    ArrangeForLive();
                    break;
                default: throw new Exception("Unhandled case");
            }
        }

        private void ArrangeForLive()
        {
            var cd = new ColumnDefinition { Width = new GridLength(0) };
            MainGrid.ColumnDefinitions[0] = cd;
            LeftPanel.Children.Remove(libraryPanel);
            LeftSplitter.IsEnabled = false;

            cd = new ColumnDefinition { Width = new GridLength(0) };
            CenterPanel.ColumnDefinitions[0] = cd;
            CenterPanel.Children.Remove(previewPanel);

            Width = ActualWidth / 2;
            Left += Width;

            ShowSetupView.IsEnabled = true;
            ShowLiveView.IsEnabled = false;
        }

        private void ArrangeForSetup()
        {
            var cd = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto), MinWidth = 200 };
            MainGrid.ColumnDefinitions[0] = cd;
            LeftPanel.Children.Add(libraryPanel);
            LeftSplitter.IsEnabled = true;

            cd = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            CenterPanel.ColumnDefinitions[0] = cd;
            CenterPanel.Children.Add(previewPanel);

            var w = ActualWidth;
            if (ActualWidth != 0)
                Width = ActualWidth * 2;

            Left -= w;

            ShowSetupView.IsEnabled = false;
            ShowLiveView.IsEnabled = true;
        }
    }
}
