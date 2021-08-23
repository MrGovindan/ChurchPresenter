using System;
using System.Threading.Tasks;
using System.Windows;
using Autofac;

namespace ChurchPresenter.UI
{
    class App : Application
    {
        private WebSocketServer.WebSocketServer server;

        [STAThread]
        public static void Main(string[] args)
        {
            var app = new App();
            app.Run();
        }

        public App()
        {
            Dispatcher.Invoke(() =>
            {
                new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext()).StartNew(() =>
                {
                    var containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterModule(new PresenterModule());
                    containerBuilder.RegisterModule(new ViewModule());
                    containerBuilder.RegisterModule(new ModelModule());
                    var container = containerBuilder.Build();

                    this.MainWindow = container.Resolve<MainWindow>();
                    this.MainWindow.Show();
                });
            });
        }
    }
}
