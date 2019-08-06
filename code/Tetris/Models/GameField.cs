using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Tetris.Helpers;
using Tetris.Models.Contracts;

namespace Tetris.Models
{
    public class GameField : IGameField
    {
        #region Fields

        private readonly List<Color?[]> _field;
        private readonly IFigureGizmoProxy _activeFigureGizmoProxy;
        private int _fieldTop;
        private readonly IVectorSpinner _vectorSpinner;

        #endregion


        #region Ctor

        public GameField( IVectorSpinner vectorSpinner, int width = 10, int height = 20)
        {
            _vectorSpinner = vectorSpinner ?? throw new ArgumentNullException(nameof(vectorSpinner), @"IVectorSpinner cannot be null."); ;
            this.Width = width > 0 ? width : throw new ArgumentException("width must be greater than zero");
            this.Height = height > 0 ? height : throw new ArgumentException("height must be greater than zero");

            _field = new List<Color?[]>(height);
            height.ForEach(() => _field.Add(new Color?[width]));
            _fieldTop = _field.Count;

            _activeFigureGizmoProxy = new FigureGizmoProxy(FigureGizmo.EmptyGizmo);
        }

        #endregion


        #region Properties

        public int Width { get; }
        public int Height { get; }
        public IFigureGizmo ActiveFigureGizmo => _activeFigureGizmoProxy.Image;

        #endregion


        #region Public Methods

        public void Clear()
        {
            for (var i = 0; i < Height; ++i)
            {
                _field[i] = new Color?[Width];
            }

            _fieldTop = _field.Count;
            ResetActiveFigureGizmo();
        }

        public bool TryAddFigure(ILiveFigureGizmo figureGizmo)
        {
            if (figureGizmo.IsEmptyGizmo) throw new ArgumentException("figureGizmo has empty figure");
            if (figureGizmo.Width > this.Width) throw new ArgumentException("figureGizmo width must be greater than gamefield width");
            if (figureGizmo.Height > this.Height) throw new ArgumentException("figureGizmo height must be greater than gamefield height");

            var center = new Point(Width / 2.0, figureGizmo.Height / 2.0);
            figureGizmo.MoveTo(center);

            if (IsOverlay(figureGizmo)) return false;

            _activeFigureGizmoProxy.Image = figureGizmo;

            return true;
        }

        public void Merge()
        {
            if (ActiveFigureGizmo.IsEmptyGizmo)
            {
                throw new InvalidOperationException("Figure is Empty");
            }

            for (int i = ActiveFigureGizmo.Top, ii = 0; i < ActiveFigureGizmo.Bottom; i++, ii++)
            {
                for (int j = ActiveFigureGizmo.Left, jj = 0; j < ActiveFigureGizmo.Right; j++, jj++)
                {
                    if ( ActiveFigureGizmo[ ii, jj ].HasValue ) {
                        _field[ i ][ j ] = ActiveFigureGizmo[ ii, jj ];
                    }
                }
            }

            if ( _fieldTop > ActiveFigureGizmo.Top ) _fieldTop = ActiveFigureGizmo.Top;

            ResetActiveFigureGizmo();
        }

        public bool TryRotateFigure(RotateDirections direction)
        {
            if (ActiveFigureGizmo.IsEmptyGizmo) {
                return false;
            }

            _activeFigureGizmoProxy.Rotate(direction);
            
            if ( !IsOverlay( _activeFigureGizmoProxy ) )
            {
                _activeFigureGizmoProxy.Image.Rotate(direction);
                return true;
            }

            foreach ( var vector in _vectorSpinner.GetVectors( _activeFigureGizmoProxy.Width, _activeFigureGizmoProxy.Height ) ) 
            {
                _activeFigureGizmoProxy.Move( vector );

                if ( !IsOverlay( _activeFigureGizmoProxy ) ) {
                    _activeFigureGizmoProxy.Image.Move( vector );
                    _activeFigureGizmoProxy.Image.Rotate( direction );
                    return true;
                }
            }

            return false;
        }

        public bool TryMove(Vector vector)
        {
            if (!vector.LengthSquared.Equals(1.0)) {
                throw new ArgumentException("Vector is wrong");
            }

            if (ActiveFigureGizmo.IsEmptyGizmo) {
                return false;
            }

            if (vector.Y > 0)
            {
                if (ActiveFigureGizmo.Bottom + vector.Y > Height) return false;

                for (int j = ActiveFigureGizmo.Left, jj = 0; j < ActiveFigureGizmo.Right; ++j, ++jj)
                {
                    var i = ActiveFigureGizmo.Height - 1;
                    while (i >= 0 && !ActiveFigureGizmo[i, jj].HasValue) { --i; }
                    if (i < 0) continue;
                    if (_field[ActiveFigureGizmo.Top + i + (int)vector.Y][j].HasValue) return false;
                }
            }
            else if (vector.Y < 0)
            {
                if (ActiveFigureGizmo.Top + vector.Y < 0) return false;

                for (int j = ActiveFigureGizmo.Left, jj = 0; j < ActiveFigureGizmo.Right; ++j, ++jj)
                {
                    var i = 0;
                    while (i < ActiveFigureGizmo.Height && !ActiveFigureGizmo[i, jj].HasValue) { ++i; }
                    if (i >= ActiveFigureGizmo.Height) continue;
                    if (_field[ActiveFigureGizmo.Bottom - 1 - i + (int)vector.Y][j].HasValue) return false;
                }
            }
            else if (vector.X > 0)
            {
                if (ActiveFigureGizmo.Right + vector.X > Width) return false;

                for (int i = ActiveFigureGizmo.Top, ii = 0; i < ActiveFigureGizmo.Bottom; ++i, ++ii)
                {
                    var j = ActiveFigureGizmo.Width - 1;
                    while (j >= 0 && !ActiveFigureGizmo[ii, j].HasValue) { --j; }
                    if (j < 0) continue;
                    if (_field[i][ActiveFigureGizmo.Left + j + (int)vector.X].HasValue) return false;
                }
            }
            else if (vector.X < 0)
            {
                if (ActiveFigureGizmo.Left + vector.X < 0) return false;

                for (int i = ActiveFigureGizmo.Top, ii = 0; i < ActiveFigureGizmo.Bottom; ++i, ++ii)
                {
                    var j = 0;
                    while (j < ActiveFigureGizmo.Width && !ActiveFigureGizmo[ii, j].HasValue) { ++j; }
                    if (j >= ActiveFigureGizmo.Width) continue;
                    if (_field[i][ActiveFigureGizmo.Right - 1 - j + (int)vector.X].HasValue) return false;
                }
            }

            ((ILiveFigureGizmo)ActiveFigureGizmo).Move( vector );
            _activeFigureGizmoProxy.MoveTo( _activeFigureGizmoProxy.Center );

            return true;
        }

        private void MoveFigure( ILiveFigureGizmo figure, Vector vector )
        {
            figure.Move( vector );
        }

        public int[] RemoveFilledLines()
        {
            var res = new List<int>(Height);

            for (int i = _fieldTop; i < Height; i++) {
                if (_field[i].All(col => col.HasValue)) {
                    res.Add(i);
                }
            }

            if ( !res.Any() ) return new int[0];

            int offset = 0;

            for ( int i = Height - 1; i >= offset; --i ) {
                while ( res.Contains( i - offset ) && ( i - offset) >= 0 ) {
                    ++offset;
                }
                if ( i - offset < 0) break;
                _field[ i ] = _field[ i - offset ];
            }

            for (var i = 0; i < offset; ++i) {
                _field[i] = new Color?[Width];
            }

            return res.ToArray();
        }

        public (Color?[][] data, int left, int top) GetFigureStack()
        {
            if ( _fieldTop == _field.Count ) {
                return ((new Color?[0][], 0, 0));
            }
            return (_field.Skip( _fieldTop ).ToArray(), 0, _fieldTop);
        }

        public (Color?[][] data, int left, int top) GetActiveFigure()
        {
            return (ActiveFigureGizmo.ToArray(), ActiveFigureGizmo.Left, ActiveFigureGizmo.Top);
        }

        public Color?[][] GetField()
        {
            var fieldCopy = _field.ToArray();

            if (!ActiveFigureGizmo.IsEmptyGizmo)
            {
                for (int i = ActiveFigureGizmo.Top, ii = 0; i < ActiveFigureGizmo.Bottom; i++, ii++)
                {
                    for (int j = ActiveFigureGizmo.Left, jj = 0; j < ActiveFigureGizmo.Right; j++, jj++)
                    {
                        fieldCopy[i][j] = ActiveFigureGizmo[ii, jj];
                    }
                }
            }

            return fieldCopy;
        }

        public virtual void CreateBorders()
        {

        }

        #endregion


        private void ResetActiveFigureGizmo()
        {
            _activeFigureGizmoProxy.Image = FigureGizmo.EmptyGizmo;
        }

        private bool IsOverlay(IFigureGizmo figureGizmo)
        {
            if ( figureGizmo.Top < 0 || figureGizmo.Bottom > Height || figureGizmo.Left < 0 || figureGizmo.Right > Width ) return true;

            for (int i = figureGizmo.Top, ii = 0; i < figureGizmo.Bottom; i++, ii++)
            {
                for (int j = figureGizmo.Left, jj = 0; j < figureGizmo.Right; j++, jj++)
                {
                    if (figureGizmo[ii, jj].HasValue && _field[i][j].HasValue) return true;
                }
            }
            return false;
        }
    }
}
