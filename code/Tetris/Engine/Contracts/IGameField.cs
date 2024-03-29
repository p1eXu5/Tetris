﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Tetris.Models;
using Tetris.Models.Contracts;

namespace Tetris.Engine.Contracts
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
        Color?[][] GetField();
        int[] RemoveFilledLines();

        (Color?[][] data, int left, int top) GetFigureStack();
        (Color?[][] data, int left, int top) GetActiveFigure();

    }
}
