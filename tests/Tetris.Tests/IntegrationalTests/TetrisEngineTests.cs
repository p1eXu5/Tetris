using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Tetris.Contracts;
using Tetris.Models;

namespace Tetris.Tests.IntegrationalTests
{
    [TestFixture]
    public class TetrisEngineTests
    {
        [ Test ]
        public void TetrisEngine_BuDefault_CanFinishGame()
        {
            int count = 0;

            var engine = GetTetrisEngine();
            (( INotifyCollectionChanged )engine.GameObjectCollection).CollectionChanged += ( sender, args ) => ++count;

            var task = engine.StartNewGameAsync();
            task.Wait();

            while ( engine.IsRunning ) {
                Thread.Sleep( 0 );
            }

            Assert.IsFalse( engine.IsRunning );
            Assert.That( count, Is.GreaterThan( 0 ) );

            engine.Dispose();
        }

        #region Factory

        public ITetrisEngine GetTetrisEngine()
        {
            var figureFactory = new FigureFlyweightFactory();
            var gameField = new GameField( new VectorSpinner(), 10, 20 );
            return new TetrisEngine( figureFactory, gameField, 0 );
        }

        #endregion
    }
}
