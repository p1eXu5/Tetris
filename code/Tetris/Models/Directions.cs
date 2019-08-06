using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tetris.Models
{
    [ Flags ]
    public enum Directions : byte
    {
        Up      = 0b_0000_0001,
        Right   = 0b_0000_0010,
        Down    = 0b_0000_0100,
        Left    = 0b_0000_1000,
    }

    internal static class DirectionVectors
    {
        public static Vector DownVector => new Vector( 0.0, 1.0 );
        public static Vector LeftVector => new Vector( -1.0, 0.0 );
        public static Vector RightVector => new Vector( 1.0, 0.0 );
        public static Vector UpVector => new Vector( 0.0, -1.0 );

        public static Vector FromDirection( Directions direction )
        {
            var res = new Vector();

            if ( (direction & Directions.Up) > 0 ) 
            {
                res += UpVector;
            }

            if ((direction & Directions.Right) > 0)
            {
                res += RightVector;
            }

            if ((direction & Directions.Down) > 0)
            {
                res += DownVector;
            }

            if ((direction & Directions.Left) > 0)
            {
                res += LeftVector;
            }

            return res;
        }
    }
}
