using System;

namespace Tetris.Models.Contracts
{
    public interface IFigureFlyweightFactory : IDisposable
    {
        ILiveFigureGizmo GetNext();
    }
}