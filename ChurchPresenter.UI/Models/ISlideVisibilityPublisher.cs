using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public interface ISlideVisibilityPublisher
    {
        event Action<bool> SlideVisibilityChanged;

        void PublishSlideVisibility(bool visible);
    }
}
