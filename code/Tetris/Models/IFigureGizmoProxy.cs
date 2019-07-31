﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Models
{
    public interface IFigureGizmoProxy: IFigureGizmo, ILiveFigureGizmo
    {
        ILiveFigureGizmo Image { get; set; }
    }
}
