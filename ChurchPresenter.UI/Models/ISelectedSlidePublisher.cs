using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public interface ISelectedSlidePublisher
    {
        event Action<Slide> SelectedSlideChanged;
        void PublishSelectedSlide(Slide slide);
    }
}
