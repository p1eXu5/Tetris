using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Tetris.Models.Contracts
{
    public interface IFigureGizmo : IEnumerable< Color?[] >
    {
        IFigure Figure { get; }
        Point Center { get; }

        int Angle { get; }
        Color Color { get; }
        int Width { get; }
        int Height { get; }
        int Top { get; }
        int Bottom { get; }
        int Left { get; }
        int Right { get; }
        Color? this[ int i, int j ] { get; }
        
        void MoveTo( Point point );
        bool IsEmptyGizmo { get; }
    }
}