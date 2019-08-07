using System;
using Tetris.Engine.Contracts;

namespace Tetris.Engine
{
    public class ScoreAgent : IScoreAgent
    {
        public int Scores { get; private set; }
        public byte Multiplier { get; private set; }
        public void AddLines( int lineCount )
        {
            if ( lineCount > FigureFlyweightFactory.MAX_HEIGHT ) throw new ArgumentException();

            Scores += lineCount * Multiplier;

            if ( Multiplier > lineCount ) {
                Multiplier = (byte)lineCount;
                return;
            }

            if ( lineCount <= 2 ) {
                Multiplier = (byte)lineCount;
                return;
            }

            Multiplier += (byte)lineCount;
        }

        public void Reset()
        {
            Multiplier = 1;
            Scores = 0;
        }
    }
}
