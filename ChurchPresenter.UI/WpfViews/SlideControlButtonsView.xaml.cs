using ChurchPresenter.UI.Presenters;
using MahApps.Metro.IconPacks;
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
    public partial class SlideControlButtonsView : Grid, ISlideControlButtonsView
    {
        static readonly GridLength STAR = new GridLength(1, GridUnitType.Star);
        static readonly GridLength AUTO = new GridLength(1, GridUnitType.Auto);

        private readonly Button nextSlideButton;
        private readonly Button previousSlideButton;

        public event Action GoToPreviousSlide;
        public event Action GoToNextSlide;

        protected SlideControlButtonsView()
        {
            InitializeComponent();

            previousSlideButton = CreateNewIconButton(PackIconMaterialKind.ChevronLeft, "Go to previous slide", () => GoToPreviousSlide?.Invoke());
            nextSlideButton = CreateNewIconButton(PackIconMaterialKind.ChevronRight, "Go to next slide", () => GoToNextSlide?.Invoke());
        }

        protected Button CreateNewIconButton(PackIconMaterialKind iconKind, string tooltipText, Action action)
        {
            var button = new Button();
            button.Margin = new Thickness(4);
            var tooltip = new ToolTip();
            var tooltipTextBlock = new TextBlock();
            tooltipTextBlock.Text = tooltipText;
            tooltip.Content = tooltipTextBlock;
            button.ToolTip = tooltip;

            var icon = new PackIconMaterial();
            icon.Kind = iconKind;
            button.Content = icon;
            Children.Add(button);
            button.Click += (o, e) => action.Invoke();

            return button;
        }
        protected Button CreateNewTextButton(string text, string tooltipText, Action action)
        {
            var button = new Button();
            button.Margin = new Thickness(4);
            var tooltip = new ToolTip();
            var tooltipTextBlock = new TextBlock();
            tooltipTextBlock.Text = tooltipText;
            tooltip.Content = tooltipTextBlock;
            button.ToolTip = tooltip;

            var textBlock = new TextBlock();
            textBlock.Text = text;
            button.Content = textBlock;
            Children.Add(button);
            button.Click += (o, e) => action.Invoke();

            return button;
        }

        protected void SetUpLayout()
        {
            ColumnDefinitions.Clear();
            ColumnDefinitions.Add(CreateColumnDefinition(STAR));
            foreach (Button item in Children)
                ColumnDefinitions.Add(CreateColumnDefinition(AUTO));
            ColumnDefinitions.Add(CreateColumnDefinition(STAR));

            int i = 1;
            foreach (Button item in Children)
            {
                Grid.SetColumn(item, i);
                i++;
            }
        }

        private ColumnDefinition CreateColumnDefinition(GridLength length)
        {
            var cd = new ColumnDefinition();
            cd.Width = length;
            return cd;
        }

        public void SetNextSlideButtonEnabled(bool enabled)
        {
            nextSlideButton.IsEnabled = enabled;
        }

        public void SetPreviousSlideButtonEnabled(bool enabled)
        {
            previousSlideButton.IsEnabled = enabled;
        }
    }

    public class LiveSlideControlButtonsView : SlideControlButtonsView, ILiveSlideControlButtonsView
    {
        public event Action SlideHidden;
        public event Action SlideShown;

        private Button slideShownButton;
        private Button slideHiddenButton;

        public LiveSlideControlButtonsView() : base()
        {
            slideShownButton = CreateNewTextButton("Show", "Show slide", () => SlideShown?.Invoke());
            slideHiddenButton = CreateNewTextButton("Hide", "Hide slide", () => SlideHidden?.Invoke());

            SetUpLayout();
        }

        public void SetShowSlideButtonEnabled(bool enabled)
        {
            slideShownButton.IsEnabled = enabled;
        }

        public void SetHideSlideButtonEnabled(bool enabled)
        {
            slideHiddenButton.IsEnabled = enabled;
        }
    }

    public class PreviewSlideControlButtonsView : SlideControlButtonsView, IPreviewSlideControlButtonsView
    {
        private Button showSongOnLiveButton;
        private Button addSongToServiceButton;

        public event Action SongAddedToService;
        public event Action SongShownOnLive;

        public PreviewSlideControlButtonsView() : base()
        {
            showSongOnLiveButton = CreateNewIconButton(PackIconMaterialKind.ProjectorScreen, "Show song on Live", () => SongShownOnLive?.Invoke());
            addSongToServiceButton = CreateNewIconButton(PackIconMaterialKind.PlaylistPlus, "Add song to service", () => SongAddedToService?.Invoke());

            SetUpLayout();
        }
    }
}
