using ChurchPresenter.UI.Models.Folder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    class ServiceModel : IServiceModel
    {
        public event Action<IFolder> ItemAdded;
        public event Action<int[]> ServiceReordered;

        private List<IFolder> currentService = new List<IFolder>();

        public IFolder ItemAt(int index)
        {
            return currentService[index];
        }

        public void RemoveFolder(int index)
        {
            currentService.RemoveAt(index);
        }

        public void AddFolder(IFolder folder)
        {
            currentService.Add(folder);
            ItemAdded?.Invoke(folder);
        }

        public IFolder[] GetFolders()
        {
            return currentService.ToArray();
        }

        public int ServiceLength()
        {
            return currentService.Count;
        }

        public void SwapFolderOrder(int first, int second)
        {
            Action<IList> swap = (collection) =>
            {
                var temp = collection[first];
                collection[first] = collection[second];
                collection[second] = temp;
            };

            swap(currentService);
            UpdateIndices(swap);
        }

        public void MakeFolderFirst(int index)
        {
            Action<IList> makeFirst = (collection) =>
            {
                var temp = collection[index];
                collection.RemoveAt(index);
                collection.Insert(0, temp);
            };

            makeFirst(currentService);
            UpdateIndices(makeFirst);
        }

        public void MakeFolderLast(int index)
        {
            Action<IList> makeLast = (collection) =>
            {
                var temp = collection[index];
                collection.RemoveAt(index);
                collection.Add(temp);
            };

            makeLast(currentService);
            UpdateIndices(makeLast);
        }

        private void UpdateIndices(Action<IList> modify)
        {
            var indices = Enumerable.Range(0, currentService.Count).ToList();
            modify(indices);
            ServiceReordered?.Invoke(indices.ToArray());
        }
    }
}
