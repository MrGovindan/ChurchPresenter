using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Models
{
    class SlideVisibilityModel : ISlideVisibilityModel
    {
        private bool visibility = false;

        private event Action<bool> SlideVisibilityChangedImpl;

        public event Action<bool> SlideVisibilityChanged
        {
            add
            {
                SlideVisibilityChangedImpl += value;
                value.Invoke(visibility);
            }
            remove
            {
                SlideVisibilityChangedImpl -= value;
            }
        }

        public bool IsSlideVisible()
        {
            return visibility;
        }

        public void SetSlideVisible(bool visible)
        {
            visibility = visible;
            SlideVisibilityChangedImpl?.Invoke(visible);
        }
    }
}
