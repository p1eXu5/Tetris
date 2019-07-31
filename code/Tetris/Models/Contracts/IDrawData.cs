using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tetris.Models.Contracts
{
    public interface IDrawData
    {
        IEnumerable< (int x, int y, Color color) > GetDrawData();
    }
}
