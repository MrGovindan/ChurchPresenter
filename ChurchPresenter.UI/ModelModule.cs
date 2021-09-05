using Autofac;
using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Services;
using ChurchPresenter.UI.Services.Import;
using ChurchPresenter.UI.Services.SlideEncoder;
using ChurchPresenter.UI.Services.WebSocket;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ChurchPresenter.UI
{
    class ModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SongLibrary>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<BibleLibrary>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<SelectedFolderService>()
                .Keyed<ISelectedFolderService>("Preview")
                .SingleInstance();

            builder.RegisterType<SelectedFolderService>()
                .Keyed<ISelectedFolderService>("Live")
                .SingleInstance();

            builder.RegisterType<DisplayedSlideService>()
                .Keyed<IDisplayedSlideService>("Preview")
                .SingleInstance();

            builder.RegisterType<DisplayedSlideService>()
                .Keyed<IDisplayedSlideService>("Live")
                .SingleInstance();

            builder.RegisterType<SlideVisibilityService>()
                .As<ISlideVisibilityService>()
                .SingleInstance();

            builder.RegisterType<ServiceModel>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<WebBrowserProjector>()
                .AsImplementedInterfaces()
                .AutoActivate()
                .SingleInstance()
                .WithAttributeFiltering()
                .WithParameter("slideEncoder", new HtmlSlideEncoder());

            builder.RegisterType<WebSocketServer>()
                .AsImplementedInterfaces()
                .Keyed<IWriter<string>>("WebSocketServer")
                .WithParameter("endPoint", new IPEndPoint(IPAddress.Loopback, 5000));

            builder.RegisterType<DialogFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<OpenLpServiceParser>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<FileImporter>()
                .SingleInstance()
                .Keyed<IWriter<string>>("FileImporter");

            builder.RegisterType<FileSystem>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
