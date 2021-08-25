using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Presenters;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ChurchPresenter.UI.WpfViews
{
    public partial class SongLibraryView : Grid, ISongLibraryView
    {
        public event Action OnLoaded;
        public event Action<string> SearchStringChanged;
        public event Action<int> SelectedSongChanged;
        public event Action<int> SongAddedToService;

        public SongLibraryView()
        {
            InitializeComponent();

            Loaded += (o, e) => OnLoaded?.Invoke();
            SearchBox.TextChanged += (o, e) => SearchStringChanged?.Invoke(SearchBox.Text);
        }

        public void SetSongList(IEnumerable<string> songs)
        {
            SongList.Items.Clear();
            var index = 0;
            foreach (var song in songs)
            {
                var entry = new ListItem(song, "Add to service");
                var currentIndex = index;
                entry.MouseDoubleClick += (o, e) => SelectedSongChanged?.Invoke(currentIndex);
                entry.ContextMenuItem.Click += (o, e) => SongAddedToService?.Invoke(currentIndex);
                SongList.Items.Add(entry);
                index++;
            }
        }
    }
}
