using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using Moq;
using NUnit.Framework;
using Tetris.Models;
using Tetris.Tests.UnitTests.TestCases;

// ReSharper disable ObjectCreationAsStatement

namespace Tetris.Tests.UnitTests
{
    [TestFixture]
    public class GameFieldTests
    {
        #region Ctor
        [Test]
        public void Ctor_ZeroWidth_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>( new TestDelegate( () => new GameField( 0, 1 ) ) );
        }

        [Test]
        public void Ctor_NegativeWidth_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(new TestDelegate(() => new GameField(-1, 1)));
        }

        [Test]
        public void Ctor_ZeroHeight_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(new TestDelegate(() => new GameField(1, -1)));
        }

        [Test]
        public void Ctor_NegativeHeight_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(new TestDelegate(() => new GameField(1, -1)));
        }

        #endregion


        #region GetField

        [ Test ]
        public void GetField_ByDefault_ReturnsNullableArray()
        {
            var gamefield = GetGameField( 1, 1 );
            var expected = new Color?[,] { { null } } ;

            Assert.That( gamefield.GetField(), Is.EquivalentTo( expected ) );
        }

        #endregion


        #region TryAddFigure

        [Test]
        public void TryAddFigure_FigureWidthGreaterGameFieldWidth_ThrowsArgumentException()
        {
            var gameField = GetGameField( 1, 1 );
            var figureGizmo = GetMockedMeasureFigureGizmo( 2, 1 );

            Assert.Throws< ArgumentException >( new TestDelegate( () => gameField.TryAddFigure( figureGizmo ) ) );
        }

        [Test]
        public void TryAddFigure_FigureHeightGreaterGameFieldWidth_ThrowsArgumentException()
        {
            var gameField = GetGameField(1, 1);
            var figureGizmo = GetMockedMeasureFigureGizmo(1, 2);

            Assert.Throws<ArgumentException>(new TestDelegate(() => gameField.TryAddFigure(figureGizmo)));
        }

        [ TestCaseSource( typeof(TryAddFigureTestCases), nameof( TryAddFigureTestCases.DefaultPlacing )) ]
        public Color?[,] TryAddFigure_ByDefault_AddFigureToTheCenterTop( int width, int height, ILiveFigureGizmo figureGizmo )
        {
            var gameField = GetGameField( width, height );
            var canAdd = gameField.TryAddFigure( figureGizmo );

            Assert.IsTrue( canAdd );

            var res = gameField.GetField();

            return res;
        }

        #endregion


        #region Merge

        [ Test ]
        public void Merge_EmptyFigureGizmo_Throws()
        {
            var gameField = GetGameField( 1, 1 );
            Assert.Throws< InvalidOperationException >( () => gameField.Merge() );
        }

        [ Test ]
        public void Merge_NotEmptyFigure_MergeFigureResetsFigureToEmptyFigure()
        {
            var gameField = GetGameField( 3, 3 );
            gameField.TryAddFigure( new FigureGizmo( FigureFlyweightFactory.JFigure ) );

            gameField.Merge();

            bool hasFilledCells = false;
            var field = gameField.GetField();

            for ( int i = 0; i < field.GetLength( 0 ); i++ ) {
                for ( int j = 0; j < field.GetLength( 1 ); j++ ) {
                    if ( field[ i, j ] != null ) {
                        hasFilledCells = true;
                        goto next;
                    }
                }
            }

            next:

            Assert.IsTrue( hasFilledCells );
            Assert.Throws< InvalidOperationException >( () => gameField.Merge() );
        }

        #endregion


        #region TryMove

        [ Test ]
        public void TryMove__EmptyFigureGizmo_DownVector__Throws()
        {
            var gameField = GetGameField( 1, 1 );
            Assert.Throws< InvalidOperationException >( () => gameField.TryMove( new Vector(0, 1.0) ) );
        }

        [Test]
        public void TryMove_ZeroVector_Throws()
        {
            var gameField = GetGameField(10, 10);
            gameField.TryAddFigure( new FigureGizmo( FigureFlyweightFactory.JFigure ) );
            Assert.Throws<ArgumentException>(() => gameField.TryMove(new Vector()));
        }

        [Test]
        public void TryMove_FigureGizmoAboveTheBottom_DownVector_ReturnsFalse()
        {
            var gameField = GetGameField(10, 2);
            gameField.TryAddFigure(new FigureGizmo(FigureFlyweightFactory.TiFigure));
            Assert.False( gameField.TryMove( new Vector(0, 1.0) ) );
        }

        [ Test ]
        public void TryMove_NotEmptyFigureNotAboveTheBottom_DownVector_MovesFigureDown()
        {
            var gameField = GetGameField( 10, 10 );
            gameField.TryAddFigure( new FigureGizmo( FigureFlyweightFactory.JFigure ) );

            gameField.TryMove( new Vector( 0, 1.0 ) );
            Assert.That( gameField.ActiveFigureGizmo.Top, Is.EqualTo( 1 ) );
        }

        [ Test ]
        public void TryMove__MoveLeft_OnTheLeftSide__ReturnsFalse()
        {
            var gameField = GetGameField( 3, 3 );
            gameField.TryAddFigure( new FigureGizmo( FigureFlyweightFactory.LFigure ) );
            var left = gameField.ActiveFigureGizmo.Left;

            Assert.False( gameField.TryMove( new Vector(-1.0, 0.0 ) ) );
            Assert.That( gameField.ActiveFigureGizmo.Left, Is.EqualTo( left ) );
        }

        [Test]
        public void TryMove__MoveRight_OnTheRightSide__ReturnsFalse()
        {
            var gameField = GetGameField(3, 3);
            gameField.TryAddFigure(new FigureGizmo(FigureFlyweightFactory.LFigure));
            var right = gameField.ActiveFigureGizmo.Right;

            Assert.False(gameField.TryMove(new Vector(1.0, 0.0)));
            Assert.That(gameField.ActiveFigureGizmo.Right, Is.EqualTo(right));
        }

        [Test]
        public void TryMove__MoveTop_OnTop__ReturnsFalse()
        {
            var gameField = GetGameField(3, 3);
            gameField.TryAddFigure(new FigureGizmo(FigureFlyweightFactory.LFigure));
            var top = gameField.ActiveFigureGizmo.Top;

            Assert.False(gameField.TryMove(new Vector(0.0, -1.0)));
            Assert.That(gameField.ActiveFigureGizmo.Top, Is.EqualTo(top));
        }

        [Test]
        public void TryMove__MoveDown_UnderTheFigure__ReturnsFalse()
        {
            var gameField = GetFilledGameField();

            gameField.TryAddFigure(new FigureGizmo(FigureFlyweightFactory.LFigure));
            gameField.TryRotateFigure( RotateDirections.Clockwise );
            var bottom = gameField.ActiveFigureGizmo.Bottom;

            Assert.False(gameField.TryMove(new Vector(0.0, 1.0)));
            Assert.That(gameField.ActiveFigureGizmo.Bottom, Is.EqualTo(bottom));
        }

        #endregion


        #region Factory

        private IGameField GetGameField( int width, int height )
        {
            var gamefield = new GameField( width, height );

            return gamefield;
        }

        private ILiveFigureGizmo GetMockedMeasureFigureGizmo( int width, int height )
        {
            var mockFigure = new Mock< ILiveFigureGizmo >();
            mockFigure.Setup( f => f.Width ).Returns( width );
            mockFigure.Setup( f => f.Height ).Returns( height );

            return mockFigure.Object;
        }

        private IGameField GetFilledGameField()
        {
            var gamefield = new GameField(5, 4);
            gamefield.TryAddFigure( new FigureGizmo( FigureFlyweightFactory.TiFigure ) );
            gamefield.TryMove( new Vector( 0, 1.0 ) );
            gamefield.TryMove( new Vector( 0, 1.0 ) );

            return gamefield;
        }

        #endregion
    }
}
