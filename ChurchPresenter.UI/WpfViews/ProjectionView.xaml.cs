using ChurchPresenter.UI.Models.Folder;
using ChurchPresenter.UI.Presenters;
using System;
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
            SetRow(slideControlButtonsView, 3);
        }

        public void SetSlides(string[] slides)
        {
            SlideList.Items.Clear();
            var slideIndex = 0;
            foreach (var slide in slides)
            {
                var currentIndex = slideIndex;
                SlideList.Items.Add(CreateNewSlideItem(slide, currentIndex));
                slideIndex++;
            }
        }

        private ListBoxItem CreateNewSlideItem(string slide, int index)
        {
            var item = new ListBoxItem
            {
                BorderBrush = new SolidColorBrush(Colors.Gray),
                BorderThickness = new Thickness(1, 1, 1, 1),
                MinHeight = 40
            };
            var text = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Text = slide
            };
            item.Content = text;
            item.Selected += (o, e) => SlideSelected?.Invoke(index);
            return item;
        }

        public void SetTitle(string title)
        {
            SongTitle.Text = title;
        }

        public void SetPreviewText(Slide slide)
        {
            foreach (var child in TextGrid.Children)
            {
                var tb = (TextBlock)child;
                tb.Inlines.Clear();

                foreach (var part in slide.GetParts())
                {

                    if (part.Type == TextType.Superscript)
                    {
                        tb.Inlines.Add(new Run(part.Text)
                        {
                            BaselineAlignment = BaselineAlignment.Superscript,
                            FontSize = 28
                        });
                    }
                    else
                    {
                        tb.Inlines.Add(new Run(part.Text));
                    }
                }
            }

            CaptionText.Text = slide.GetCaption();
        }

        public void SelectSlide(int index)
        {
            var selectedItem = (ListBoxItem)SlideList.Items[index]; 
            selectedItem.IsSelected = true;
            selectedItem.Focus();
            SlideList.ScrollIntoView((ListBoxItem)SlideList.Items[index]);
        }

        public void SetPreviewTextVisible(bool visible)
        {
            foreach (UIElement child in PreviewGrid.Children)
                child.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
