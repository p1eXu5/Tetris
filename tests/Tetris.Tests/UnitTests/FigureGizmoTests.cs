using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NUnit.Framework;
using Tetris.Models;

namespace Tetris.Tests.UnitTests
{
    [TestFixture]
    public class FigureGizmoTests
    {
        #region Width

        [ Test ]
        public void Width_AngleIsZero_ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();

            var width = figureGizmo.Width;

            Assert.That( width, Is.EqualTo( figureGizmo.Figure.Width ) );
        }

        [Test]
        public void Width__AngleIs90_ClockwiseRotate__ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();

            figureGizmo.ClockwiseRotate();
            var width = figureGizmo.Width;

            Assert.That(width, Is.EqualTo(figureGizmo.Figure.Height));
        }

        [Test]
        public void Width__AngleIs180_ClockwiseRotate__ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.ClockwiseRotate();
            figureGizmo.ClockwiseRotate();

            var width = figureGizmo.Width;

            Assert.That(width, Is.EqualTo(figureGizmo.Figure.Width));
        }

        [Test]
        public void Width__AngleIs270_ClockwiseRotate__ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.ClockwiseRotate();
            figureGizmo.ClockwiseRotate();
            figureGizmo.ClockwiseRotate();

            var width = figureGizmo.Width;

            Assert.That(width, Is.EqualTo(figureGizmo.Figure.Height));
        }

        [Test]
        public void Width__AngleIs90_CounterclockwiseRotate__ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.CounterclockwiseRotate();
            figureGizmo.CounterclockwiseRotate();
            figureGizmo.CounterclockwiseRotate();

            var width = figureGizmo.Width;

            Assert.That(width, Is.EqualTo(figureGizmo.Figure.Height));
        }

        [Test]
        public void Width__AngleIs180_CounterclockwiseRotate__ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.CounterclockwiseRotate();
            figureGizmo.CounterclockwiseRotate();

            var width = figureGizmo.Width;

            Assert.That(width, Is.EqualTo(figureGizmo.Figure.Width));
        }

        [Test]
        public void Width__AngleIs270_CounterclockwiseRotate__ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.CounterclockwiseRotate();

            var width = figureGizmo.Width;

            Assert.That(width, Is.EqualTo(figureGizmo.Figure.Height));
        }

        #endregion

        #region Height

        [Test]
        public void Height_AngleIsZero_ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();

            var height = figureGizmo.Height;

            Assert.That(height, Is.EqualTo(figureGizmo.Figure.Height));
        }

        [Test]
        public void Height__AngleIs90_ClockwiseRotate__ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.ClockwiseRotate();

            var height = figureGizmo.Height;

            Assert.That(height, Is.EqualTo(figureGizmo.Figure.Width));
        }

        [Test]
        public void Height__AngleIs180_ClockwiseRotate__ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.ClockwiseRotate();
            figureGizmo.ClockwiseRotate();

            var height = figureGizmo.Height;

            Assert.That(height, Is.EqualTo(figureGizmo.Figure.Height));
        }

        [Test]
        public void Height__AngleIs270_ClockwiseRotate__ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.ClockwiseRotate();
            figureGizmo.ClockwiseRotate();
            figureGizmo.ClockwiseRotate();

            var height = figureGizmo.Height;

            Assert.That(height, Is.EqualTo(figureGizmo.Figure.Width));
        }

        [Test]
        public void Height__AngleIs90_CounterclockwiseRotate__ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.CounterclockwiseRotate();
            figureGizmo.CounterclockwiseRotate();
            figureGizmo.CounterclockwiseRotate();

            var height = figureGizmo.Height;

            Assert.That(height, Is.EqualTo(figureGizmo.Figure.Width));
        }

        [Test]
        public void Height__AngleIs180_CounterclockwiseRotate__ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.CounterclockwiseRotate();
            figureGizmo.CounterclockwiseRotate();

            var height = figureGizmo.Height;

            Assert.That(height, Is.EqualTo(figureGizmo.Figure.Height));
        }

        [Test]
        public void Height__AngleIs270_CounterclockwiseRotate__ReturnsRowLength()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.CounterclockwiseRotate();

            var height = figureGizmo.Height;

            Assert.That(height, Is.EqualTo(figureGizmo.Figure.Width));
        }

        #endregion

        #region Indexer

        [Test]
        public void Indexer_AngleIsZero_ReturnsExpected()
        {
            var figureGizmo = GetFigureGizmo();

            Assert.That( figureGizmo[0,0], Is.EqualTo((Color?)null) );
            Assert.That( figureGizmo[0,1], Is.EqualTo(figureGizmo.Color) );
            Assert.That( figureGizmo[0,2], Is.EqualTo((Color?)null) );

            Assert.That( figureGizmo[1,0], Is.EqualTo(figureGizmo.Color) );
            Assert.That( figureGizmo[1,1], Is.EqualTo(figureGizmo.Color) );
            Assert.That( figureGizmo[1,2], Is.EqualTo(figureGizmo.Color) );
        }

        [Test]
        public void Indexer_AngleIs90_ReturnsExpected()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.CounterclockwiseRotate();

            Assert.That(figureGizmo[0, 0], Is.EqualTo((Color?)null));
            Assert.That(figureGizmo[0, 1], Is.EqualTo(figureGizmo.Color));

            Assert.That(figureGizmo[1, 0], Is.EqualTo(figureGizmo.Color));
            Assert.That(figureGizmo[1, 1], Is.EqualTo(figureGizmo.Color));

            Assert.That(figureGizmo[2, 0], Is.EqualTo((Color?)null));
            Assert.That(figureGizmo[2, 1], Is.EqualTo(figureGizmo.Color));
        }

        [Test]
        public void Indexer_AngleIs270_ReturnsExpected()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.ClockwiseRotate();

            Assert.That(figureGizmo[0, 0], Is.EqualTo(figureGizmo.Color));
            Assert.That(figureGizmo[0, 1], Is.EqualTo((Color?)null));

            Assert.That(figureGizmo[1, 0], Is.EqualTo(figureGizmo.Color));
            Assert.That(figureGizmo[1, 1], Is.EqualTo(figureGizmo.Color));

            Assert.That(figureGizmo[2, 0], Is.EqualTo(figureGizmo.Color));
            Assert.That(figureGizmo[2, 1], Is.EqualTo((Color?)null));
        }

        [Test]
        public void Indexer_AngleIs180_ReturnsExpected()
        {
            var figureGizmo = GetFigureGizmo();
            figureGizmo.ClockwiseRotate();
            figureGizmo.ClockwiseRotate();

            Assert.That(figureGizmo[0, 0], Is.EqualTo(figureGizmo.Color));
            Assert.That(figureGizmo[0, 1], Is.EqualTo(figureGizmo.Color));
            Assert.That(figureGizmo[0, 2], Is.EqualTo(figureGizmo.Color));

            Assert.That(figureGizmo[1, 0], Is.EqualTo((Color?)null));
            Assert.That(figureGizmo[1, 1], Is.EqualTo(figureGizmo.Color));
            Assert.That(figureGizmo[1, 2], Is.EqualTo((Color?)null));

        }

        #endregion

        #region IsEmptyGizmo

        [ Test ]
        public void IsEmptyGizmo_DefaultFigure_ReturnTrue()
        {
            Assert.True( FigureGizmo.EmptyGizmo.IsEmptyGizmo );
        }

        #endregion

        #region Factory

        private ILiveFigureGizmo GetFigureGizmo()
        {
            var shape = new[,] {
                { 0, 1, 0 },
                { 1, 1, 1 }
            };
            return new FigureGizmo( new Figure(shape, Colors.White) );
        }

        #endregion
    }
}
