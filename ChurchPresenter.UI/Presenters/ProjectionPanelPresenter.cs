using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using ChurchPresenter.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChurchPresenter.UI.Presenters
{
    public interface IProjectionView
    {
        event Action<int> SlideSelected;

        void SetTitle(string title);
        void SetSlides(string[] slides);
        void SetPreviewText(Slide slide);
        void SelectSlide(int index);
        void SetPreviewTextVisible(bool visible);
    }

    class LivePanelPresenter : ProjectionPanelPresenter
    {
        public LivePanelPresenter(
            [KeyFilter("Live")] IProjectionView view,
            [KeyFilter("Live")] ISelectedFolderModel selectedFolderModel,
            [KeyFilter("Live")] IDisplayedSlideService selectedSlidePublisher,
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
            [KeyFilter("Preview")] IDisplayedSlideService selectedSlidePublisher)
            : base(view, selectedFolderModel, selectedSlidePublisher)
        {
        }
    }

    class ProjectionPanelPresenter
    {
        private readonly IProjectionView view;
        private readonly IDisplayedSlideService selectedSlidePublisher;
        private IFolder currentFolder;

        public ProjectionPanelPresenter(
            IProjectionView view,
            ISelectedFolderModel selectedFolderModel,
            IDisplayedSlideService selectedSlidePublisher)
        {
            this.view = view;
            this.selectedSlidePublisher = selectedSlidePublisher;

            selectedFolderModel.SelectedFolderChanged += HandleFolderSelected;
            view.SlideSelected += index => selectedSlidePublisher.DisplaySlide(currentFolder.GetSlides()[index]);
            selectedSlidePublisher.DisplayedSlideChanged += HandleSlideSeleted;
        }


        private void HandleFolderSelected(IFolder folder)
        {
            currentFolder = folder;
            view.SetTitle(folder.GetTitle());
            view.SetSlides(folder.GetSlides().Select(s => string.Join(" ", s.GetParts().Select(p => p.Text))).ToArray());
            selectedSlidePublisher.DisplaySlide(folder.GetSlides()[0]);
        }

        private void HandleSlideSeleted(Slide selectedSlide)
        {
            view.SelectSlide(GetIndexOfSlide(selectedSlide));
            view.SetPreviewText(selectedSlide);
        }

        private int GetIndexOfSlide(Slide selectedSlide) => new List<Slide>(currentFolder.GetSlides()).IndexOf(selectedSlide);
    }
}
