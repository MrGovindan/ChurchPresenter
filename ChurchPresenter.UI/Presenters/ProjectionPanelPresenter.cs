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
        void SetPreviewTextVisible(bool visible);
    }

    class LivePanelPresenter : ProjectionPanelPresenter
    {
        public LivePanelPresenter(
            [KeyFilter("Live")] IProjectionView view,
            [KeyFilter("Live")] ISelectedSongPublisher selectedSongPublisher,
            [KeyFilter("Live")] ISelectedSlidePublisher selectedSlidePublisher,
            ISlideVisibilityPublisher slideVisibilityPublisher)
            : base(view, selectedSongPublisher, selectedSlidePublisher)
        {
            slideVisibilityPublisher.SlideVisibilityChanged += view.SetPreviewTextVisible;
        }
    }

    class PreviewPanelPresenter : ProjectionPanelPresenter
    {
        public PreviewPanelPresenter(
            [KeyFilter("Preview")] IProjectionView view,
            [KeyFilter("Preview")] ISelectedSongPublisher selectedSongPublisher,
            [KeyFilter("Preview")] ISelectedSlidePublisher selectedSlidePublisher)
            : base(view, selectedSongPublisher, selectedSlidePublisher)
        {
        }
    }

    class ProjectionPanelPresenter
    {
        private IProjectionView view;
        protected Song currentSong;
        private ISelectedSlidePublisher selectedSlidePublisher;

        public ProjectionPanelPresenter(
            IProjectionView view,
            ISelectedSongPublisher selectedSongPublisher,
            ISelectedSlidePublisher selectedSlidePublisher)
        {
            this.view = view;
            this.selectedSlidePublisher = selectedSlidePublisher;

            selectedSongPublisher.SelectedSongChanged += HandleSongSelected;
            view.SlideSelected += index => selectedSlidePublisher.PublishSelectedSlide(currentSong.slides[index]);
            selectedSlidePublisher.SelectedSlideChanged += HandleSlideSeleted;
        }


        private void HandleSongSelected(Song song)
        {
            currentSong = song;
            view.SetTitle(song.title);
            view.SetSlides(song.slides);
            selectedSlidePublisher.PublishSelectedSlide(song.slides[0]);
        }
        private void HandleSlideSeleted(Slide selectedSlide)
        {
            view.SelectSlide(GetIndexOfSlide(selectedSlide));
            view.SetPreviewText(selectedSlide.text);
        }

        private int GetIndexOfSlide(Slide selectedSlide) => new List<Slide>(currentSong.slides).IndexOf(selectedSlide);
    }
}
