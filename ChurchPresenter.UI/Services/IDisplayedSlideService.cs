using ChurchPresenter.UI.Models.Folder;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Services
{
    public interface IDisplayedSlideService
    {
        event Action<Slide> DisplayedSlideChanged;
        void DisplaySlide(Slide slide);
    }
}
