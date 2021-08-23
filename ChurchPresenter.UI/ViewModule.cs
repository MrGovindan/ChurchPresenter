using Autofac;
using ChurchPresenter.UI.Presenters;
using ChurchPresenter.UI.WpfViews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ChurchPresenter.UI
{
    public class PreviewProjectionView : ProjectionView
    {
        public PreviewProjectionView() : base("Preview")
        {
        }
    }

    public class LiveProjectionView : ProjectionView
    {
        public LiveProjectionView() : base("Live")
        {
        }
    }

    class ViewModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SongLibraryView>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<BibleLibraryView>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<LibraryView>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ServiceView>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<LibraryContentPanelFactory>().As<ILibraryContentFactory<Panel>>().SingleInstance();
            builder.RegisterType<PreviewProjectionView>().AsSelf().Keyed<IProjectionView>("Preview").SingleInstance();
            builder.RegisterType<LiveProjectionView>().AsSelf().Keyed<IProjectionView>("Live").SingleInstance();
            builder.RegisterType<MainWindow>().SingleInstance();
        }
    }

    internal class LibraryContentPanelFactory : ILibraryContentFactory<Panel>
    {
        private readonly SongLibraryView songLibraryContent;
        private readonly BibleLibraryView bibleLibraryContent;

        public LibraryContentPanelFactory(SongLibraryView songLibraryContent, BibleLibraryView bibleLibraryContent)
        {
            this.songLibraryContent = songLibraryContent;
            this.bibleLibraryContent = bibleLibraryContent;
        }

        public Panel GetContent(LibraryContent contentType)
        {
            switch (contentType)
            {
                case LibraryContent.Songs:
                    return songLibraryContent;
                case LibraryContent.Bibles:
                    return bibleLibraryContent;
            }
            return null;
        }
    }
}
