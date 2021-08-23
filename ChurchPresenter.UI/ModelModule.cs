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
            builder.RegisterType<SelectedSongPublisher>().Keyed<ISelectedSongPublisher>("Preview").SingleInstance();
            builder.RegisterType<SelectedSongPublisher>().Keyed<ISelectedSongPublisher>("Live").SingleInstance();
            builder.RegisterType<SelectedSlidePublisher>().Keyed<ISelectedSlidePublisher>("Preview").SingleInstance();
            builder.RegisterType<SelectedSlidePublisher>().Keyed<ISelectedSlidePublisher>("Live").SingleInstance();
            builder.RegisterType<ServiceModel>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<WebBrowserProjector>().AsImplementedInterfaces().AutoActivate().SingleInstance().WithAttributeFiltering();
        }
    }
}
