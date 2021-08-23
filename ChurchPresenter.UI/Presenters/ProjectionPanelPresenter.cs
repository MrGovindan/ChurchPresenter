using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Presenters
{
    public interface IProjectionView
    {
        event Action<int> SlideSelected;

        void SetTitle(string title);
        void SetSlides(Slide[] slides);
        void SetPreviewText(string text);
        void SelectSlide(int index);
    }

    class LivePanelPresenter : ProjectionPanelPresenter
    {
        public LivePanelPresenter(
            [KeyFilter("Live")] IProjectionView view,
            [KeyFilter("Live")] ISongSelectedPublisher selectedSongPublisher,
            IProjector projector) : base(view, selectedSongPublisher)
        {
            view.SlideSelected += index => projector.Show(currentSong.slides[index].text);
        }
    }

    class PreviewPanelPresenter : ProjectionPanelPresenter
    {
        public PreviewPanelPresenter(
            [KeyFilter("Preview")] IProjectionView view,
            [KeyFilter("Preview")] ISongSelectedPublisher selectedSongPublisher) : base(view, selectedSongPublisher)
        {
        }
    }

    class ProjectionPanelPresenter
    {
        private IProjectionView view;
        protected Song currentSong;

        public ProjectionPanelPresenter(IProjectionView view, ISongSelectedPublisher selectedSongPublisher)
        {
            this.view = view;

            selectedSongPublisher.SelectedSongChanged += HandleSongSelected;
            view.SlideSelected += index => view.SetPreviewText(currentSong.slides[index].text);
        }

        private void HandleSongSelected(Song song)
        {
            currentSong = song;
            view.SetTitle(song.title);
            view.SetSlides(song.slides);
            view.SelectSlide(0);
        }
    }
}
