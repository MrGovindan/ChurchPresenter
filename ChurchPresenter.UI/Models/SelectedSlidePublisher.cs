using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    class SelectedSlidePublisher : ISelectedSliderPublisher
    {
        public event Action<Slide> SelectedSlideChanged;

        public void PublishSelectedSlide(Slide slide)
        {
            SelectedSlideChanged?.Invoke(slide);
        }
    }
}
