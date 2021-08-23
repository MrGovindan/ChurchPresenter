using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Presenters
{
    public interface ISlideControlButtonsView
    {
        event Action GoToPreviousSlide;
        event Action GoToNextSlide;
        void SetPreviousSlideButtonEnabled(bool enabled);
        void SetNextSlideButtonEnabled(bool enabled);
    }

    class SlideControlButtonsPresenter
    {
        private readonly ISlideControlButtonsView view;
        protected Song selectedSong = new SongBuilder().Build();
        int currentSlideIndex = 0;

        public SlideControlButtonsPresenter(
            ISlideControlButtonsView view,
            ISelectedSongPublisher selectedSongPublisher,
            ISelectedSlidePublisher selectedSlidePublisher)
        {
            this.view = view;

            view.SetPreviousSlideButtonEnabled(false);
            view.SetNextSlideButtonEnabled(false);

            view.GoToNextSlide += () => selectedSlidePublisher.PublishSelectedSlide(selectedSong.slides[++currentSlideIndex]);
            view.GoToPreviousSlide += () => selectedSlidePublisher.PublishSelectedSlide(selectedSong.slides[--currentSlideIndex]);

            selectedSongPublisher.SelectedSongChanged += HandleSongChanged;
            selectedSlidePublisher.SelectedSlideChanged += HandleSlideChanged;
        }

        private void HandleSongChanged(Song selectedSong)
        {
            this.selectedSong = selectedSong;
        }

        private void HandleSlideChanged(Slide selectedSlide)
        {
            currentSlideIndex = GetIndexOfSlideInSong(selectedSlide);
            view.SetPreviousSlideButtonEnabled(currentSlideIndex != 0);
            view.SetNextSlideButtonEnabled(currentSlideIndex != selectedSong.slides.Length - 1);
        }

        private int GetIndexOfSlideInSong(Slide selectedSlide)
        {
            return new List<Slide>(selectedSong.slides).IndexOf(selectedSlide);
        }
    }
    public interface ILiveSlideControlButtonsView : ISlideControlButtonsView
    {
        event Action SlideShown;
        event Action SlideHidden;

        void SetShowSlideButtonEnabled(bool enabled);
        void SetHideSlideButtonEnabled(bool enabled);
    }

    class LiveSlideControlButtonsPresenter : SlideControlButtonsPresenter
    {
        private readonly IPreviewSlideControlButtonsView view;
        private bool firstSongSelected = false;

        public LiveSlideControlButtonsPresenter(
            ILiveSlideControlButtonsView view,
            [KeyFilter("Live")] ISelectedSongPublisher selectedSongPublisher,
            [KeyFilter("Live")] ISelectedSlidePublisher selectedSlidePublisher,
            ISlideVisibilityPublisher visibilityPublisher) : base(view, selectedSongPublisher, selectedSlidePublisher)
        {
            view.SetHideSlideButtonEnabled(false);
            view.SetShowSlideButtonEnabled(false);

            view.SlideHidden += () =>
            {
                visibilityPublisher.PublishSlideVisibility(false);
                view.SetHideSlideButtonEnabled(false);
                view.SetShowSlideButtonEnabled(true);
            };
            view.SlideShown += () =>
            {
                visibilityPublisher.PublishSlideVisibility(true);
                view.SetShowSlideButtonEnabled(false);
                view.SetHideSlideButtonEnabled(true);
            };
            selectedSongPublisher.SelectedSongChanged += song =>
            {
                if (!firstSongSelected)
                {
                    firstSongSelected = true;
                    view.SetHideSlideButtonEnabled(true);
                }
            };
        }
    }

    public interface IPreviewSlideControlButtonsView : ISlideControlButtonsView
    {
        event Action SongAddedToService;
        event Action SongShownOnLive;
    }

    class PreviewSlideControlButtonsPresenter : SlideControlButtonsPresenter
    {
        public PreviewSlideControlButtonsPresenter(
            IPreviewSlideControlButtonsView view,
            [KeyFilter("Preview")] ISelectedSongPublisher selectedSongPublisher,
            [KeyFilter("Preview")] ISelectedSlidePublisher selectedSlidePublisher,
            IServiceModel serviceModel,
            [KeyFilter("Live")] ISelectedSongPublisher liveSongSelectionPublisher) : base(view, selectedSongPublisher, selectedSlidePublisher)
        {
            view.SongAddedToService += () => serviceModel.AddSongToService(selectedSong);
            view.SongShownOnLive += () => liveSongSelectionPublisher.PublishSelectedSong(selectedSong);
        }
    }
}
