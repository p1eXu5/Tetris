using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Agbm.Wpf.MvvmBaseLibrary;
using Tetris.Engine;
using Tetris.Engine.Contracts;
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

        protected override async void OnStartup( StartupEventArgs e )
        {
            SetupServices();

            var wnd = new MainWindow { DataContext = _container.Resolve< MainViewModel >() };
            wnd.Show();

            await ((MvvmAsyncCommand)(( MainViewModel )wnd.DataContext).StartGameCommand).ExecuteAsync();
        }

        private void SetupServices()
        {
            _container.RegisterType< IFigureFlyweightFactory, FigureFlyweightFactory >();
            _container.RegisterType< IVectorSpinner, VectorSpinner >();
            _container.RegisterType< IGameField, GameField >();
            _container.RegisterType< ITetrisEngine, TetrisEngine >();
            _container.RegisterType< MainViewModel >();
        }
    }
}
