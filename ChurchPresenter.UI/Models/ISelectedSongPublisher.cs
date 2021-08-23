using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public interface ISelectedSongPublisher
    {
        event Action<Song> SelectedSongChanged;
        void PublishSelectedSong(Song song);
    }
}
