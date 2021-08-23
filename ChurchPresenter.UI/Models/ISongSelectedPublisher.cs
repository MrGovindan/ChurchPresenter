using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public interface ISongSelectedPublisher
    {
        event Action<Song> SelectedSongChanged;
        void PublishSelectedSong(Song song);
    }
}
