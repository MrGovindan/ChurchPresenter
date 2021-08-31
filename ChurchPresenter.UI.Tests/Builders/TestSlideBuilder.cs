using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Models.Folder;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI.Tests.Builders
{
    internal class TestSlideBuilder
    {
        private List<Slide> slides = new List<Slide>();

        internal TestSlideBuilder WithNSlides(int n)
        {
            slides = new List<Slide>();
            for (int i = 0; i < n; ++i)
                slides.Add(new Slide(i.ToString()));
            return this;
        }

        internal Slide[] Build()
        {
            return slides.ToArray();
        }

        internal TestSlideBuilder WithRandomSlides()
        {
            return WithNSlides((int)(new Random().NextDouble() * 11));
        }
    }
}
