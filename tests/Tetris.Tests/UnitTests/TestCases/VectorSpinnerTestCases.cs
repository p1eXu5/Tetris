using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NUnit.Framework;

namespace Tetris.Tests.UnitTests.TestCases
{
    public class VectorSpinnerTestCases
    {
        public static IEnumerable WidthGreaterHeightCases()
        {
            var i = 0;

            var res0 = new[] {
                new Vector( 1.0, 0.0),
                new Vector( 0.0, 1.0 ),
                new Vector( 1.0, 1.0 ), 
            };
            yield return new TestCaseData( 2, 1 ).Returns( res0 ).SetName( $"WidthGreaterHeightCase #{i++}" );

            var res1 = new[] {
                new Vector( 1.0, 0.0),
                new Vector( 0.0, 1.0 ),
                new Vector( 1.0, 1.0 ),
            };
            yield return new TestCaseData(3, 2).Returns(res1).SetName($"WidthGreaterHeightCase #{i++}");

            var res2 = new[] {
                new Vector( -1.0, 0.0),
                //new Vector( 0.0, 0.0),
                new Vector( 1.0, 0.0),
                new Vector( 2.0, 0.0 ),
                new Vector( -1.0, 1.0 ),
                new Vector( 0.0, 1.0 ),
                new Vector( 1.0, 1.0 ),
                new Vector( 2.0, 1.0 ),
            };
            yield return new TestCaseData(4, 1).Returns(res2).SetName($"WidthGreaterHeightCase #{i++}");
        }

        public static IEnumerable HeightGreaterWidthCases()
        {
            var i = 0;

            var res0 = new[] {
                new Vector( 1.0, 0.0),
                new Vector( 0.0, 1.0 ),
                new Vector( 1.0, 1.0 ),
            };
            yield return new TestCaseData(1, 2).Returns(res0).SetName($"WidthGreaterHeightCase #{i++}");

            var res1 = new[] {
                new Vector( 1.0, 0.0),
                new Vector( 0.0, 1.0 ),
                new Vector( 1.0, 1.0 ),
            };
            yield return new TestCaseData(2, 3).Returns(res1).SetName($"WidthGreaterHeightCase #{i++}");

            var res2 = new[] {
                new Vector( 0.0, -1.0),
                new Vector( 1.0, -1.0),
                //new Vector( 0.0, 0.0),
                new Vector( 1.0, 0.0 ),
                new Vector( 0.0, 1.0 ),
                new Vector( 1.0, 1.0 ),
                new Vector( 0.0, 2.0 ),
                new Vector( 1.0, 2.0 ),
            };
            yield return new TestCaseData(1, 4).Returns(res2).SetName($"WidthGreaterHeightCase #{i++}");
        }
    }
}
