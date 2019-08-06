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
using Tetris.Models;

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

        public ICommand StartGameCommand => new MvvmAsyncCommand( StartGameAsync, o => !_tetrisEngine.IsRunning );
        public ICommand MoveLeftCommand => new MvvmAsyncCommand( async _ => await MoveFigureAsync( Directions.Left ), 
                                                                 o => _tetrisEngine.CanManipulate );
        public ICommand MoveRightCommand => new MvvmAsyncCommand( async _ => await MoveFigureAsync( Directions.Right ), 
                                                                  o => _tetrisEngine.CanManipulate);
        public ICommand RotateClockwiseCommand => new MvvmAsyncCommand( async _ => await RotateFigureAsync( RotateDirections.Clockwise ), 
                                                                        o => _tetrisEngine.CanManipulate);
        public ICommand RotateCounterclockwiseCommand => new MvvmAsyncCommand( async _ => await RotateFigureAsync( RotateDirections.Couterclockwise ), 
                                                                               o => _tetrisEngine.CanManipulate);
        public ICommand FallDownCommand => new MvvmAsyncCommand( async _ => await _tetrisEngine.DropFigureAsync(), 
                                                                 o => _tetrisEngine.CanManipulate );


        private async Task StartGameAsync( object o )
        {
            await _tetrisEngine.StartNewGameAsync( TaskScheduler.FromCurrentSynchronizationContext() );
            _tetrisEngine.UpdateField();
            _tetrisEngine.UpdateFigure();
        }

        private async Task MoveFigureAsync( Directions direction )
        {
            var canMove = await _tetrisEngine.MoveFigureAsync( direction );
            if ( canMove )_tetrisEngine.UpdateFigure();
        }

        private async Task RotateFigureAsync( RotateDirections rotateDirection )
        {
            var canMove = await _tetrisEngine.RotateFigureAsync( rotateDirection );
            if (canMove) _tetrisEngine.UpdateFigure();
        }
    }
}
