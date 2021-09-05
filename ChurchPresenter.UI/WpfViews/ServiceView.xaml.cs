using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
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
        public event Action<int> FolderSelected;
        public event Action FolderActivated;
        public event Action<int> FolderRemoved;
        public event Action FolderShitedUp;
        public event Action FolderShitedDown;
        public event Action FolderMadeTopmost;
        public event Action FolderMadeBottommost;
        public event Action ImportStarted;

        public ServiceView()
        {
            InitializeComponent();
            ShiftUp.Click += (o, e) => FolderShitedUp?.Invoke();
            ShiftDown.Click += (o, e) => FolderShitedDown?.Invoke();
            MakeFirst.Click += (o, e) => FolderMadeTopmost?.Invoke();
            MakeLast.Click += (o, e) => FolderMadeBottommost?.Invoke();
            ImportService.Click += (o, e) => ImportStarted?.Invoke();
        }

        private int GetEntryIndex(ListItem entry) => ServiceList.Items.IndexOf(entry);

        public void RemoveFolder(int index)
        {
            ServiceList.Items.RemoveAt(index);
        }

        public void AddFolder(IFolder folder)
        {
            var entry = new ListItem(folder, "Remove from service");
            ServiceList.Items.Add(entry);
            var index = ServiceList.Items.Count - 1;
            entry.Selected += (o, e) => FolderSelected?.Invoke(GetEntryIndex(entry));
            entry.MouseDoubleClick += (o, e) => FolderActivated?.Invoke();
            entry.ContextMenuItem.Click += (o, e) => FolderRemoved?.Invoke(GetEntryIndex(entry));
        }

        public void EnableShiftUp(bool enabled)
        {
            ShiftUp.IsEnabled = enabled;
            MakeFirst.IsEnabled = enabled;
        }

        public void EnableShiftDown(bool enabled)
        {
            ShiftDown.IsEnabled = enabled;
            MakeLast.IsEnabled = enabled;
        }

        public void UpdateServiceOrder(int[] newOrder)
        {
            var selected = (ListItem)ServiceList.SelectedItem;
            var serviceListCopy = new ListItem[ServiceList.Items.Count];
            ServiceList.Items.CopyTo(serviceListCopy, 0);
            ServiceList.Items.Clear();
            foreach (var index in newOrder)
                ServiceList.Items.Add(serviceListCopy[index]);
            selected.IsSelected = true;
            ServiceList.Focus();
        }

        public void ClearFolders()
        {
            ServiceList.Items.Clear();
        }
    }
}
