using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public interface ISlideVisibilityService
    {
        event Action<bool> SlideVisibilityChanged;
        void SetSlideVisible(bool visible);
        bool IsSlideVisible();
    }
}
