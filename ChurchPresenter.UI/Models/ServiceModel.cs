using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    class ServiceModel : IServiceModel
    {
        public event Action<IFolder> ItemAdded;

        private List<IFolder> currentService = new List<IFolder>();

        public IFolder ItemAt(int index)
        {
            return currentService[index];
        }

        public void RemoveSongAt(int index)
        {
            currentService.RemoveAt(index);
        }

        public void AddFolder(IFolder folder)
        {
            currentService.Add(folder);
            ItemAdded?.Invoke(folder);
        }
    }
}
