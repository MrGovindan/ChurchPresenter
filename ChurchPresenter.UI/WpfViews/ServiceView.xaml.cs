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
    public partial class ServiceView : Grid, IServicePanelView
    {
        public event Action<int> SongSelected;
        public event Action<int> SongRemoved;

        public ServiceView()
        {
            InitializeComponent();
        }

        public void AddSongTitle(string name)
        {
            var entry = new SongEntry(name, "Remove from service");
            ServiceList.Items.Add(entry);
            var index = ServiceList.Items.Count - 1;
            entry.MouseDoubleClick += (o, e) => SongSelected?.Invoke(GetEntryIndex(entry));
            entry.ContextMenuItem.Click += (o, e) => SongRemoved?.Invoke(GetEntryIndex(entry));
        }

        private int GetEntryIndex(SongEntry entry) => ServiceList.Items.IndexOf(entry);

        public void RemoveSongTitle(int index)
        {
            ServiceList.Items.RemoveAt(index);
        }
    }
}
