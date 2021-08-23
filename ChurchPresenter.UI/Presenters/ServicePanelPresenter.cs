using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Presenters
{
    public interface IServicePanelView
    {
        event Action<int> SongSelected;
        event Action<int> SongRemoved;
        void AddSongTitle(string name);
        void RemoveSongTitle(int index);
    }

    class ServicePanelPresenter
    {
        private IServicePanelView view;
        private IServiceModel model;

        public ServicePanelPresenter(
            IServicePanelView view,
            IServiceModel model,
            [KeyFilter("Live")] ISelectedSongPublisher songSelectedPublisher)
        {
            this.view = view;
            this.model = model;

            model.SongAdded += song => view.AddSongTitle(song.title);
            view.SongSelected += index => songSelectedPublisher.PublishSelectedSong(model.ItemAt(index));
            view.SongRemoved += index => view.RemoveSongTitle(index);
        }
    }
}
