using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tetris.Models
{
    public class FigureGizmoEnumerator : IEnumerator< Color?[] >
    {
        private readonly FigureGizmoBase _figureGizmo;
        private int _curIndex;
        private Color?[] _curColorRow;
        public FigureGizmoEnumerator( FigureGizmoBase figureGizmo )
        {
            _figureGizmo = figureGizmo;
            _curIndex = -1;
            _curColorRow = default( Color?[] );
        }
        public bool MoveNext()
        {
            //Avoids going beyond the end of the collection.
            if (++_curIndex >= _figureGizmo.Height)
            {
                return false;
            }
            else
            {
                // Set current box to next item in collection.
                _curColorRow = new Color?[ _figureGizmo.Width ];
                for ( int i = 0; i < _figureGizmo.Width; i++ ) {
                    _curColorRow[ i ] = _figureGizmo[ _curIndex, i ];
                }
            }
            return true;
        }
        public void Reset()
        {
            _curIndex = -1;
        }


        void IDisposable.Dispose() { }



        public Color?[] Current => _curColorRow;

        object IEnumerator.Current => Current;
    }
}
