using System;
using Tetris.Engine.Contracts;

namespace Tetris.Engine
{
    public class ScoreAgent : IScoreAgent
    {
        private byte _lastLineCount;
        public int Scores { get; private set; }
        public byte Multiplier { get; private set; } = 1;
        public void AddLines( int lineCount )
        {
            if ( lineCount > FigureFlyweightFactory.MAX_HEIGHT ) throw new ArgumentException();

            Scores += lineCount * Multiplier;

            if ( lineCount < _lastLineCount ) {
                Multiplier = (byte)lineCount;
            }
            else {
                if ( _lastLineCount == 1 || lineCount <= 2 ) {
                    Multiplier = (byte)lineCount;
                }
                else {
                    Multiplier += (byte)lineCount;
                }

            }

            _lastLineCount = (byte)lineCount;
        }

        public void Reset()
        {
            Multiplier = 1;
            Scores = 0;
        }
    }
}
