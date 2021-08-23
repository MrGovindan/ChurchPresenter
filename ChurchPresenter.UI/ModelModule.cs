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
            builder.RegisterType<SongSelectedPublisher>().Keyed<ISongSelectedPublisher>("Preview").SingleInstance();
            builder.RegisterType<SongSelectedPublisher>().Keyed<ISongSelectedPublisher>("Live").SingleInstance();
            builder.RegisterType<ServiceModel>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<LiveProjector>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
