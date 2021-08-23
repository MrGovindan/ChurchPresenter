using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    class SongSelectedPublisher : ISongSelectedPublisher
    {
        public event Action<Song> SelectedSongChanged;

        public SongSelectedPublisher()
        {
            Debug.WriteLine("SongSelectedPublisher created");
        }

        public void PublishSelectedSong(Song song)
        {
            SelectedSongChanged?.Invoke(song);
        }
    }
}
