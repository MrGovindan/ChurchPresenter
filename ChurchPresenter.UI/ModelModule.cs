using Autofac;
using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchPresenter.UI
{
    class ModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SongLibrary>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<BibleLibrary>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SelectedFolderModel>().Keyed<ISelectedFolderModel>("Preview").SingleInstance();
            builder.RegisterType<SelectedFolderModel>().Keyed<ISelectedFolderModel>("Live").SingleInstance();
            builder.RegisterType<SelectedSlidePublisher>().Keyed<ISelectedSliderPublisher>("Preview").SingleInstance();
            builder.RegisterType<SelectedSlidePublisher>().Keyed<ISelectedSliderPublisher>("Live").SingleInstance();
            builder.RegisterType<SlideVisibilityModel>().As<ISlideVisibilityModel>().SingleInstance();
            builder.RegisterType<ServiceModel>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<WebBrowserProjector>().AsImplementedInterfaces().AutoActivate().SingleInstance().WithAttributeFiltering();
        }
    }
}
