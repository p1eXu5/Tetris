using System.Linq;
using System.Windows;
using NUnit.Framework;
using Tetris.Models;
using Tetris.Models.Contracts;
using Tetris.Tests.UnitTests.TestCases;

namespace Tetris.Tests.UnitTests
{
    [TestFixture]
    public class VectorSpinnerTests
    {
        [ Test ]
        public void GetVectors_WidthEqualsHeight_ReturnsNoVector()
        {
            var spinner = GetVectorSpinner();
            var width = 5;
            var height = width;

            var vectors = spinner.GetVectors( width, height ).ToArray();

            Assert.That( vectors, Is.Empty );
        }

        [TestCaseSource( typeof( VectorSpinnerTestCases ), nameof( VectorSpinnerTestCases.WidthGreaterHeightCases ))]
        public Vector[] GetVectors_WidthGreaterThanHeight_ReturnsExpected( int width, int height )
        {
            var spinner = GetVectorSpinner();

            var vectors = spinner.GetVectors(width, height).ToArray();

            return vectors;
        }

        [TestCaseSource(typeof(VectorSpinnerTestCases), nameof(VectorSpinnerTestCases.WidthGreaterHeightCases))]
        public Vector[] GetVectors_HeightGreaterThanWidth_ReturnsExpected(int width, int height)
        {
            var spinner = GetVectorSpinner();

            var vectors = spinner.GetVectors(width, height).ToArray();

            return vectors;
        }

        #region Facrtory

        private IVectorSpinner GetVectorSpinner()
        {
            return new VectorSpinner();
        }

        #endregion
    }
}
