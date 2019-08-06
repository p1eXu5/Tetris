using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tetris.Models
{
    public static class Directions
    {
        public static Vector DownVector => new Vector( 0.0, 1.0 );
        public static Vector LeftVector => new Vector( -1.0, 0.0 );
        public static Vector RightVector => new Vector( 1.0, 0.0 );
        public static Vector UpVector => new Vector( 0.0, -1.0 );
    }
}
