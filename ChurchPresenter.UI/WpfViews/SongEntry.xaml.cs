using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
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
    public partial class ListItem : ListBoxItem
    {
        public ListItem(string songTitle, string contextMenuItemText)
        {
            InitializeComponent();
            SongTitle.Text = songTitle;
            ContextMenuItem.Header = contextMenuItemText;
        }

        public ListItem(IFolder folder, string contextMenuItemText)
        {
            InitializeComponent();
            Icon.Kind = folder.GetFolderType() == FolderType.Lyric ? PackIconMaterialKind.Music : PackIconMaterialKind.Book;
            SongTitle.Text = folder.GetTitle();
            ContextMenuItem.Header = contextMenuItemText;
        }
    }
}
