using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Tetris.Models.Contracts
{
    public interface IFigure
    {
        Color Color { get; }
        int Width { get; }
        int Height { get; }
        Color? this[int i, int j] { get; }
    }
}
