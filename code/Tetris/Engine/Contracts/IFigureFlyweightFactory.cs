using System;
using Tetris.Models.Contracts;

namespace Tetris.Engine.Contracts
{
    public interface IFigureFlyweightFactory : IDisposable
    {
        ILiveFigureGizmo GetNext();
    }
}