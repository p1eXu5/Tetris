using System;
using System.Windows;
using System.Windows.Media;

namespace Tetris.Models
{
    public class GameField : IGameField
    {
        private readonly Color?[,] _field;
        private readonly IFigureGizmoProxy _activeFigureGizmo;

        public GameField( int width, int height )
        {
            this.Width = width > 0 ? width : throw new ArgumentException( "width must be greater than zero" );
            this.Height = height > 0 ? height : throw new ArgumentException("height must be greater than zero");
            _field = new Color?[ height, width ];

            _activeFigureGizmo = new FigureGizmoProxy( FigureGizmo.EmptyGizmo );
        }

        public int Width { get; }
        public int Height { get; }

        public IFigureGizmo ActiveFigureGizmo => _activeFigureGizmo;

        public void Clear()
        {
            for ( int i = 0; i < _field.GetLength( 0 ); i++ ) {
                for ( int j = 0; j < _field.GetLength( 1 ); j++ ) {
                    _field[ i, j ] = (Color?)null;
                }
            }

            ResetActiveFigureGizmo();
        }


        public bool TryAddFigure( ILiveFigureGizmo figureGizmo )
        {
            if ( figureGizmo.Width > this.Width ) throw new ArgumentException( "figureGizmo width must be greater than gamefield width" );
            if ( figureGizmo.Height > this.Height ) throw new ArgumentException( "figureGizmo height must be greater than gamefield height" );

            var center = new Point( Width / 2.0, figureGizmo.Height / 2.0 );
            figureGizmo.MoveTo( center );

            if ( IsOverlay( figureGizmo ) ) return false;

            _activeFigureGizmo.Image =  figureGizmo;

            return true;
        }

        public void Merge()
        {
            if ( _activeFigureGizmo.IsEmptyGizmo ) {
                throw new InvalidOperationException("Figure is Empty");
            }

            var ii = 0;

            for (int i = _activeFigureGizmo.Top; i < _activeFigureGizmo.Bottom; i++)
            {
                var jj = 0;

                for (int j = _activeFigureGizmo.Left; j < _activeFigureGizmo.Right; j++)
                {
                    _field[i, j] = _activeFigureGizmo[ii, jj];
                    ++jj;
                }

                ++ii;
            }

            ResetActiveFigureGizmo();
        }

        private void ResetActiveFigureGizmo() => _activeFigureGizmo.Image = FigureGizmo.EmptyGizmo;

        public bool TryRotateFigure( RotateDirections direction )
        {
            if ( direction == RotateDirections.Clockwise ) {
                _activeFigureGizmo.ClockwiseRotate();
                if ( IsOverlay( _activeFigureGizmo ) ) {
                    _activeFigureGizmo.Image.ClockwiseRotate();
                    return true;
                }
            }
            else {
                _activeFigureGizmo.CounterclockwiseRotate();
                if (IsOverlay( _activeFigureGizmo ) ) {
                    _activeFigureGizmo.Image.CounterclockwiseRotate();
                    return true;
                }
            }

            return false;
        }

        public Color?[,] GetField()
        {
            var figure = _activeFigureGizmo.Image;

            var fieldCopy = new Color?[_field.GetLength(0), _field.GetLength(1)];
            Array.Copy(_field, fieldCopy, _field.GetLength(0) * _field.GetLength(1));

            if (figure != null ) {

                var ii = 0;
                for ( int i = figure.Top; i < figure.Bottom; i++ ) 
                {
                    var jj = 0;

                    for ( int j = figure.Left; j < figure.Right; j++ ) {
                        fieldCopy[ i, j ] = figure[ ii, jj ];
                        ++jj;
                    }

                    ++ii;
                }
            }

            return fieldCopy;
        }

        private bool IsOverlay( IFigureGizmo figureGizmo )
        {
            var ii = 0;

            for ( int i = figureGizmo.Top; i < figureGizmo.Bottom; i++ ) 
            {
                var jj = 0;

                for ( int j = figureGizmo.Left; j < figureGizmo.Right; j++ ) {
                    if ( figureGizmo[ii, jj].HasValue && _field[ i, j ].HasValue ) return true;
                    ++jj;
                }

                ++ii;
            }
            return false;
        }

        public bool TryMove( Vector vector )
        {
            if ( !vector.LengthSquared.Equals( 1.0 ) ) {
                throw new ArgumentException("Vector is wrong");
            }

            if ( _activeFigureGizmo.IsEmptyGizmo ) {
                throw new InvalidOperationException("Figure is Empty");
            }

            if ( vector.Y > 0 ) 
            { 
                if ( _activeFigureGizmo.Bottom + vector.Y >= Height ) return false;

                for ( int j = _activeFigureGizmo.Left, jj = 0; j < _activeFigureGizmo.Right; ++j, ++jj ) 
                {
                    var i = _activeFigureGizmo.Height - 1;
                    while ( i >= 0 && !_activeFigureGizmo[ i, jj ].HasValue ) { --i; }
                    if ( i < 0) continue;
                    if ( _field[ _activeFigureGizmo.Top + i + (int)vector.Y, j ].HasValue ) return false;
                }
            }
            else if ( vector.Y < 0 )
            {
                if ( _activeFigureGizmo.Top + vector.Y < 0 ) return false;

                for (int j = _activeFigureGizmo.Left, jj = 0; j < _activeFigureGizmo.Right; ++j, ++jj)
                {
                    var i = 0;
                    while (i < _activeFigureGizmo.Height && !_activeFigureGizmo[i, jj].HasValue) { ++i; }
                    if (i >= _activeFigureGizmo.Height) continue;
                    if (_field[_activeFigureGizmo.Bottom - 1 - i + (int)vector.Y, j].HasValue) return false;
                }
            }
            else if ( vector.X > 0 ) 
            {
                if ( _activeFigureGizmo.Right + vector.X >= Width ) return false;

                for (int i = _activeFigureGizmo.Top, ii = 0; i < _activeFigureGizmo.Bottom; ++i, ++ii)
                {
                    var j = _activeFigureGizmo.Width - 1;
                    while (j >= 0 && !_activeFigureGizmo[ii, j].HasValue) { --j; }
                    if ( j < 0 ) continue;
                    if ( _field[i, _activeFigureGizmo.Left + j + (int)vector.X].HasValue) return false;
                }
            }
            else if ( vector.X < 0 ) 
            {
                if ( _activeFigureGizmo.Left + vector.X < 0 ) return false;

                for (int i = _activeFigureGizmo.Top, ii = 0; i < _activeFigureGizmo.Bottom; ++i, ++ii)
                {
                    var j = 0;
                    while (j < _activeFigureGizmo.Width && !_activeFigureGizmo[ii, j].HasValue) { ++j; }
                    if (j >= _activeFigureGizmo.Width) continue;
                    if (_field[i, _activeFigureGizmo.Right - 1 - j + (int)vector.X].HasValue) return false;
                }
            }

            _activeFigureGizmo.Center = Point.Add( _activeFigureGizmo.Center, vector );

            return true;
        }

        public int[] RemoveFullLines()
        {
            throw new NotImplementedException();
        }
    }
}
