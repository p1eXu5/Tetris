using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Tetris.Models.Contracts
{
    public interface IFigureGizmoProxy: IFigureGizmo, ILiveFigureGizmo
    {
        /// <summary>
        /// Proxied figure gizmo.
        /// </summary>
        ILiveFigureGizmo Image { get; set; }

        /// <summary>
        /// Image angle.
        /// </summary>
        new int Angle { get; }

        /// <summary>
        /// Image center.
        /// </summary>
        new Point Center { get; }
    }
}
