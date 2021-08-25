using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Presenters;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ChurchPresenter.UI.WpfViews
{
    public partial class ProjectionView : Grid, IProjectionView
    {
        public event Action<int> SlideSelected;

        protected ProjectionView(string heading, SlideControlButtonsView slideControlButtonsView)
        {
            InitializeComponent();
            Heading.Text = heading;

            Children.Add(slideControlButtonsView);
            Grid.SetRow(slideControlButtonsView, 3);
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
            text.TextWrapping = TextWrapping.Wrap;
            text.Text = slide.ToString();
            item.Content = text;
            item.Selected += (o, e) => SlideSelected?.Invoke(index);
            return item;
        }

        public void SetTitle(string title)
        {
            SongTitle.Text = title;
        }

        private static readonly Regex supPattern = new Regex(@"<sup>(.*)<\/sup>(.*)");

        public void SetPreviewText(Slide slide)
        {
            var html = slide.ToHtml();
            var matches = supPattern.Match(html);

            foreach (var child in PreviewGrid.Children)
            {
                if (child is TextBlock)
                {
                    var tb = (TextBlock)child;
                    tb.Inlines.Clear();
                    var verseRun = new Run(matches.Groups[1].Value);
                    verseRun.BaselineAlignment = BaselineAlignment.Top;
                    verseRun.FontSize = 28;
                    tb.Inlines.Add(verseRun);
                    tb.Inlines.Add(new Run(matches.Groups[2].Value));
                    tb.TextAlignment = TextAlignment.Center;
                }
            }
        }

        public void SelectSlide(int index)
        {
            ((ListBoxItem)SlideList.Items[index]).IsSelected = true;
            ((ListBoxItem)SlideList.Items[index]).Focus();
            SlideList.ScrollIntoView((ListBoxItem)SlideList.Items[index]);
        }

        public void SetPreviewTextVisible(bool visible)
        {
            foreach (UIElement child in PreviewGrid.Children)
                child.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
