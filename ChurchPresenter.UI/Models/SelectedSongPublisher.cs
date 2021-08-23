using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    class SelectedSongPublisher : ISelectedSongPublisher
    {
        public event Action<Song> SelectedSongChanged;

        public SelectedSongPublisher()
        {
            Debug.WriteLine("SongSelectedPublisher created");
        }

        public void PublishSelectedSong(Song song)
        {
            SelectedSongChanged?.Invoke(song);
        }
    }
}
