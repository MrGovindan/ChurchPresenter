using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChurchPresenter.UI.WpfViews
{
    public partial class SlideView : ListBoxItem
    {
        public SlideView(Slide slide, Action selectedAction)
        {
            InitializeComponent();
            BorderBrush = new SolidColorBrush(Colors.Gray);
            BorderThickness = new Thickness(1, 1, 1, 1);
            MinHeight = 40;
            var text = new TextBlock();
            text.TextWrapping = TextWrapping.Wrap;
            text.Text = slide.ToString();
            Content = text;
            Selected += (o, e) => selectedAction.Invoke();
        }
    }
}
