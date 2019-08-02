using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tetris.Components
{
    public class TetrisCanvas : Canvas
    {
        public static readonly DependencyProperty GameFieldDataProperty;

        static TetrisCanvas()
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata( 
                new Color?[0][], 
                new PropertyChangedCallback( GameFieldDataChanged ) 
            );

            metadata.CoerceValueCallback = CoerceGameField;

            GameFieldDataProperty = DependencyProperty.Register( 
                "GameFieldData", 
                typeof( Color?[][] ), 
                typeof( TetrisCanvas ), 
                metadata
            );
        }

        private readonly List< Visual > _visuals = new List< Visual >();
        private readonly Brush _brush = Brushes.Brown;
        private readonly Pen _pen = new Pen( Brushes.Black, 1.0 );
        private int _gameFieldWidth;
        private int _gameFieldHeight;
        private double _ki;
        private double _kj;

        public TetrisCanvas()
        {
            Loaded += AddSquare;
        }

        public Size GameFieldData
        {
            get => ( Size )GetValue( GameFieldDataProperty );
            set => SetValue( GameFieldDataProperty, value );
        }

        protected override Visual GetVisualChild(int index) => _visuals[index];

        protected override int VisualChildrenCount => _visuals.Count;

        private static void GameFieldDataChanged( DependencyObject d, DependencyPropertyChangedEventArgs args )
        {
            TetrisCanvas canvas = ( TetrisCanvas )d;
            canvas.Update( args.NewValue as Color?[][] );
        }

        private static object CoerceGameField( DependencyObject d, object value )
        {
            if ( value == null ) {
                return new Color?[0][];
            }

            return value;
        }

        public void Update( Color?[][] gameField )
        {
            //var visuals = new List< Visual >( gameField.Length * gameField?[0].Length ?? 0 );
            //for ( int i = 0; i < gameField.Length; i++ ) {
            //    for ( int j = 0; j < gameField[i].Length; j++ ) {
            //        if ( gameField[ i ][ j ].HasValue ) {
            //            visuals.Add( GetVisual( gameField[ i ][ j ], i, j ) );
            //        }
            //    }
            //}

            //Replace( visuals );
        }

        private void RefreshKoefs( Color?[][] gameField )
        {
            if ( gameField.Length == 0 ) {
                if ( _gameFieldHeight != 0 ) {
                    _ki = 0.0;
                    _kj = 0.0;
                    _gameFieldHeight ^= _gameFieldHeight;
                    if ( _gameFieldWidth != 0 ) _gameFieldWidth ^= _gameFieldWidth;
                }

                return;
            }


        }

        private Visual GetVisual( Color color, int i, int j )
        {
            var visual = new DrawingVisual();

            //using ( var context = visual.RenderOpen() ) {
            //    context.DrawRectangle( _brush, _pen, new Rect( new Point( i * ki )) );
            //}

            return visual;
        }

        private void AddVisual( Visual visual )
        {
            _visuals.Add( visual );

            AddVisualChild( visual );
            AddLogicalChild( visual );
        }

        private void RemoveVisual(Visual visual)
        {
            _visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }
        private void AddSquare( object sender, RoutedEventArgs args )
        {
            var visual = new DrawingVisual();

            using ( var context = visual.RenderOpen() ) {
                context.DrawRectangle( _brush, _pen, new Rect( new Size( 10, 10)) );
            }

            AddVisual( visual );
        }
    }
}
