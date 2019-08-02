using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Tetris.Contracts;

namespace Tetris
{
    public class FakeTetrisEngine : ITetrisEngine
    {
        public Color?[][] GetGameField()
        {
            return null;
            const int h = 10;
            const int w = 7;
            Color Color() => Colors.Black;

            var res = new List< Color?[]>(h);

            for ( int i = 0; i < h; i++ ) {
                res.Add( new Color?[w] );
                switch ( i ) {
                    case 0:
                    case (h - 1):
                        break;
                    case (h / 3 * 2):
                        for ( int j = 1; j < w - 1; j++ ) {
                            res[ i ][ j ] = Color();
                        }
                        break;
                    default:
                        res[i][ w / 2 ] = Color();
                        break;
                }
            }

            return res.ToArray();
        }
    }
}
