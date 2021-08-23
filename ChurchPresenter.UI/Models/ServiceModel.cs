using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    class ServiceModel : IServiceModel
    {
        public event Action<Song> SongAdded;

        private List<Song> currentService = new List<Song>();

        public void AddSongToService(Song song)
        {
            currentService.Add(song);
            SongAdded?.Invoke(song);
        }

        public Song ItemAt(int index)
        {
            return currentService[index];
        }
    }
}
