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
        private string _commandName = "";

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


        public string CommandName
        {
            get => _commandName;
            set {
                _commandName = value;
                OnPropertyChanged();
            }
        }

        public int[] RemovableLines
        {
            get => _removableLines;
            set {
                _removableLines = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartGameCommand => new MvvmAsyncCommand( async _ => await _tetrisEngine.StartNewGameAsync(), 
                                                                  o => !_tetrisEngine.IsRunning );
        public ICommand MoveLeftCommand => new MvvmAsyncCommand( async _ => await _tetrisEngine.MoveFigureLeftAsync(), 
                                                                 o => _tetrisEngine.IsRunning && _tetrisEngine.HasActiveFigure );
        public ICommand MoveRightCommand => new MvvmAsyncCommand( async _ => await _tetrisEngine.MoveFigureRightAsync(), 
                                                                  o => _tetrisEngine.IsRunning && _tetrisEngine.HasActiveFigure );
        public ICommand RotateClockwiseCommand => new MvvmAsyncCommand( async _ => await _tetrisEngine.RotateFigureClockwiseAsync(), 
                                                                        o => _tetrisEngine.IsRunning && _tetrisEngine.HasActiveFigure );
        public ICommand RotateCounterclockwiseCommand => new MvvmAsyncCommand( async _ => await _tetrisEngine.RotateFigureCounterclockwiseAsync(), 
                                                                               o => _tetrisEngine.IsRunning && _tetrisEngine.HasActiveFigure );
        public ICommand FallDownCommand => new MvvmAsyncCommand( async _ => await _tetrisEngine.DropFigureAsync(), 
                                                                 o => _tetrisEngine.IsRunning && _tetrisEngine.HasActiveFigure );


        private async Task StartGame( object o )
        {
            await _tetrisEngine.StartNewGameAsync();
        }

    }
}
