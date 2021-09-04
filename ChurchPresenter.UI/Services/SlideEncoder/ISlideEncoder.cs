using ChurchPresenter.UI.Models.Folder;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Services.SlideEncoder
{
    public interface ISlideEncoder
    {
        string Encode(Slide slide);
    }
}
