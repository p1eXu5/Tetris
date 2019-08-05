using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Tetris.Models.Contracts;

namespace Tetris.Models
{
    public class FigureGizmoProxy : FigureGizmoBase, IFigureGizmoProxy
    {
        public FigureGizmoProxy( ILiveFigureGizmo image )
        {
            Image = image ?? throw new ArgumentNullException();
        }

        public override IFigure Figure => Image.Figure;

        public override Color Color => Image.Color;
        public override int Angle => Image.Angle;
        public override Point Center => Image.Center;

        public ILiveFigureGizmo Image { get; set; }
    }
}
