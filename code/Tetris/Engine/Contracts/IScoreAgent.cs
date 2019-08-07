using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Engine.Contracts
{
    public interface IScoreAgent
    {
        int Scores { get; }
        byte Multiplier { get; }
        void AddLines( int lineCount );

        void Reset();
    }
}
