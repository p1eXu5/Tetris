using System.Windows;
using NUnit.Framework;
using Tetris.Models;

namespace Tetris.Tests.UnitTests
{

    [TestFixture]
    public class DirectionVectorsTests
    {
        [Test]
        public void FromDirection_LeftMoveDirection_ReturnsLeftVector()
        {
            var direction = MoveDirections.Left;
            var resVector = new Vector( -1.0, 0.0 );
            Assert.That( DirectionVectors.FromDirection( direction ), Is.EqualTo( resVector ) );
        }

        [Test]
        public void FromDirection_RightMoveDirection_ReturnsRightVector()
        {
            var direction = MoveDirections.Right;
            var resVector = new Vector(1.0, 0.0);
            Assert.That(DirectionVectors.FromDirection(direction), Is.EqualTo(resVector));
        }

        [Test]
        public void FromDirection_DownMoveDirection_ReturnsDownVector()
        {
            var direction = MoveDirections.Down;
            var resVector = new Vector(0.0, 1.0);
            Assert.That(DirectionVectors.FromDirection(direction), Is.EqualTo(resVector));
        }

        [Test]
        public void FromDirection_UpMoveDirection_ReturnsUpVector()
        {
            var direction = MoveDirections.Up;
            var resVector = new Vector(0.0, -1.0);
            Assert.That(DirectionVectors.FromDirection(direction), Is.EqualTo(resVector));
        }

        #region Factory
        // Insert factory methods and test class variables hear:

        #endregion
    }

}
