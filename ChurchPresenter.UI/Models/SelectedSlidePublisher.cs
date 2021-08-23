using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    class SelectedSlidePublisher : ISelectedSlidePublisher
    {
        public event Action<Slide> SelectedSlideChanged;

        public SelectedSlidePublisher()
        {
            Debug.WriteLine("SelectedSlidePublisher created");
        }

        public void PublishSelectedSlide(Slide slide)
        {
            SelectedSlideChanged?.Invoke(slide);
        }
    }
}
