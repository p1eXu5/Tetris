using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Engine;
using Tetris.Engine.Contracts;
using Tetris.Tests.UnitTests.TestCases;

namespace Tetris.Tests.UnitTests
{

    [TestFixture]
    public class ScoreAgentTests
    {
        [Test]
        public void AddLines_LineCountGreaterMaxHeight_Throws()
        {
            var agent = GetScoreAgent();

            Assert.Throws< ArgumentException >( () => agent.AddLines( FigureFlyweightFactory.MAX_HEIGHT + 1 ) );
        }

        [TestCaseSource( typeof( ScoreAgentTestCases ), nameof( ScoreAgentTestCases.ReturnExpectedScoresAndMultiplierCases ) )]
        public (int scores, byte multiplier) AddLines_ByDefault_SetsExpectedScoresAndMultiplier( int[] lineCounts )
        {
            var agent = GetScoreAgent();

            foreach ( var lineCount in lineCounts ) {
                agent.AddLines( lineCount );
            }

            return (agent.Scores, agent.Multiplier);
        }

        #region Factory
        // Insert factory methods and test class variables hear:
        private IScoreAgent GetScoreAgent()
        {
            return new ScoreAgent();
        }

        #endregion
    }

}
