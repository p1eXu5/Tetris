using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Agbm.Wpf.MvvmBaseLibrary;
using Tetris.Contracts;

namespace Tetris
{
    public class MainViewModel : ViewModel
    {
        private readonly ITetrisEngine _tetrisEngine;
        private int[] _removableLines;

        public MainViewModel( ITetrisEngine tetrisEngine )
        {
            _tetrisEngine = tetrisEngine ?? throw new ArgumentNullException(nameof(tetrisEngine), @"TetrisEngine cannot be null.");
            _tetrisEngine.RemovableLinesFormed += ( s, e ) => {
                RemovableLines = e;
            };
        }

        public int Width => _tetrisEngine.GameFieldWidth;
        public int Height => _tetrisEngine.GameFieldHeight;
        public ReadOnlyObservableCollection< (Color?[][], int, int ) > GameObjectCollection => _tetrisEngine.GameObjectCollection;

        public int[] RemovableLines
        {
            get => _removableLines;
            set {
                _removableLines = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartGameCommand => new MvvmAsyncCommand( StartGame, o => !_tetrisEngine.IsRunning );

        private async Task StartGame( object o )
        {
            await _tetrisEngine.StartNewGameAsync();
        }
    }
}
