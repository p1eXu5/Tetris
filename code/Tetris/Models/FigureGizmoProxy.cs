﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Tetris.Models
{
    public class FigureGizmoProxy : FigureGizmoBase, IFigureGizmoProxy
    {

        public FigureGizmoProxy( ILiveFigureGizmo image )
        {
            Image = image ?? throw new ArgumentNullException();
        }

        public override IFigure Figure => Image.Figure;

        public override Point Center
        {
            get => Image.Center;
            set => Image.Center = value;
        }

        public override Color Color => Image.Color;
        public override int Angle => Image.Angle;

        public ILiveFigureGizmo Image { get; set; }
    }
}