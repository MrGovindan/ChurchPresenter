using ChurchPresenter.UI.Models.Folder;
using System;

namespace ChurchPresenter.UI.Services
{
    class DisplayedSlideService : IDisplayedSlideService
    {
        public event Action<Slide> DisplayedSlideChanged;

        public void DisplaySlide(Slide slide)
        {
            DisplayedSlideChanged?.Invoke(slide);
        }
    }
}
