using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tetris.Models;
using System.Windows.Media;

namespace Tetris.Tests.UnitTests.TestCases
{
    public class TryAddFigureTestCases
    {
        public static IEnumerable DefaultPlacing()
        {
            var i = 0;

            #region Line-Figure

            var figure0 = new FigureGizmo(FigureFlyweightFactory.LineFigure);
            var res0 = new Color?[,]
            {
                { null, figure0.Color, figure0.Color, figure0.Color, figure0.Color, null },
                { null, null, null, null, null, null },
                { null, null, null, null, null, null },
            };
            yield return new TestCaseData(res0.GetLength(1), res0.GetLength(0), figure0)
                         .Returns(res0)
                         .SetName($"#{i++}: width: {res0.GetLength(1)}, horizontal line-figure");



            var figure1 = new FigureGizmo(FigureFlyweightFactory.LineFigure);
            var res1 = new Color?[,]
            {
                { null, figure1.Color, figure1.Color, figure1.Color, figure1.Color, null, null },
                { null, null, null, null, null, null, null },
                { null, null, null, null, null, null, null },
            };
            yield return new TestCaseData(res1.GetLength(1), res1.GetLength(0), figure1)
                         .Returns(res1)
                         .SetName($"#{i++}: width: {res1.GetLength(1)}, horizontal line-figure");



            var figure2 = new FigureGizmo(FigureFlyweightFactory.LineFigure);
            figure2.ClockwiseRotate();

            var res2 = new Color?[,]
            {
                { null, figure2.Color, null },
                { null, figure2.Color, null },
                { null, figure2.Color, null },
                { null, figure2.Color, null },
                { null, null, null },
                { null, null, null },

            };
            yield return new TestCaseData(res2.GetLength(1), res2.GetLength(0), figure2)
                         .Returns(res2)
                         .SetName($"#{i++}: width: {res2.GetLength(1)}, vertical line-figure");


            var figure3 = new FigureGizmo(FigureFlyweightFactory.LineFigure);
            figure3.ClockwiseRotate();

            var res3 = new Color?[,]
            {
                { null, figure3.Color, null, null },
                { null, figure3.Color, null, null },
                { null, figure3.Color, null, null },
                { null, figure3.Color, null, null },
                { null, null, null, null },
                { null, null, null, null },

            };
            yield return new TestCaseData(res3.GetLength(1), res3.GetLength(0), figure3)
                         .Returns(res3)
                         .SetName($"#{i++}: width: {res3.GetLength(1)}, vertical line-figure");

            #endregion


            #region L-Figure

            var figure4 = new FigureGizmo( FigureFlyweightFactory.LFigure );

            var res4 = new Color?[,]
            {
                { null, figure4.Color, figure4.Color, figure4.Color, null },
                { null, figure4.Color, null, null, null },
                { null, null, null, null, null },
                { null, null, null, null, null },
            };
            yield return new TestCaseData(res4.GetLength(1), res4.GetLength(0), figure4)
                         .Returns(res4)
                         .SetName($"#{i++}: width: {res4.GetLength(1)}, horizontal L-figure");


            var figure5 = new FigureGizmo(FigureFlyweightFactory.LFigure);
            
            var res5 = new Color?[,]
            {
                { null, figure5.Color, figure5.Color, figure5.Color, null, null },
                { null, figure5.Color, null, null, null, null },
                { null, null, null, null, null, null },
                { null, null, null, null, null, null },
            };
            yield return new TestCaseData(res5.GetLength(1), res5.GetLength(0), figure5)
                         .Returns(res5)
                         .SetName($"#{i++}: width: {res5.GetLength(1)}, horizontal L-figure");


            var figure6 = new FigureGizmo(FigureFlyweightFactory.LFigure);
            figure6.CounterclockwiseRotate();

            var res6 = new Color?[,]
            {
                { null, figure6.Color, null, null },
                { null, figure6.Color, null, null },
                { null, figure6.Color, figure6.Color, null },
                { null, null, null, null },
                { null, null, null, null },
            };
            yield return new TestCaseData(res6.GetLength(1), res6.GetLength(0), figure6)
                         .Returns(res6)
                         .SetName($"#{i++}: width: {res6.GetLength(1)}, horizontal L-figure");


            var figure7 = new FigureGizmo(FigureFlyweightFactory.LFigure);
            figure7.CounterclockwiseRotate();

            var res7 = new Color?[,]
            {
                { null, figure7.Color, null, null, null },
                { null, figure7.Color, null, null, null },
                { null, figure7.Color, figure7.Color, null, null },
                { null, null, null, null, null },
                { null, null, null, null, null },
            };
            yield return new TestCaseData(res7.GetLength(1), res7.GetLength(0), figure7)
                         .Returns(res7)
                         .SetName($"#{i}: width: {res7.GetLength(1)}, horizontal L-figure");

            #endregion
        }
    }
}
