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
        private const int DROPPING_SPEED = 20;

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
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim( 1, 1 );
        private TaskScheduler _taskScheduler;
        private bool _isDropping;

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

        public TaskScheduler TaskScheduler
        {
            get => _taskScheduler ?? TaskScheduler.Default; 
            private set => _taskScheduler = value;
        }

        public bool CanManipulate => !_gameField.ActiveFigureGizmo.IsEmptyGizmo && IsRunning;
        public bool IsDropping => _isDropping;
        public ReadOnlyObservableCollection<(Color?[][] data, int left, int top)> GameObjectCollection { get; }

        public bool IsRunning => _isRunning == 1;


        public async Task StartNewGameAsync( TaskScheduler taskScheduler )
        {
            await Task.Run( () => RunGame( taskScheduler ) );
        }

        public void UpdateField()
        {
            _semaphore.Wait();
            _gameObjectCollection[0] = _gameField.GetFigureStack();
            _semaphore.Release();
        }

        public void UpdateFigure()
        {
            _semaphore.Wait();
            _gameObjectCollection[1] = _gameField.GetActiveFigure();
            _semaphore.Release();
        }

        private void RunGame( TaskScheduler taskScheduler )
        {
            if ( Interlocked.Exchange( ref _isRunning, 1 ) == 1 ) return;
            _semaphore.Wait();

            if ( taskScheduler != null ) {
                TaskScheduler = taskScheduler;
            }
            _gameField.Clear();
            if ( !_gameField.TryAddFigure( _factory.GetNext() ) ) throw new InvalidOperationException("Cannot add a figure at the start of the game");
            _timer.Change( 0, _speed);

            _semaphore.Release();
        }

        private async void OnTimer( object o )
        {
            if (Interlocked.Exchange( ref _timerInvoked, 1 ) != 0 ) return;

            await Task.Run( MoveFigureDown ).ContinueWith( task => 
            {
                var res = task.Result;

                UpdateFigure();

                if (res[0] < -1) {
                    UpdateField();
                }

                if (res.Length > 1) {
                    RemovableLinesFormed?.Invoke(this, res.Skip(1).ToArray() );
                }

                if (res[0] == -4) {
                    GameOver();
                }
            }, TaskScheduler );

            Volatile.Write( ref _timerInvoked, 0 );
        }

        /// <summary>
        ///     Move the figure down.
        /// </summary>
        /// <returns>
        ///     [-1] if the dropping is continuing;
        ///     [-2] if figure has been merged;
        ///     [-3, removable_lines] if there are removable lines;
        ///     [-4] if can't add a new figure.
        /// </returns>
        private int[] MoveFigureDown()
        {
            _semaphore.Wait();

            var innerRes = new List<int> { -1 };

            if (_gameField.TryMove( DirectionVectors.DownVector )) 
            {
                // the dropping continues 
                _semaphore.Release();
                return new[] { -1 };
            }

            // The figure has been dropped
            if (_isDropping)
            {
                _timer.Change(0, _speed);
                _isDropping = false;
            }

            _gameField.Merge();
            innerRes[0] = -2;

            var removedLines = _gameField.RemoveFilledLines();

            if (removedLines.Any())
            {
                innerRes[0] = -3;
                foreach (var t in removedLines) { innerRes.Add(t); }
            }

            if (!_gameField.TryAddFigure(_factory.GetNext()))
            {
                innerRes[0] = -4;
            }

            _semaphore.Release();
            return innerRes.ToArray();
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

        public async Task< bool > MoveFigureAsync( MoveDirections moveDirection )
        {
            return await Task.Run( () => {
                _semaphore.Wait();
                var res = _gameField.TryMove( DirectionVectors.FromDirection( moveDirection ) );
                _semaphore.Release();
                return res;
            } );
        }

        public async Task< bool > RotateFigureAsync( RotateDirections rotateDirections )
        {
            return await Task.Run(() => {
                _semaphore.Wait();
                var res = _gameField.TryRotateFigure( rotateDirections );
                _semaphore.Release();
                return res;
            });
        }


        public void DropFigure()
        {
            _isDropping = true;
            _timer.Change( 0, DROPPING_SPEED);
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
