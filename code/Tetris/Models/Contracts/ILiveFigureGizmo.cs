using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Tetris.Models.Contracts
{
    public interface ILiveFigureGizmo : IFigureGizmo
    {
        void Move( Vector vector );
        void Rotate( RotateDirections direction );
        void CounterclockwiseRotate();
        void ClockwiseRotate();

        int GetWidth( int angle );
        int GetHeight( int angle );
    }
}
