using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Presenters;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChurchPresenter.UI.WpfViews
{
    public partial class ProjectionView : Grid, IProjectionView
    {
        public event Action<int> SlideSelected;

        protected ProjectionView(string heading)
        {
            InitializeComponent();
            Heading.Text = heading;
        }

        public void SetSlides(Slide[] slides)
        {
            SlideList.Items.Clear();
            var slideIndex = 0;
            foreach (var slide in slides)
                SlideList.Items.Add(CreateNewSlideItem(slide, slideIndex++));
        }

        private ListBoxItem CreateNewSlideItem(Slide slide, int index)
        {
            var item = new ListBoxItem();
            item.BorderBrush = new SolidColorBrush(Colors.Gray);
            item.BorderThickness = new Thickness(1, 1, 1, 1);
            item.MinHeight = 40;
            var text = new TextBlock();
            text.Text = slide.text;
            item.Content = text;
            item.Selected += (o, e) => SlideSelected?.Invoke(index);
            return item;
        }

        public void SetTitle(string title)
        {
            SongTitle.Text = title;
        }

        public void SetPreviewText(string text)
        {
            foreach (var child in PreviewGrid.Children)
            {
                if (child is TextBlock)
                {
                    ((TextBlock)child).Text = text;
                    ((TextBlock)child).TextAlignment = TextAlignment.Center;
                }
            }
        }

        public void SelectSlide(int index)
        {
            ((ListBoxItem)SlideList.Items[0]).IsSelected = true;
        }
    }
}
