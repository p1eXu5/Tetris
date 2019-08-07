using System;
using System.Windows;

namespace Tetris.Models
{
    [ Flags ]
    public enum MoveDirections : byte
    {
        Up      = 0b_0000_0001,
        Right   = 0b_0000_0010,
        Down    = 0b_0000_0100,
        Left    = 0b_0000_1000,
    }

    public static class DirectionVectors
    {
        public static Vector DownVector => new Vector( 0.0, 1.0 );
        public static Vector LeftVector => new Vector( -1.0, 0.0 );
        public static Vector RightVector => new Vector( 1.0, 0.0 );
        public static Vector UpVector => new Vector( 0.0, -1.0 );

        public static Vector FromDirection( MoveDirections moveDirection )
        {
            var res = new Vector();

            if ( (moveDirection & MoveDirections.Up) > 0 ) 
            {
                res += UpVector;
            }

            if ((moveDirection & MoveDirections.Right) > 0)
            {
                res += RightVector;
            }

            if ((moveDirection & MoveDirections.Down) > 0)
            {
                res += DownVector;
            }

            if ((moveDirection & MoveDirections.Left) > 0)
            {
                res += LeftVector;
            }

            return res;
        }
    }
}
