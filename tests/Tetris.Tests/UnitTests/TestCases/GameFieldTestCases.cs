using System.Collections;
using NUnit.Framework;
using Tetris.Models;
using System.Windows.Media;

namespace Tetris.Tests.UnitTests.TestCases
{
    public class GameFieldTestCases
    {
        public static IEnumerable InitialFigurePlacementCases()
        {
            var i = 0;

            #region Line-Figure

            var figure0 = new FigureGizmo(FigureFlyweightFactory.LineFigure);
            var res0 = new[] {
                new Color?[] { null, figure0.Color, figure0.Color, figure0.Color, figure0.Color, null },
                new Color?[] { null, null, null, null, null, null },
                new Color?[] { null, null, null, null, null, null },
            };
            yield return new TestCaseData(res0[0].Length, res0.Length, figure0)
                         .Returns(res0)
                         .SetName($"#{i++}: width: {res0[0].Length}, horizontal line-figure");



            var figure1 = new FigureGizmo(FigureFlyweightFactory.LineFigure);
            var res1 = new[] {
                new Color?[] { null, figure1.Color, figure1.Color, figure1.Color, figure1.Color, null, null },
                new Color?[] { null, null, null, null, null, null, null },
                new Color?[] { null, null, null, null, null, null, null },
            };
            yield return new TestCaseData(res1[0].Length, res1.Length, figure1)
                         .Returns(res1)
                         .SetName($"#{i++}: width: {res1[0].Length}, horizontal line-figure");



            var figure2 = new FigureGizmo(FigureFlyweightFactory.LineFigure);
            figure2.ClockwiseRotate();

            var res2 = new[] {
                new Color?[] { null, figure2.Color, null },
                new Color?[] { null, figure2.Color, null },
                new Color?[] { null, figure2.Color, null },
                new Color?[] { null, figure2.Color, null },
                new Color?[] { null, null, null },
                new Color?[] { null, null, null },

            };
            yield return new TestCaseData(res2[0].Length, res2.Length, figure2)
                         .Returns(res2)
                         .SetName($"#{i++}: width: {res2[0].Length}, vertical line-figure");


            var figure3 = new FigureGizmo(FigureFlyweightFactory.LineFigure);
            figure3.ClockwiseRotate();

            var res3 = new[] {
                new Color?[] { null, figure3.Color, null, null },
                new Color?[] { null, figure3.Color, null, null },
                new Color?[] { null, figure3.Color, null, null },
                new Color?[] { null, figure3.Color, null, null },
                new Color?[] { null, null, null, null },
                new Color?[] { null, null, null, null },

            };
            yield return new TestCaseData(res3[0].Length, res3.Length, figure3)
                         .Returns(res3)
                         .SetName($"#{i++}: width: {res3[0].Length}, vertical line-figure");

            #endregion


            #region L-Figure

            var figure4 = new FigureGizmo( FigureFlyweightFactory.LFigure );

            var res4 = new[] {
                new Color?[] { null, figure4.Color, figure4.Color, figure4.Color, null },
                new Color?[] { null, figure4.Color, null, null, null },
                new Color?[] { null, null, null, null, null },
                new Color?[] { null, null, null, null, null },
            };
            yield return new TestCaseData(res4[0].Length, res4.Length, figure4)
                         .Returns(res4)
                         .SetName($"#{i++}: width: {res4[0].Length}, horizontal L-figure");


            var figure5 = new FigureGizmo(FigureFlyweightFactory.LFigure);
            
            var res5 = new[] {
                new Color?[] { null, figure5.Color, figure5.Color, figure5.Color, null, null },
                new Color?[] { null, figure5.Color, null, null, null, null },
                new Color?[] { null, null, null, null, null, null },
                new Color?[] { null, null, null, null, null, null },
            };
            yield return new TestCaseData(res5[0].Length, res5.Length, figure5)
                         .Returns(res5)
                         .SetName($"#{i++}: width: {res5[0].Length}, horizontal L-figure");


            var figure6 = new FigureGizmo(FigureFlyweightFactory.LFigure);
            figure6.CounterclockwiseRotate();

            var res6 = new[] {
                new Color?[] { null, figure6.Color, null, null },
                new Color?[] { null, figure6.Color, null, null },
                new Color?[] { null, figure6.Color, figure6.Color, null },
                new Color?[] { null, null, null, null },
                new Color?[] { null, null, null, null },
            };
            yield return new TestCaseData(res6[0].Length, res6.Length, figure6)
                         .Returns(res6)
                         .SetName($"#{i++}: width: {res6[0].Length}, horizontal L-figure");


            var figure7 = new FigureGizmo(FigureFlyweightFactory.LFigure);
            figure7.CounterclockwiseRotate();

            var res7 = new[] {
                new Color?[] { null, figure7.Color, null, null, null },
                new Color?[] { null, figure7.Color, null, null, null },
                new Color?[] { null, figure7.Color, figure7.Color, null, null },
                new Color?[] { null, null, null, null, null },
                new Color?[] { null, null, null, null, null },
            };
            yield return new TestCaseData(res7[0].Length, res7.Length, figure7)
                         .Returns(res7)
                         .SetName($"#{i}: width: {res7[0].Length}, horizontal L-figure");

            #endregion
        }
    }
}
