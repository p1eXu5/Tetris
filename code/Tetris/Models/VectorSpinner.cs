using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tetris.Models.Contracts;

namespace Tetris.Models
{
    public class VectorSpinner : IVectorSpinner
    {
        public IEnumerable< Vector > GetVectors( int width, int height )
        {
            if ( width == height ) yield break;

            if ( width > height ) {
                foreach ( var vector in MostlyHorizontalVectors(width, height) ) {
                    yield return vector;
                }
                yield break;
            }

            foreach ( var vector in MostlyVerticalVectors(width, height) ) {
                yield return vector;
            }
        }

        private int GetMaxOffset( int width, int height ) => (int)Math.Ceiling(Math.Abs(width / 2.0 - height / 2.0));

        private IEnumerable< Vector > MostlyHorizontalVectors( int width, int height )
        {
            var maxOffset = GetMaxOffset( width, height );

            for ( int y = 0; y < 2; y++ ) 
            {
                var x = -(maxOffset - 1);
                while ( x <= maxOffset ) {
                    if ( y == 0 && x == 0 ) {
                        ++x;
                        continue;
                    }
                    yield return new Vector( x, y );
                    ++x;
                }
            }
        }

        private IEnumerable< Vector > MostlyVerticalVectors( int width, int height )
        {
            var maxOffset = GetMaxOffset(width, height);

            var y = -(maxOffset - 1);
            while (y <= maxOffset)
            {
                for ( int x = 0; x < 2; x++ ) {
                    if ( y == 0 && x == 0 ) {
                        continue;
                    }
                    yield return new Vector(x, y);
                }
                ++y;
            }
        }
    }
}
