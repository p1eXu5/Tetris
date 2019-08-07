using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Tetris.Models;

namespace Tetris.Contracts
{
    public interface ITetrisEngine: IDisposable
    {
        event EventHandler<int[]> RemovableLinesFormed;

        int GameFieldWidth { get; }
        int GameFieldHeight { get; }

        bool IsRunning { get; }
        bool CanManipulate { get; }

        bool IsDropping { get; }

        ReadOnlyObservableCollection<(Color?[][] data, int left, int top)> GameObjectCollection { get; }

        Task StartNewGameAsync( TaskScheduler taskScheduler );

        void UpdateField();
        void UpdateFigure();

        Task<bool> MoveFigureAsync( MoveDirections moveDirection );


        Task< bool > RotateFigureAsync( RotateDirections rotateDirections );


        void DropFigure();
    }
}
