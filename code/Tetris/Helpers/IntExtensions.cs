using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Helpers
{
    public static class IntExtensions
    {
        public static void ForEach( this int n, Action action )
        {
            for ( int i = 0; i < n; i++ ) {
                action();
            }
        }
    }
}
