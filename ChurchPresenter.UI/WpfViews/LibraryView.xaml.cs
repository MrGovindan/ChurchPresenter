using ChurchPresenter.UI.Presenters;
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
    public partial class LibraryView : Grid, ILibraryView<Panel>
    {
        static readonly GridLength STAR = new GridLength(1, GridUnitType.Star);
        static readonly GridLength AUTO = new GridLength(1, GridUnitType.Auto);

        public event Action SongsSelected;
        public event Action BiblesSelected;
        public event Action OnLoaded;

        public LibraryView()
        {
            InitializeComponent();
            SongsButton.MouseLeftButtonUp += (o, e) => SongsSelected?.Invoke();
            BiblesButton.MouseLeftButtonUp += (o, e) => BiblesSelected?.Invoke();
            Loaded += (o, e) => OnLoaded?.Invoke();
        }

        private RowDefinition CreateRowDefinition(GridLength gridLength)
        {
            var rowDefinition = new RowDefinition();
            rowDefinition.Height = gridLength;
            return rowDefinition;
        }

        public void ShowContent(LibraryContent contentType, Panel content)
        {
            switch (contentType)
            {
                case LibraryContent.Songs:
                    SongsButton.Content = content;
                    //ArrangeForSongContent();
                    break;
                case LibraryContent.Bibles:
                    BiblesButton.Content = content;
                    ArrangeForBibleContent();
                    break;
            }

            //Contents.Children.Clear();
            //Contents.Children.Add(content);
        }

        private void ArrangeForSongContent()
        {
            //Categories.RowDefinitions.Clear();
            //Categories.RowDefinitions.Add(CreateRowDefinition(AUTO));
            //Categories.RowDefinitions.Add(CreateRowDefinition(STAR));
            //Categories.RowDefinitions.Add(CreateRowDefinition(AUTO));

            //Grid.SetRow(SongsButton, 0);
            //Grid.SetRow(CategoryContent, 1);
            //Grid.SetRow(BiblesButton, 2);
        }

        private void ArrangeForBibleContent()
        {
            //Categories.RowDefinitions.Clear();
            //Categories.RowDefinitions.Add(CreateRowDefinition(AUTO));
            //Categories.RowDefinitions.Add(CreateRowDefinition(AUTO));
            //Categories.RowDefinitions.Add(CreateRowDefinition(STAR));

            //Grid.SetRow(SongsButton, 0);
            //Grid.SetRow(BiblesButton, 1);
            //Grid.SetRow(CategoryContent, 2);
        }
    }
}
