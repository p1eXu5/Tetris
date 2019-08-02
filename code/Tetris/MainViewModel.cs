using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Agbm.Wpf.MvvmBaseLibrary;
using Tetris.Contracts;

namespace Tetris
{
    public class MainViewModel : ViewModel
    {
        private readonly ITetrisEngine _tetrisEngine;
        public MainViewModel( ITetrisEngine tetrisEngine )
        {
            _tetrisEngine = tetrisEngine ?? throw new ArgumentNullException(nameof(tetrisEngine), @"TetrisEngine cannot be null."); ;
        }

        public Color?[][] GameField => _tetrisEngine.GetGameField();
    }
}
