﻿using System;
using System.Windows.Media;
using System.Windows;
using Moq;
using NUnit.Framework;
using Tetris.Engine;
using Tetris.Engine.Contracts;
using Tetris.Models;
using Tetris.Models.Contracts;
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
            Assert.Throws<ArgumentException>( () => new GameField( new VectorSpinner(),0, 1 ) );
        }

        [Test]
        public void Ctor_NegativeWidth_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new GameField( new VectorSpinner(), -1, 1));
        }

        [Test]
        public void Ctor_ZeroHeight_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new GameField( new VectorSpinner(), 1, -1 ) );
        }

        [Test]
        public void Ctor_NegativeHeight_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new GameField(new VectorSpinner(), 1, -1));
        }

        #endregion


        #region GetField

        [ Test ]
        public void GetField_ByDefault_ReturnsNullableArray()
        {
            var gamefield = GetGameField( 1, 1 );
            var expected = new[] { new Color?[] { null } } ;

            Assert.That( gamefield.GetField(), Is.EquivalentTo( expected ) );
        }

        #endregion


        #region TryAddFigure

        [Test]
        public void TryAddFigure_FigureWidthGreaterGameFieldWidth_ThrowsArgumentException()
        {
            var gameField = GetGameField( 1, 1 );
            var figureGizmo = GetMockedMeasureFigureGizmo( 2, 1 );

            Assert.Throws< ArgumentException >( () => gameField.TryAddFigure( figureGizmo ) );
        }

        [Test]
        public void TryAddFigure_FigureHeightGreaterGameFieldWidth_ThrowsArgumentException()
        {
            var gameField = GetGameField(1, 1);
            var figureGizmo = GetMockedMeasureFigureGizmo(1, 2);

            Assert.Throws<ArgumentException>(() => gameField.TryAddFigure(figureGizmo));
        }

        [ TestCaseSource( typeof(GameFieldTestCases), nameof( GameFieldTestCases.InitialFigurePlacementCases )) ]
        public Color?[][] TryAddFigure_ByDefault_AddFigureToTheCenterTop( int width, int height, ILiveFigureGizmo figureGizmo )
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
        public void Merge_NotEmptyFigure_MergeFigureAndResetsFigureToEmptyFigure()
        {
            var gameField = GetGameField( 3, 3 );
            gameField.TryAddFigure( new FigureGizmo( FigureFlyweightFactory.JFigure ) );

            gameField.Merge();

            bool hasFilledCells = false;
            var field = gameField.GetField();

            foreach ( var t in field ) {
                foreach ( var t1 in t ) {
                    if ( t1 != null ) {
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
        public void TryMove__EmptyFigureGizmo_DownVector__ReturnsFalse()
        {
            var gameField = GetGameField( 1, 1 );
            Assert.False( gameField.TryMove( new Vector(0, 1.0) ) );
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

        [ Test ]
        public void TryMove__CannotMoveFigureLeft_BecauseAnotherFigure__ReturnsFalse()
        {
            var gameField = GetGameField( 6, 4 );
            gameField.TryAddFigure( new FigureGizmo( FigureFlyweightFactory.SquareFigure ) );
            Assert.True(gameField.TryMove( DirectionVectors.DownVector ));
            Assert.True(gameField.TryMove( DirectionVectors.DownVector ));
            Assert.True(gameField.TryMove( DirectionVectors.LeftVector ));
            Assert.True(gameField.TryMove( DirectionVectors.LeftVector ));
            gameField.Merge();

            gameField.TryAddFigure(new FigureGizmo(FigureFlyweightFactory.SquareFigure));
            Assert.True(gameField.TryMove(DirectionVectors.DownVector));

            Assert.False( gameField.TryMove( DirectionVectors.LeftVector ) );
        }

        [Test]
        public void TryMove__CannotMoveFigureLeft_BecauseLeftSide__ReturnsFalse()
        {
            var gameField = GetGameField(6, 4);
            gameField.TryAddFigure(new FigureGizmo(FigureFlyweightFactory.SquareFigure));

            Assert.True(gameField.TryMove(DirectionVectors.LeftVector));
            Assert.True(gameField.TryMove(DirectionVectors.LeftVector));

            Assert.False(gameField.TryMove(DirectionVectors.LeftVector));
        }

        [Test]
        public void TryMove__CannotMoveFigureRight_BecauseAnotherFigure__ReturnsFalse()
        {
            var gameField = GetGameField(6, 4);
            gameField.TryAddFigure(new FigureGizmo(FigureFlyweightFactory.SquareFigure));
            Assert.True(gameField.TryMove(DirectionVectors.DownVector));
            Assert.True(gameField.TryMove(DirectionVectors.DownVector));
            Assert.True(gameField.TryMove(DirectionVectors.RightVector));
            Assert.True(gameField.TryMove(DirectionVectors.RightVector));
            gameField.Merge();

            gameField.TryAddFigure(new FigureGizmo(FigureFlyweightFactory.SquareFigure));
            Assert.True(gameField.TryMove(DirectionVectors.DownVector));

            Assert.False(gameField.TryMove(DirectionVectors.RightVector));
        }

        [Test]
        public void TryMove__CannotMoveFigureRight_BecauseRightSide__ReturnsFalse()
        {
            var gameField = GetGameField(6, 4);
            gameField.TryAddFigure(new FigureGizmo(FigureFlyweightFactory.SquareFigure));

            Assert.True(gameField.TryMove(DirectionVectors.RightVector));
            Assert.True(gameField.TryMove(DirectionVectors.RightVector));

            Assert.False(gameField.TryMove(DirectionVectors.RightVector));
        }

        #endregion


        #region RemoveFilledLines

        [ Test ]
        public void RemoveFilledLines_ByDefault_ReturnsEmptyArray()
        {
            var gameField = GetGameField( 1, 1 );
            var expected = new int[0];

            Assert.That( gameField.RemoveFilledLines(), Is.EqualTo( expected ) );
        }

        [ Test ]
        public void RemoveFilledLines_FieldHasFilledLines_ReturnsFilledLinesNumbers()
        {
            var gamefield = GetGameField( 3, 4 );

            gamefield.TryAddFigure( new FigureGizmo( FigureFlyweightFactory.TiFigure ) );
            gamefield.TryMove( new Vector( 0.0, 1.0 ) );
            gamefield.TryMove( new Vector( 0.0, 1.0 ) );
            gamefield.Merge();

            var figure = new FigureGizmo( FigureFlyweightFactory.TiFigure );
            figure.ClockwiseRotate();
            figure.ClockwiseRotate();
            gamefield.TryAddFigure( figure );
            gamefield.Merge();

            var expected = new[] { 0, 3 };

            Assert.That( gamefield.RemoveFilledLines(), Is.EquivalentTo( expected ) );
        }

        [Test]
        public void RemoveFilledLines_FieldHasNoFilledLines_ReturnsEmptyArray()
        {
            var gamefield = GetGameField(4, 4);

            gamefield.TryAddFigure(new FigureGizmo(FigureFlyweightFactory.TiFigure));
            gamefield.TryMove(new Vector(0.0, 1.0));
            gamefield.TryMove(new Vector(0.0, 1.0));
            gamefield.Merge();

            var figure = new FigureGizmo(FigureFlyweightFactory.TiFigure);
            figure.ClockwiseRotate();
            figure.ClockwiseRotate();
            gamefield.TryAddFigure(figure);
            gamefield.Merge();

            var expected = new int[0];

            Assert.That(gamefield.RemoveFilledLines(), Is.EquivalentTo(expected));
        }

        #endregion


        #region GetFigureStack

        [ Test ]
        public void GetFigureStack_FieldIsEmpty_ReturnsEmptyArray()
        {
            var gameField = GetGameField( 1, 1 );
            Assert.That( gameField.GetFigureStack().data, Is.Empty );
        }

        [Test]
        public void GetFigureStack_FieldIsFull_ReturnsFullHeightArray()
        {
            var gameField = GetGameField(3, 2);
            gameField.TryAddFigure( new FigureGizmo( FigureFlyweightFactory.TiFigure ) );
            gameField.Merge();

            Assert.That(gameField.GetFigureStack().data.Length, Is.EqualTo( 2 ));
        }

        #endregion


        #region GetActiveFigure

        [ Test ]
        public void GetActiveFigure_FigureIsEmpty_ReturnsEmptyArray()
        {
            var gameField = GetGameField( 1, 1 );
            Assert.That( gameField.GetActiveFigure().data, Is.Empty );
        }

        [Test]
        public void GetActiveFigure_FigureIsNotEmpty_ReturnsFilledArray()
        {
            var gameField = GetGameField(3, 2);
            gameField.TryAddFigure(new FigureGizmo(FigureFlyweightFactory.TiFigure));

            Assert.That(gameField.GetActiveFigure().data, Is.Not.Empty);
        }

        #endregion


        #region TryRotateFigure

        [ Test ]
        public void TryRotateFigure_FigureIsEmpty_ReturnsFalse()
        {
            var gameField = GetGameField( 1, 1 );
            Assert.False( gameField.TryRotateFigure( RotateDirections.Clockwise ) );
        }

        [Test]
        public void TryRotateFigure_FigureIsNotEmpty_ReturnsTrue()
        {
            var gameField = GetGameField(10, 10);
            gameField.TryAddFigure( new FigureGizmo( FigureFlyweightFactory.TiFigure ) );

            Assert.True(gameField.TryRotateFigure(RotateDirections.Clockwise));
        }

        #endregion


        #region Factory

        private IGameField GetGameField( int width, int height )
        {
            var gamefield = new GameField( new VectorSpinner(), width, height );

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
            var gamefield = new GameField( new VectorSpinner(), 5, 4);
            gamefield.TryAddFigure( new FigureGizmo( FigureFlyweightFactory.TiFigure ) );
            gamefield.TryMove( new Vector( 0, 1.0 ) );
            gamefield.TryMove( new Vector( 0, 1.0 ) );
            gamefield.Merge();

            return gamefield;
        }

        #endregion
    }
}
