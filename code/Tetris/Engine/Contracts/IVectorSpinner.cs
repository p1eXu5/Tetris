using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tetris.Engine.Contracts
{
    public interface IVectorSpinner
    {
        IEnumerable< Vector > GetVectors( int width, int height );
    }
}
