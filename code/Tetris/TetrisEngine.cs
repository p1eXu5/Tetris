using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Tetris.Models;
using Tetris.Models.Contracts;

namespace Tetris
{
    public class TetrisEngine : IDisposable
    {
        private const int MAX_SPEED = 600;
        private const int MIN_SPEED = 100;
        private const int SPEED_STEP = 50;

        private readonly IFigureFlyweightFactory _factory;
        private readonly IGameField _gameField;
        private readonly Vector _gravityVector = new Vector(0, 1 );
        private readonly Timer _timer;
        private int _speed;

        public TetrisEngine( IFigureFlyweightFactory factory, IGameField graveyard, int speed = MAX_SPEED )
        {
            _factory = factory ?? throw new ArgumentNullException();
            _gameField = graveyard ?? throw new ArgumentNullException();

            _timer = new Timer( TakeFigureDown );
            _speed = speed;
        }

        public void StartNewGame()
        {
            _gameField.Clear();
            if ( !_gameField.TryAddFigure( _factory.GetNext() ) ) throw new InvalidOperationException("Cannot add a figure at the start of the game");
            _timer.Change( 0, _speed);
        }


        public void TakeFigureDown( object o )
        {
            if ( _gameField.TryMove( _gravityVector ) ) {
                return;
            }

            _gameField.Merge();

            var removedLines = _gameField.RemoveFilledLines();
            if ( removedLines.Any() ) { 

            }

            if ( !_gameField.TryAddFigure( _factory.GetNext() ) ) {
                GameOver();
            }
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
            throw new NotImplementedException();
        }
    }
}
