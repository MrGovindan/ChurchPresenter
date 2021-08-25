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
        void AddFolder(IFolder folder);
        void RemoveSongTitle(int index);
    }

    class ServicePanelPresenter
    {
        private IServicePanelView view;
        private IServiceModel model;

        public ServicePanelPresenter(
            IServicePanelView view,
            IServiceModel model,
            [KeyFilter("Live")] ISelectedFolderModel songSelectedPublisher)
        {
            this.view = view;
            this.model = model;

            model.ItemAdded += item => view.AddFolder(item);
            view.SongSelected += index => songSelectedPublisher.PublishSelectedFolder(model.ItemAt(index));
            view.SongRemoved += HandleSongRemoved;
        }

        private void HandleSongRemoved(int songIndex)
        {
            model.RemoveSongAt(songIndex);
            view.RemoveSongTitle(songIndex);
        }
    }
}
