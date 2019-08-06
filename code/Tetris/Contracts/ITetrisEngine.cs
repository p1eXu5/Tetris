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

        ReadOnlyObservableCollection<(Color?[][] data, int left, int top)> GameObjectCollection { get; }

        Task StartNewGameAsync( TaskScheduler taskScheduler );

        void UpdateField();
        void UpdateFigure();

        Task<bool> MoveFigureAsync( Directions direction );


        Task< bool > RotateFigureAsync( RotateDirections rotateDirections );


        Task DropFigureAsync();
    }
}
