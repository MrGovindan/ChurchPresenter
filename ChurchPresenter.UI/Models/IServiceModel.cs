using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public interface IServiceModel
    {
        event Action<Song> SongAdded;
        void AddSongToService(Song song);

        Song ItemAt(int index);
    }
}
