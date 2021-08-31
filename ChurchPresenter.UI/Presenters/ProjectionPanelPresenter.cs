using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
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
        void SetPreviewText(Slide slide);
        void SelectSlide(int index);
        void SetPreviewTextVisible(bool visible);
    }

    class LivePanelPresenter : ProjectionPanelPresenter
    {
        public LivePanelPresenter(
            [KeyFilter("Live")] IProjectionView view,
            [KeyFilter("Live")] ISelectedFolderModel selectedFolderModel,
            [KeyFilter("Live")] ISelectedSliderPublisher selectedSlidePublisher,
            ISlideVisibilityModel slideVisibilityPublisher)
            : base(view, selectedFolderModel, selectedSlidePublisher)
        {
            slideVisibilityPublisher.SlideVisibilityChanged += view.SetPreviewTextVisible;
        }
    }

    class PreviewPanelPresenter : ProjectionPanelPresenter
    {
        public PreviewPanelPresenter(
            [KeyFilter("Preview")] IProjectionView view,
            [KeyFilter("Preview")] ISelectedFolderModel selectedFolderModel,
            [KeyFilter("Preview")] ISelectedSliderPublisher selectedSlidePublisher)
            : base(view, selectedFolderModel, selectedSlidePublisher)
        {
        }
    }

    class ProjectionPanelPresenter
    {
        private IProjectionView view;
        protected IFolder currentFolder;
        private ISelectedSliderPublisher selectedSlidePublisher;

        public ProjectionPanelPresenter(
            IProjectionView view,
            ISelectedFolderModel selectedFolderModel,
            ISelectedSliderPublisher selectedSlidePublisher)
        {
            this.view = view;
            this.selectedSlidePublisher = selectedSlidePublisher;

            selectedFolderModel.SelectedFolderChanged += HandleFolderSelected;
            view.SlideSelected += index => selectedSlidePublisher.PublishSelectedSlide(currentFolder.GetSlides()[index]);
            selectedSlidePublisher.SelectedSlideChanged += HandleSlideSeleted;
        }


        private void HandleFolderSelected(IFolder folder)
        {
            currentFolder = folder;
            view.SetTitle(folder.GetTitle());
            view.SetSlides(folder.GetSlides());
            selectedSlidePublisher.PublishSelectedSlide(folder.GetSlides()[0]);
        }

        private void HandleSlideSeleted(Slide selectedSlide)
        {
            view.SelectSlide(GetIndexOfSlide(selectedSlide));
            view.SetPreviewText(selectedSlide);
        }

        private int GetIndexOfSlide(Slide selectedSlide) => new List<Slide>(currentFolder.GetSlides()).IndexOf(selectedSlide);
    }
}
