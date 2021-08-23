using Autofac.Features.AttributeFilters;
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
    public partial class MainWindow : Window
    {
        public MainWindow(
            LibraryView libraryView,
            PreviewProjectionView previewPanelView,
            LiveProjectionView livePanelView,
            ServiceView serviceView)
        {
            InitializeComponent();
            LeftPanel.Children.Add(libraryView);

            CenterPanel.Children.Add(previewPanelView);
            Grid.SetColumn(previewPanelView, 0);
            CenterPanel.Children.Add(livePanelView);
            Grid.SetColumn(livePanelView, 1);

            RightPanel.Children.Add(serviceView);
        }
    }
}
