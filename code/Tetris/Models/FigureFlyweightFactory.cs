using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Tetris.Models.Contracts;

namespace Tetris.Models
{
    public class FigureFlyweightFactory : IDisposable, IFigureFlyweightFactory
    {
        private readonly RandomNumberGenerator _random = RandomNumberGenerator.Create();
        private static IFigure _squareFigure;
        private static IFigure _lineFigure;
        private static IFigure _tiFigure;
        private static IFigure _lFigure;
        private static IFigure _jFigure;
        private static IFigure _sFigure;
        private static IFigure _srFigure;

        /// <summary>
        /// figure shape: ██
        /// </summary>
        public static IFigure SquareFigure =>
            _squareFigure ?? (_squareFigure = new Figure( form: new int[,] {
                                                             { 1, 1 },
                                                             { 1, 1 }
                                                         },
                                                         color: Colors.Green ));

        /// <summary>
        /// figure shape: ▄▄▄▄
        /// </summary>
        public static IFigure LineFigure =>
            _lineFigure ?? (_lineFigure = new Figure( form: new int[,] {
                                                              { 1, 1, 1, 1 }
                                                          },
                                                        color: Colors.Blue ));

        /// <summary>
        /// figure shape: ▄█▄
        /// </summary>
        public static IFigure TiFigure =>
            _tiFigure ?? (_tiFigure = new Figure( form: new int[,] {
                                                              { 0, 1, 0 },
                                                              { 1, 1, 1 },
                                                          },
                                                        color: Colors.Yellow ));

        /// <summary>
        /// figure shape: █▀▀
        /// </summary>
        public static IFigure LFigure =>
            _lFigure ?? (_lFigure = new Figure( form: new int[,] {
                                                              { 1, 1, 1 },
                                                              { 1, 0, 0 },
                                                        },
                                                        color: Colors.Tomato ));

        /// <summary>
        /// figure shape: █▄▄
        /// </summary>
        public static IFigure JFigure =>
            _jFigure ?? (_jFigure = new Figure( form: new int[,] {
                                                              { 1, 0, 0 },
                                                              { 1, 1, 1 },
                                                        },
                                                        color: Colors.DarkViolet ));

        /// <summary>
        /// figure shape: ▄█▀
        /// </summary>
        public static IFigure SFigure =>
            _sFigure ?? (_sFigure = new Figure( form: new int[,] {
                                                              { 0, 1, 1 },
                                                              { 1, 1, 0 },
                                                        },
                                                        color: Colors.MediumAquamarine ));

        /// <summary>
        /// figure shape: ▀█▄
        /// </summary>
        public static IFigure SrFigure =>
            _srFigure ?? (_srFigure = new Figure(form: new int[,] {
                                                              { 1, 1, 0 },
                                                              { 0, 1, 1 },
                                                        },
                                                        color: Colors.DeepPink ));

        public ILiveFigureGizmo GetNext()
        {
            byte[] next = new byte[1];
            _random.GetBytes( next );

            ILiveFigureGizmo figureGizmo;

            switch ( (next[0] & 0b111) % 7 ) {
                case 0:
                    figureGizmo = new FigureGizmo(SquareFigure);
                    break;
                case 1:
                    figureGizmo = new FigureGizmo(LineFigure);
                    break;
                case 2:
                    figureGizmo = new FigureGizmo(TiFigure);
                    break;
                case 3:
                    figureGizmo = new FigureGizmo(LFigure);
                    break;
                case 4:
                    figureGizmo = new FigureGizmo(JFigure);
                    break;
                case 5:
                    figureGizmo = new FigureGizmo(SFigure);
                    break;
                case 6:
                    figureGizmo = new FigureGizmo(SrFigure);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            for ( var i = 0; i < (next[0] >> 2 & 0b11); ++i ) {
                    figureGizmo.ClockwiseRotate();
            }

            return figureGizmo;
        }

        public void Dispose()
        {
            _random?.Dispose();
        }
    }
}
