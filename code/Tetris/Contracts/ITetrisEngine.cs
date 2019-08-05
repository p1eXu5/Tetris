using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tetris.Contracts
{
    public interface ITetrisEngine: IDisposable
    {
        event EventHandler<int[]> RemovableLinesFormed;

        int GameFieldWidth { get; }
        int GameFieldHeight { get; }

        bool IsRunning { get; }
        bool HasActiveFigure { get; }

        ReadOnlyObservableCollection<(Color?[][] data, int left, int top)> GameObjectCollection { get; }
        Color?[][] GetGameField();

        void StartNewGame();
        Task StartNewGameAsync();
        Task MoveFigureLeftAsync();
        Task MoveFigureRightAsync();

        Task RotateFigureClockwiseAsync();
        Task RotateFigureCounterclockwiseAsync();

        Task DropFigureAsync();
    }
}
