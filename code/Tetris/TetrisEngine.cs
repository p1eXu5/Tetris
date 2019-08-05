using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Tetris.Contracts;
using Tetris.Models;
using Tetris.Models.Contracts;

namespace Tetris
{
    public class TetrisEngine : ITetrisEngine, IDisposable
    {
        #region Constants

        private const int MIN_SPEED = 600;
        private const int MAN_SPEED = 100;
        private const int SPEED_STEP = 50;

        #endregion


        #region Fields

        private readonly IFigureFlyweightFactory _factory;
        private readonly IGameField _gameField;
        private readonly Vector _gravityVector = new Vector(0, 1 );
        private readonly Timer _timer;
        private int _speed;
        private int _isRunning;
        private int _timerInvoked;
        private readonly ObservableCollection<(Color?[][] data, int left, int top)> _gameObjectCollection;
        private readonly object _lock = new object();

        #endregion


        #region Ctor

        public TetrisEngine(IFigureFlyweightFactory factory, IGameField graveyard, int speed = MIN_SPEED)
        {
            _factory = factory ?? throw new ArgumentNullException();
            _gameField = graveyard ?? throw new ArgumentNullException();

            _timer = new Timer(OnTimer);
            _speed = speed < MAN_SPEED ? MAN_SPEED : speed > MIN_SPEED ? MIN_SPEED : speed;

            _gameObjectCollection = new ObservableCollection<(Color?[][] data, int left, int top)>();
            GameObjectCollection = new ReadOnlyObservableCollection<(Color?[][] data, int left, int top)>(_gameObjectCollection);

            _gameObjectCollection.Add(_gameField.GetFigureStack());
            _gameObjectCollection.Add(_gameField.GetActiveFigure());

            try
            {
                TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            }
            catch (InvalidOperationException)
            {
                TaskScheduler = TaskScheduler.Default;
            }
        } 

        #endregion



        public event EventHandler< int[] > RemovableLinesFormed;

        public TaskScheduler TaskScheduler { get; set; }

        public bool HasActiveFigure => !_gameField.ActiveFigureGizmo.IsEmptyGizmo;
        public ReadOnlyObservableCollection<(Color?[][] data, int left, int top)> GameObjectCollection { get; }

        public bool IsRunning => _isRunning == 1;

        private void RunGame()
        {
            _gameField.Clear();
            if ( !_gameField.TryAddFigure( _factory.GetNext() ) ) throw new InvalidOperationException("Cannot add a figure at the start of the game");
            _timer.Change( 0, _speed);
        }

        public async Task StartNewGameAsync()
        {
            if ( Interlocked.Exchange( ref _isRunning, 1 ) == 1 ) return;

            await Task.Run( RunGame );
            _gameObjectCollection[1] = _gameField.GetActiveFigure();
        }

        public void StartNewGame()
        {
            RunGame();
            _gameObjectCollection[1] = _gameField.GetActiveFigure();
        }

        private void OnTimer( object o )
        {
            if (Interlocked.Exchange( ref _timerInvoked, 1 ) != 0 ) return;

            var t = new Task( MoveFigureDown );
            t.RunSynchronously( TaskScheduler );
            t.Wait();

            Volatile.Write( ref _timerInvoked, 0 );
        }

        private async void MoveFigureDown()
        {

            bool isLocked = false;
            Monitor.Enter( _lock, ref isLocked );

            var res = await Task.Run( () => {

                var innerRes = new List< int > { -1 };

                if ( _gameField.TryMove( _gravityVector ) ) {
                    return new[] {-1 };
                }

                _gameField.Merge();
                innerRes[0] = -2;
                    
                var removedLines = _gameField.RemoveFilledLines();
                
                if ( removedLines.Any() ) {
                    innerRes[0] = -3;
                    foreach ( var t in removedLines ) { innerRes.Add( t ); }
                }

                if ( !_gameField.TryAddFigure( _factory.GetNext() ) ) {
                    innerRes[0] = -4;
                }

                return innerRes.ToArray();
            } );


            if (res[0] < -1)
            {
                _gameObjectCollection[0] = _gameField.GetFigureStack();
            }

            if (res.Length > 1)
            {
                RemovableLinesFormed?.Invoke(this, res.Skip(1).ToArray());
            }

            _gameObjectCollection[1] = _gameField.GetActiveFigure();

            if (res[0] == -4)
            {
                GameOver();
            }

            if ( isLocked ) Monitor.Exit( _lock );
        }

        public void SpeedDown()
        {
            if ( _speed - MAN_SPEED < SPEED_STEP ) return;
            _speed -= SPEED_STEP;
        }

        public void SpeedUp()
        {
            if (MIN_SPEED - _speed < SPEED_STEP) return;
            _speed += SPEED_STEP;
        }
        public async Task MoveFigureLeftAsync()
        {
            bool isLocked = false;
            Monitor.Enter( _lock, ref isLocked);

            var res = await Task.Run( () => _gameField.TryMove( new Vector( -1.0, 0.0 ) ) );

            if ( res ) {
                _gameObjectCollection[ 1 ] = _gameField.GetActiveFigure();
            }

            if ( isLocked ) Monitor.Exit( _lock );
        }

        public async Task MoveFigureRightAsync()
        {
            bool isLocked = false;
            Monitor.Enter(_lock, ref isLocked);

            var res = await Task.Run(() => _gameField.TryMove(new Vector(1.0, 0.0) ) );

            if (res)
            {
                _gameObjectCollection[1] = _gameField.GetActiveFigure();
            }

            if (isLocked) Monitor.Exit(_lock);
        }

        public async Task RotateFigureClockwiseAsync()
        {
            bool isLocked = false;
            Monitor.Enter(_lock, ref isLocked);

            var res = await Task.Run(() => _gameField.TryRotateFigure( RotateDirections.Clockwise ) );

            if (res)
            {
                _gameObjectCollection[1] = _gameField.GetActiveFigure();
            }

            if (isLocked) Monitor.Exit(_lock);
        }

        public async Task RotateFigureCounterclockwiseAsync()
        {
            bool isLocked = false;
            Monitor.Enter(_lock, ref isLocked);

            var res = await Task.Run(() => _gameField.TryRotateFigure(RotateDirections.Couterclockwise ) );

            if (res)
            {
                _gameObjectCollection[1] = _gameField.GetActiveFigure();
            }

            if (isLocked) Monitor.Exit(_lock);
        }

        public Task DropFigureAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _factory?.Dispose();
            _timer.Dispose();
        }

        private FigureGizmo GetActiveFigureGizmo()
        {
            throw new NotImplementedException();
            //var figure = _factory.GetNext();

            //if ( figure.Width >= _gameField.Width || figure.Height >= _gameField.Height ) {
            //    throw new InvalidOperationException( "Figure size is greater than game field" );
            //}

            //return new FigureGizmo( 
            //               figure, 
            //               new Point( _gameField.Width / 2.0 - figure.Width / 2.0,
            //                          figure.Height / 2.0 ) 
            //           );
        }

        public void GameOver()
        {
            _timer.Change( Timeout.Infinite, Timeout.Infinite );
            _isRunning = 0;
        }

        public int GameFieldWidth => _gameField.Width;
        public int GameFieldHeight => _gameField.Height;
        public Color?[][] GetGameField() => _gameField.GetField();

    }
}
