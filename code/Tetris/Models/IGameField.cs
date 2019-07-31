using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Tetris.Models
{
    public interface IGameField
    {
        int Width { get; }
        int Height { get; }

        IFigureGizmo ActiveFigureGizmo { get; }

        void Clear();
        bool TryAddFigure( ILiveFigureGizmo figure );

        void Merge();
        bool TryMove( Vector vector );
        bool TryRotateFigure( RotateDirections direction );
        Color?[,] GetField();
        int[] RemoveFullLines();
    }
}
