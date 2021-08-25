using Autofac;
using Autofac.Features.AttributeFilters;
using ChurchPresenter.UI.Models;
using ChurchPresenter.UI.Presenters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ChurchPresenter.UI
{
    class PresenterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LibraryPresenter<Panel>>().SingleInstance().AutoActivate();
            builder.RegisterType<SongLibraryPresenter>().SingleInstance().AutoActivate().WithAttributeFiltering();
            builder.RegisterType<BibleLibraryPresenter>().SingleInstance().AutoActivate().WithAttributeFiltering();
            builder.RegisterType<LivePanelPresenter>().SingleInstance().AutoActivate().WithAttributeFiltering();
            builder.RegisterType<PreviewPanelPresenter>().SingleInstance().AutoActivate().WithAttributeFiltering();
            builder.RegisterType<ServicePanelPresenter>().SingleInstance().AutoActivate().WithAttributeFiltering();
            builder.RegisterType<LiveSlideControlButtonsPresenter>().SingleInstance().AutoActivate().WithAttributeFiltering();
            builder.RegisterType<PreviewSlideControlButtonsPresenter>().SingleInstance().AutoActivate().WithAttributeFiltering();
        }
    }
}
