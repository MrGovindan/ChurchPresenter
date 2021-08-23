using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    class SlideVisibilityPublisher : ISlideVisibilityPublisher
    {
        public event Action<bool> SlideVisibilityChanged;

        public void PublishSlideVisibility(bool visible)
        {
            SlideVisibilityChanged?.Invoke(visible);
        }
    }
}
