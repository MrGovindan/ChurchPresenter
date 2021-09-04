using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Services
{
    public interface IWriter<T>
    {
        void Write(T data);
    }
}
