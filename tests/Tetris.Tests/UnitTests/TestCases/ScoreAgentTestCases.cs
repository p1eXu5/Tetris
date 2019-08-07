using System.Collections;
using NUnit.Framework;

namespace Tetris.Tests.UnitTests.TestCases
{
    public class ScoreAgentTestCases
    {
        public static IEnumerable ReturnExpectedScoresAndMultiplierCases()
        {
            var i = 0;

            var seq0 = new[] { 1, 2, 3, 4 };
            yield return new TestCaseData( seq0 ).Returns( (29, 9 ) ).SetName( $"ReturnExpectedScoresAndMultiplierCase #{i++}" );

            var seq1 = new[] { 1, 2, 3, 4, 1 };
            yield return new TestCaseData( seq1 ).Returns( (38, 1) ).SetName($"ReturnExpectedScoresAndMultiplierCase #{i++}");

            var seq2 = new[] { 1, 4, 2, 4 };
            yield return new TestCaseData( seq2 ).Returns( (21, 6) ).SetName($"ReturnExpectedScoresAndMultiplierCase #{i++}");

            var seq3 = new[] { 1, 3, 2, 1, 3, 4 };
            yield return new TestCaseData( seq3 ).Returns( (27, 7) ).SetName($"ReturnExpectedScoresAndMultiplierCase #{i}");
        }
    }
}
