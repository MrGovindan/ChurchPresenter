using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Presenters;
using ChurchPresenter.UI.WpfViews;
using System;
using System.Collections.Generic;
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
        Panel libraryPanel;
        Panel servicePanel;

        public MainWindow(
            LibraryView libraryView,
            PreviewProjectionView previewPanelView,
            LiveProjectionView livePanelView,
            ServiceView serviceView)
        {
            libraryPanel = libraryView;
            previewPanel = previewPanelView;
            livePanel = livePanelView;
            servicePanel = serviceView;
            
            InitializeComponent();

            Grid.SetColumn(previewPanel, 0);

            CenterPanel.Children.Add(livePanel);
            Grid.SetColumn(livePanel, 1);

            RightPanel.Children.Add(servicePanel);

            ShowLiveView.Click += (o, e) => LiveViewSelected?.Invoke();
            ShowSetupView.Click += (o, e) => SetupViewSelected?.Invoke();
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

            cd = new ColumnDefinition();
            cd.Width = new GridLength(0);
            CenterPanel.ColumnDefinitions[0] = cd;
            CenterPanel.Children.Remove(previewPanel);
            Width = ActualWidth / 2;

            ShowSetupView.IsEnabled = true;
            ShowLiveView.IsEnabled = false;
        }

        private void ArrangeForSetup()
        {
            var cd = new ColumnDefinition();
            cd.Width = new GridLength(400);
            MainGrid.ColumnDefinitions[0] = cd;
            LeftPanel.Children.Add(libraryPanel);

            cd = new ColumnDefinition();
            cd.Width = new GridLength(1, GridUnitType.Star);
            CenterPanel.ColumnDefinitions[0] = cd;
            CenterPanel.Children.Add(previewPanel);

            if (ActualWidth != 0)
                Width = ActualWidth * 2;

            ShowSetupView.IsEnabled = false;
            ShowLiveView.IsEnabled = true;
        }
    }
}
