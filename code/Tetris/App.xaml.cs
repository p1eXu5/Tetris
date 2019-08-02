using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tetris.Contracts;
using Tetris.Models;
using Tetris.Models.Contracts;
using Unity;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IUnityContainer _container = new UnityContainer();

        protected override void OnStartup( StartupEventArgs e )
        {
            SetupServices();

            var wnd = new MainWindow { DataContext = _container.Resolve< MainViewModel >() };
            wnd.Show();
        }

        private void SetupServices()
        {
            _container.RegisterType< IFigureFlyweightFactory, FigureFlyweightFactory >();
            _container.RegisterType< ITetrisEngine, TetrisEngine >();
            _container.RegisterType< MainViewModel >();
        }
    }
}
