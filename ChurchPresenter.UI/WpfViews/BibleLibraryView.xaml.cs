using ChurchPresenter.UI.Presenters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class BibleLibraryView : Grid, IBibleLibraryView
    {
        public event Action<string> SearchStringChanged;
        public event Action<string, int> SearchStarted;
        public event Action<int[]> AddedToService;
        public event Action<int[]> ShownOnPreview;

        public BibleLibraryView()
        {
            InitializeComponent();

            SearchBox.TextInput += (o, e) => SearchStringChanged?.Invoke(SearchBox.Text);
            SearchBox.KeyUp += HandleSearchBoxKeyUp;
            SearchBox.IsTextSearchEnabled = false;
            SearchBox.StaysOpenOnEdit = true;
            ScriptureList.SelectionMode = SelectionMode.Extended;
            SearchButton.Click += (o, e) => StartSearch();
        }

        private void HandleSearchBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                StartSearch();
            else
                SearchStringChanged?.Invoke(SearchBox.Text);
        }

        private void StartSearch()
        {
            SearchStarted?.Invoke(SearchBox.Text, VersionsList.SelectedIndex);
        }

        public void ShowNoResults()
        {
            ScriptureList.Items.Clear();
            var item = new ListBoxItem();
            var textBlock = new TextBlock();
            textBlock.Text = "No results found";
            textBlock.FontStyle = FontStyles.Italic;
            item.Content = textBlock;
            item.IsEnabled = false;
            ScriptureList.Items.Add(item);
        }

        public void ShowScriptures(string[] scriptures)
        {
            ScriptureList.Items.Clear();
            foreach (var script in scriptures)
            {
                var item = new ListItem(script, "Add to service");
                item.ContextMenuItem.Click += HandleAddToService;
                item.MouseDoubleClick += ShowItemsInPreview;
                item.KeyUp += (o, e) => { if (e.Key == Key.Enter) ShowItemsInPreview(o, null); };
                ScriptureList.Items.Add(item);
            }
            ScriptureList.SelectAll();
        }

        private void ShowItemsInPreview(object sender, MouseButtonEventArgs e)
        {
            ShownOnPreview?.Invoke(GetSelectedIndices().ToArray());
        }

        private void HandleAddToService(object sender, RoutedEventArgs e)
        {
            AddedToService?.Invoke(GetSelectedIndices().ToArray());
        }

        private List<int> GetSelectedIndices()
        {
            var index = 0;
            var selectedIndices = new List<int>();
            var selectedItems = ScriptureList.SelectedItems;
            foreach (var item in ScriptureList.Items)
            {
                if (selectedItems.Contains(item))
                    selectedIndices.Add(index);

                ++index;
            }

            return selectedIndices;
        }

        public void ShowSuggestions(string[] suggestions)
        {
            if (suggestions.Length == 0)
                return;

            while (SearchBox.Items.Count != 0)
                SearchBox.Items.RemoveAt(SearchBox.Items.Count - 1);

            foreach (var suggestion in suggestions)
            {
                var item = new ComboBoxItem();
                var textBlock = new TextBlock();
                textBlock.Text = suggestion;
                item.Content = textBlock;
                SearchBox.Items.Add(item);
            }
            SearchBox.IsDropDownOpen = true;
            ((TextBox)SearchBox.Template.FindName("PART_EditableTextBox", SearchBox)).CaretIndex = SearchBox.Text.Length;
        }

        public void SetBibleVersions(string[] versions)
        {
            VersionsList.Items.Clear();
            foreach (var version in versions)
            {
                var item = new ComboBoxItem();
                var textBlock = new TextBlock();
                textBlock.Text = version;
                item.Content = textBlock;
                VersionsList.Items.Add(item);
            }
            VersionsList.SelectedItem = VersionsList.Items[0];
        }
    }
}
