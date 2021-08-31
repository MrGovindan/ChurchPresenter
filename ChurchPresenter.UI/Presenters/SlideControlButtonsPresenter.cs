using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
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
        protected IFolder selectedSong = new LyricFolderBuilder().Build();
        int currentSlideIndex = 0;

        public SlideControlButtonsPresenter(
            ISlideControlButtonsView view,
            ISelectedFolderModel selectedSongPublisher,
            ISelectedSliderPublisher selectedSlidePublisher)
        {
            this.view = view;

            view.SetPreviousSlideButtonEnabled(false);
            view.SetNextSlideButtonEnabled(false);

            view.GoToNextSlide += () => selectedSlidePublisher.PublishSelectedSlide(selectedSong.GetSlides()[++currentSlideIndex]);
            view.GoToPreviousSlide += () => selectedSlidePublisher.PublishSelectedSlide(selectedSong.GetSlides()[--currentSlideIndex]);

            selectedSongPublisher.SelectedFolderChanged += HandleSongChanged;
            selectedSlidePublisher.SelectedSlideChanged += HandleSlideChanged;
        }

        private void HandleSongChanged(IFolder selectedSong)
        {
            this.selectedSong = selectedSong;
        }

        private void HandleSlideChanged(Slide selectedSlide)
        {
            currentSlideIndex = GetIndexOfSlideInSong(selectedSlide);
            view.SetPreviousSlideButtonEnabled(currentSlideIndex != 0);
            view.SetNextSlideButtonEnabled(currentSlideIndex != selectedSong.GetSlides().Length - 1);
        }

        private int GetIndexOfSlideInSong(Slide selectedSlide)
        {
            return new List<Slide>(selectedSong.GetSlides()).IndexOf(selectedSlide);
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
        public LiveSlideControlButtonsPresenter(
            ILiveSlideControlButtonsView view,
            [KeyFilter("Live")] ISelectedFolderModel selectedSongPublisher,
            [KeyFilter("Live")] ISelectedSliderPublisher selectedSlidePublisher,
            ISlideVisibilityModel visibilityModel) : base(view, selectedSongPublisher, selectedSlidePublisher)
        {
            visibilityModel.SlideVisibilityChanged += visible =>
            {
                view.SetHideSlideButtonEnabled(visible);
                view.SetShowSlideButtonEnabled(!visible);
            };
            view.SlideShown += () => visibilityModel.SetSlideVisible(true);
            view.SlideHidden += () => visibilityModel.SetSlideVisible(false);
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
            [KeyFilter("Preview")] ISelectedFolderModel selectedSongPublisher,
            [KeyFilter("Preview")] ISelectedSliderPublisher selectedSlidePublisher,
            IServiceModel serviceModel,
            [KeyFilter("Live")] ISelectedFolderModel liveSongSelectionPublisher) : base(view, selectedSongPublisher, selectedSlidePublisher)
        {
            view.SongAddedToService += () => serviceModel.AddFolder(selectedSong);
            view.SongShownOnLive += () => liveSongSelectionPublisher.PublishSelectedFolder(selectedSong);
        }
    }
}
