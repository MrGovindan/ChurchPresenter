using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Presenters;
using ChurchPresenter.UI.WpfViews;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChurchPresenter.UI
{
    public partial class MainWindow : Window, IMainWindow
    {
        Panel previewPanel;
        Panel livePanel;
        private readonly IServiceModel serviceModel;
        Panel libraryPanel;
        Panel servicePanel;

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
            Import.Click += HandleImport;
        }

        private void HandleImport(object sender, RoutedEventArgs e)
        {
            var fd = new OpenFileDialog();
            if (fd.ShowDialog() == true)
            {
                var fp = fd.FileNames[0];
                var archive = ZipFile.Open(fp, ZipArchiveMode.Read);
                var stream = archive.Entries[0].Open();
                var reader = new StreamReader(stream);
                var text = reader.ReadToEnd();
                var parser = new OpenLpServiceParser();
                try
                {
                    serviceModel.ClearService();

                    var serviceItems = parser.Parse(text);
                    foreach (var item in serviceItems)
                        serviceModel.AddFolder(item);
                }
                catch (Exception)
                {
                }
            }
        }

        public event Action LiveViewSelected;
        public event Action SetupViewSelected;

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
            var cd = new ColumnDefinition();
            cd.Width = new GridLength(0);
            MainGrid.ColumnDefinitions[0] = cd;
            LeftPanel.Children.Remove(libraryPanel);
            LeftSplitter.IsEnabled = false;

            cd = new ColumnDefinition();
            cd.Width = new GridLength(0);
            CenterPanel.ColumnDefinitions[0] = cd;
            CenterPanel.Children.Remove(previewPanel);

            Width = ActualWidth / 2;
            Left += Width;

            ShowSetupView.IsEnabled = true;
            ShowLiveView.IsEnabled = false;
        }

        private void ArrangeForSetup()
        {
            var cd = new ColumnDefinition();
            cd.Width = new GridLength(1, GridUnitType.Auto);
            cd.MinWidth = 200;
            MainGrid.ColumnDefinitions[0] = cd;
            LeftPanel.Children.Add(libraryPanel);
            LeftSplitter.IsEnabled = true;

            cd = new ColumnDefinition();
            cd.Width = new GridLength(1, GridUnitType.Star);
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
