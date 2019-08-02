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
        private const int MAX_SPEED = 600;
        private const int MIN_SPEED = 100;
        private const int SPEED_STEP = 50;

        private readonly IFigureFlyweightFactory _factory;
        private readonly IGameField _gameField;
        private readonly Vector _gravityVector = new Vector(0, 1 );
        private readonly Timer _timer;
        private int _speed;
        private int _isRunning;
        private int _timerInvoked;
        private readonly ObservableCollection<(Color?[][] data, int left, int top)> _gameObjectCollection;

        public TetrisEngine( IFigureFlyweightFactory factory, IGameField graveyard, int speed = MAX_SPEED )
        {
            _factory = factory ?? throw new ArgumentNullException();
            _gameField = graveyard ?? throw new ArgumentNullException();

            _timer = new Timer( OnTimer );
            _speed = speed < MIN_SPEED ? MIN_SPEED : speed > MAX_SPEED ? MAX_SPEED : speed;

            _gameObjectCollection = new ObservableCollection< (Color?[][] data, int left, int top) >();
            GameObjectCollection = new ReadOnlyObservableCollection< (Color?[][] data, int left, int top) >( _gameObjectCollection );

            _gameObjectCollection.Add( _gameField.GetFigureStack() );
            _gameObjectCollection.Add( _gameField.GetActiveFigure() );

            TaskScheduler = TaskScheduler.Default;
        }

        public event EventHandler< int[] > RemovableLinesFormed;

        public TaskScheduler TaskScheduler { get; set; }

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

            Task.Run( TakeFigureDown ).ContinueWith( 
                task => 
                {
                    var res = task.Result;

                    if ( res[ 0 ] < -1 ) {
                        _gameObjectCollection[ 0 ] = _gameField.GetFigureStack();
                    }

                    if ( res.Length > 1 ) {
                        RemovableLinesFormed?.Invoke( this, res.Skip( 1 ).ToArray() );
                    }

                    _gameObjectCollection[ 1 ] = _gameField.GetActiveFigure();

                    if ( res[ 0 ] == -4 ) {
                        GameOver();
                    }

                    Volatile.Write( ref _timerInvoked, 0 );
                },
                TaskScheduler
            );

        }

        public int[] TakeFigureDown()
        {
            var res = new List< int > { -1 };

            if ( _gameField.TryMove( _gravityVector ) ) {
                return new[]{-1};
            }

            _gameField.Merge();
            res[0] = -2;

            var removedLines = _gameField.RemoveFilledLines();
            if ( removedLines.Any() ) {
                res[0] = -3;
                foreach ( var t in removedLines ) { res.Add( t ); }
            }

            if ( !_gameField.TryAddFigure( _factory.GetNext() ) ) {
                return new[] { -4 };
            }

            return res.ToArray();
        }


        public void SpeedDown()
        {
            if ( _speed - MIN_SPEED < SPEED_STEP ) return;
            _speed -= SPEED_STEP;
        }

        public void SpeedUp()
        {
            if (MAX_SPEED - _speed < SPEED_STEP) return;
            _speed += SPEED_STEP;
        }
        public void MoveFigureLeft()
        {
            throw new NotImplementedException();
        }

        public void MoveFigureRight()
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
