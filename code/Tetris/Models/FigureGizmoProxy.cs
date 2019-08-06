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
            _center = Image.Center;
            _angle = Image.Angle;
        }

        /// <summary>
        /// <see cref="IFigureGizmoProxy.Image"/>
        /// </summary>
        public override IFigure Figure => Image.Figure;

        /// <summary>
        /// <see cref="IFigureGizmoProxy.Angle"/>
        /// </summary>
        public override int Angle => Image.Angle;

        /// <summary>
        /// <see cref="IFigureGizmoProxy.Center"/>
        /// </summary>
        public override Point Center => Image.Center;

        public ILiveFigureGizmo Image { get; set; }
    }
}
