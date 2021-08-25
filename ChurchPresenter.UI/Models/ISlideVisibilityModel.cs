using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    public interface ISlideVisibilityModel
    {
        event Action<bool> SlideVisibilityChanged;
        void SetSlideVisible(bool visible);
        bool IsSlideVisible();
    }
}
