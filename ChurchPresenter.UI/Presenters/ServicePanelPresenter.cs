using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using ChurchPresenter.UI.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Presenters
{
    public interface IServicePanelView : IImportView
    {
        event Action<int> FolderSelected;
        event Action FolderActivated;
        event Action<int> FolderRemoved;
        event Action FolderShitedUp;
        event Action FolderShitedDown;
        event Action FolderMadeTopmost;
        event Action FolderMadeBottommost;

        void AddFolder(IFolder folder);
        void RemoveFolder(int index);
        void EnableShiftUp(bool enabled);
        void EnableShiftDown(bool enabled);
        void UpdateServiceOrder(int[] newOrder);
        void ClearFolders();
    }

    class ServicePanelPresenter
    {
        private IServicePanelView view;
        private IServiceModel model;
        private ISelectedFolderService selectedFolderModel;
        private int selectedIndex;

        public ServicePanelPresenter(
            IServicePanelView view,
            IServiceModel model,
            [KeyFilter("Live")] ISelectedFolderService selectedFolderModel)
        {
            this.view = view;
            this.model = model;
            this.selectedFolderModel = selectedFolderModel;

            model.FolderAdded += item => view.AddFolder(item);
            model.ServiceReordered += view.UpdateServiceOrder;
            model.ServiceCleared += () => view.ClearFolders();

            view.FolderSelected += HandleFolderSelected;
            view.FolderActivated += () => selectedFolderModel.SelectFolder(model.ItemAt(selectedIndex));
            view.FolderRemoved += HandleSongRemoved;

            view.FolderShitedUp += HandleFolderShiftedUp;
            view.FolderShitedDown += HandleFolderShiftedDown;
            view.FolderMadeTopmost += () => model.MakeFolderFirst(selectedIndex);
            view.FolderMadeBottommost += () => model.MakeFolderLast(selectedIndex);

            view.EnableShiftDown(false);
            view.EnableShiftUp(false);
        }

        private void HandleFolderSelected(int index)
        {
            selectedIndex = index;
            view.EnableShiftUp(index != 0);
            view.EnableShiftDown(!LastItemSelected());
        }

        private void HandleSongRemoved(int songIndex)
        {
            model.RemoveFolder(songIndex);
            view.RemoveFolder(songIndex);
        }

        private void HandleFolderShiftedUp()
        {
            if (selectedIndex > 0)
                model.SwapFolderOrder(selectedIndex, selectedIndex - 1);
        }

        private void HandleFolderShiftedDown()
        {
            if (!LastItemSelected())
                model.SwapFolderOrder(selectedIndex, selectedIndex + 1);
        }

        private bool LastItemSelected()
        {
            return selectedIndex == model.ServiceLength() - 1;
        }
    }
}
