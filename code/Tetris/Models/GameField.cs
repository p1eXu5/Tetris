using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Tetris.Helpers;

namespace Tetris.Models
{
    public class GameField : IGameField
    {
        private readonly List< Color?[] > _field;
        private readonly IFigureGizmoProxy _activeFigureGizmoProxy;

        public GameField( int width, int height )
        {
            this.Width = width > 0 ? width : throw new ArgumentException( "width must be greater than zero" );
            this.Height = height > 0 ? height : throw new ArgumentException("height must be greater than zero");
            _field = new List< Color?[] >( height );
            height.ForEach( () => _field.Add( new Color?[width] ) );

            _activeFigureGizmoProxy = new FigureGizmoProxy( FigureGizmo.EmptyGizmo );
        }

        public int Width { get; }
        public int Height { get; }

        public IFigureGizmo ActiveFigureGizmo => _activeFigureGizmoProxy.Image;

        public void Clear()
        {
            foreach ( var t in _field ) {
                for ( int j = 0; j < t.Length; j++ ) {
                    t[ j ] = (Color?)null;
                }
            }

            ResetActiveFigureGizmo();
        }
        public bool TryAddFigure( ILiveFigureGizmo figureGizmo )
        {
            if ( figureGizmo.IsEmptyGizmo ) throw new ArgumentException( "figureGizmo has empty figure" );
            if ( figureGizmo.Width > this.Width ) throw new ArgumentException( "figureGizmo width must be greater than gamefield width" );
            if ( figureGizmo.Height > this.Height ) throw new ArgumentException( "figureGizmo height must be greater than gamefield height" );

            var center = new Point( Width / 2.0, figureGizmo.Height / 2.0 );
            figureGizmo.MoveTo( center );

            if ( IsOverlay( figureGizmo ) ) return false;

            _activeFigureGizmoProxy.Image =  figureGizmo;

            return true;
        }
        public void Merge()
        {
            if ( ActiveFigureGizmo.IsEmptyGizmo ) {
                throw new InvalidOperationException("Figure is Empty");
            }

            for (int i = ActiveFigureGizmo.Top, ii = 0; i < ActiveFigureGizmo.Bottom; i++, ii++)
            {

                for (int j = ActiveFigureGizmo.Left, jj = 0; j < ActiveFigureGizmo.Right; j++, jj++)
                {
                    _field[ i ][ j ] = ActiveFigureGizmo[ii, jj];
                }
            }

            ResetActiveFigureGizmo();
        }
        private void ResetActiveFigureGizmo() => _activeFigureGizmoProxy.Image = FigureGizmo.EmptyGizmo;
        public bool TryRotateFigure( RotateDirections direction )
        {
            if ( direction == RotateDirections.Clockwise ) {
                _activeFigureGizmoProxy.ClockwiseRotate();
                if ( IsOverlay( _activeFigureGizmoProxy ) ) {
                    _activeFigureGizmoProxy.Image.ClockwiseRotate();
                    return true;
                }
            }
            else {
                _activeFigureGizmoProxy.CounterclockwiseRotate();
                if (IsOverlay( _activeFigureGizmoProxy ) ) {
                    _activeFigureGizmoProxy.Image.CounterclockwiseRotate();
                    return true;
                }
            }

            return false;
        }
        public Color?[][] GetField()
        {
            var fieldCopy = _field.ToArray();

            if ( !ActiveFigureGizmo.IsEmptyGizmo ) 
            {
                for ( int i = ActiveFigureGizmo.Top, ii = 0; i < ActiveFigureGizmo.Bottom; i++, ii++ ) {
                    for ( int j = ActiveFigureGizmo.Left, jj = 0; j < ActiveFigureGizmo.Right; j++, jj++ ) {
                        fieldCopy[ i ][ j ] = ActiveFigureGizmo[ ii, jj ];
                    }
                }
            }

            return fieldCopy;
        }
        private bool IsOverlay( IFigureGizmo figureGizmo )
        {
            for ( int i = figureGizmo.Top, ii = 0; i < figureGizmo.Bottom; i++, ii++ ) {
                for ( int j = figureGizmo.Left, jj = 0; j < figureGizmo.Right; j++, jj++ ) {
                    if ( figureGizmo[ii, jj].HasValue && _field[ i ][ j ].HasValue ) return true;
                }
            }
            return false;
        }
        public bool TryMove( Vector vector )
        {
            if ( !vector.LengthSquared.Equals( 1.0 ) ) {
                throw new ArgumentException("Vector is wrong");
            }

            if ( ActiveFigureGizmo.IsEmptyGizmo ) {
                return false;
            }

            if ( vector.Y > 0 ) 
            { 
                if (ActiveFigureGizmo.Bottom + vector.Y >= Height ) return false;

                for ( int j = ActiveFigureGizmo.Left, jj = 0; j < ActiveFigureGizmo.Right; ++j, ++jj ) 
                {
                    var i = ActiveFigureGizmo.Height - 1;
                    while ( i >= 0 && !ActiveFigureGizmo[ i, jj ].HasValue ) { --i; }
                    if ( i < 0) continue;
                    if ( _field[ActiveFigureGizmo.Top + i + (int)vector.Y][ j ].HasValue ) return false;
                }
            }
            else if ( vector.Y < 0 )
            {
                if (ActiveFigureGizmo.Top + vector.Y < 0 ) return false;

                for (int j = ActiveFigureGizmo.Left, jj = 0; j < ActiveFigureGizmo.Right; ++j, ++jj)
                {
                    var i = 0;
                    while (i < ActiveFigureGizmo.Height && !ActiveFigureGizmo[i, jj].HasValue) { ++i; }
                    if (i >= ActiveFigureGizmo.Height) continue;
                    if (_field[ActiveFigureGizmo.Bottom - 1 - i + (int)vector.Y][ j].HasValue) return false;
                }
            }
            else if ( vector.X > 0 ) 
            {
                if (ActiveFigureGizmo.Right + vector.X >= Width ) return false;

                for (int i = ActiveFigureGizmo.Top, ii = 0; i < ActiveFigureGizmo.Bottom; ++i, ++ii)
                {
                    var j = ActiveFigureGizmo.Width - 1;
                    while (j >= 0 && !ActiveFigureGizmo[ ii, j ].HasValue) { --j; }
                    if ( j < 0 ) continue;
                    if ( _field[ i ][ ActiveFigureGizmo.Left + j + (int)vector.X ].HasValue) return false;
                }
            }
            else if ( vector.X < 0 ) 
            {
                if (ActiveFigureGizmo.Left + vector.X < 0 ) return false;

                for (int i = ActiveFigureGizmo.Top, ii = 0; i < ActiveFigureGizmo.Bottom; ++i, ++ii)
                {
                    var j = 0;
                    while (j < ActiveFigureGizmo.Width && !ActiveFigureGizmo[ii, j].HasValue) { ++j; }
                    if (j >= ActiveFigureGizmo.Width) continue;
                    if (_field[ i ][ ActiveFigureGizmo.Right - 1 - j + (int)vector.X ].HasValue) return false;
                }
            }

            _activeFigureGizmoProxy.Center = Point.Add( _activeFigureGizmoProxy.Center, vector );

            return true;
        }









        public int[] RemoveFullLines()
        {
            throw new NotImplementedException();
        }
    }
}
