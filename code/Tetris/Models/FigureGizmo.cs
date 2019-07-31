using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Tetris.Models
{
    public class FigureGizmo : FigureGizmoBase
    {
        private static FigureGizmo _emptyGizmo;

        public FigureGizmo( IFigure figure )
        {
            Figure = figure;
            Center = new Point( figure.Width / 2.0, figure.Height / 2.0 );
            _angle = 0;
        }

        public static FigureGizmo EmptyGizmo => _emptyGizmo ?? (_emptyGizmo = new FigureGizmo( new Figure() ));

        public override IFigure Figure { get; }
        public override Point Center { get; set; }
        public override int Angle => _angle;
        public override Color Color => Figure.Color;
    }
}
