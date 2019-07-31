using System;

namespace Tetris.Models {
    public interface IFigureFlyweightFactory : IDisposable
    {
        ILiveFigureGizmo GetNext();
    }
}