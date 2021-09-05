using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using ChurchPresenter.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchPresenter.UI.Presenters
{
    public interface ISongLibraryView
    {
        event Action OnLoaded;
        event Action<string> SearchStringChanged;
        event Action<int> SelectedSongChanged;
        event Action<int> SongAddedToService;

        void SetSongList(IEnumerable<string> songs);
    }


    public interface ISongLibrary
    {
        Task<IList<LyricFolder>> GetAllSongs();
        Task<IList<LyricFolder>> GetMatchingSongs(string pattern);

    }

    public class SongLibraryPresenter
    {
        private readonly ISongLibraryView view;
        private readonly ISongLibrary songLibrary;
        private List<LyricFolder> currentSongList;
        private bool loadedHandled = false;

        public SongLibraryPresenter(
            ISongLibraryView view,
            ISongLibrary songLibrary,
            [KeyFilter("Preview")] ISelectedFolderModel selectedSongPublisher,
            IServiceModel serviceModel)
        {
            this.view = view;
            this.songLibrary = songLibrary;

            view.OnLoaded += async () => await HandleViewLoaded();
            view.SearchStringChanged += async (searchString) => await HandleNewSearchString(searchString);
            view.SelectedSongChanged += index => selectedSongPublisher.PublishSelectedFolder(currentSongList[index]);
            view.SongAddedToService += index => serviceModel.AddFolder(currentSongList[index]);
        }

        private async Task HandleNewSearchString(string searchString)
        {
            if (searchString.Length > 3)
            {
                currentSongList = new List<LyricFolder>(await songLibrary.GetMatchingSongs(searchString));
                UpdateView();
            }
            else if (searchString.Length == 0)
                await GetAllSongs();
        }

        private async Task HandleViewLoaded()
        {
            if (!loadedHandled)
            {
                loadedHandled = true;
                await GetAllSongs();
            }
        }

        private async Task GetAllSongs()
        {
            currentSongList = new List<LyricFolder>(await songLibrary.GetAllSongs());
            UpdateView();
        }

        private void UpdateView()
        {
            view.SetSongList(currentSongList.Select(s => s.GetTitle()));
        }
    }
}