using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Tetris.Models
{
    public struct Figure : IFigure
    {
        public readonly int[,] _shape;
        public Figure( int[,] form, Color color )
        {
            if ( form == null ) throw new ArgumentNullException();

            var isCorrect = false;

            for ( int i = 0; i < form.GetLength( 0 ); i++ ) {
                for ( int j = 0; j < form.GetLength( 1 ); j++ ) {
                    if ( form[ i, j ] != 0 ) {
                        isCorrect = true;
                        goto next;
                    }
                }    
            }

            next:

            if ( !isCorrect ) throw new ArgumentException();

            _shape = new int[ form.GetLength( 0 ), form.GetLength( 1 ) ];
            Array.Copy( form, _shape, form.GetLength( 0 ) * form.GetLength( 1 ) );

            Color = color;
        }

        public Color Color { get; }

        public int Width => _shape?.GetLength( 1 ) ?? 0;

        public int Height => _shape?.GetLength( 0 ) ?? 0;

        public Color? this[ int i, int j ] => _shape[ i, j ] != 0 ? Color : ( Color? )null;
        
    }
}
