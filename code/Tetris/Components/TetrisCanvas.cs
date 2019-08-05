using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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
        #region Fields

        private readonly List< Visual > _visuals = new List< Visual >();
        private readonly Pen _pen = new Pen( Brushes.Black, 0.05 );
        private readonly IDictionary< Color, Brush > _brushes = new Dictionary< Color, Brush >(10);
        private readonly GuidelineSet _guidelineSet = new GuidelineSet();

        #endregion


        #region Ctor

        static TetrisCanvas()
        {
            WidthProperty.OverrideMetadata( typeof( TetrisCanvas ), new FrameworkPropertyMetadata(WidthChanged) );
            HeightProperty.OverrideMetadata( typeof( TetrisCanvas ), new FrameworkPropertyMetadata(HeightChanged) );
        }

        #endregion


        #region Width & Height

        private static void WidthChanged( DependencyObject obj, DependencyPropertyChangedEventArgs args )
        {
            var tetrisCanvas = ( TetrisCanvas )obj;
            
            tetrisCanvas.UpdateHorizontalGuidelines( ( int )(double)args.NewValue );
        }

        private static void HeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var tetrisCanvas = (TetrisCanvas)obj;
            tetrisCanvas.UpdateVerticalGuidelines((int)(double)args.NewValue);
        }

        public void UpdateHorizontalGuidelines( int width )
        {
            this.VerifyAccess();

            var penHalfWidth = _pen.Thickness / 2;

            _guidelineSet.GuidelinesY.Clear();
            _guidelineSet.GuidelinesY.Add( penHalfWidth );
            _guidelineSet.GuidelinesY.Add( width + penHalfWidth );
        }

        public void UpdateVerticalGuidelines(int height)
        {
            this.VerifyAccess();

            var penHalfWidth = _pen.Thickness / 2;

            _guidelineSet.GuidelinesX.Clear();
            _guidelineSet.GuidelinesX.Add(penHalfWidth);
            _guidelineSet.GuidelinesX.Add(height + penHalfWidth);
            
        }

        #endregion


        #region GameObjectsSourceProperty

        public static readonly DependencyProperty GameObjectsSourceProperty = DependencyProperty.Register(
            "GameObjectsSource",
            typeof(ReadOnlyObservableCollection<(Color?[][] data, int left, int top)>),
            typeof(TetrisCanvas),
            new FrameworkPropertyMetadata(
                (ReadOnlyObservableCollection<(Color?[][] data, int left, int top)>)null,
                new PropertyChangedCallback(GameFieldDataChanged)
            )
        );

        private static void GameFieldDataChanged( DependencyObject d, DependencyPropertyChangedEventArgs args )
        {
            TetrisCanvas canvas = ( TetrisCanvas )d;
            var oldValue = ( ReadOnlyObservableCollection< (Color?[][], int, int) > )args.OldValue;
            var newValue = ( ReadOnlyObservableCollection< (Color?[][], int, int) > )args.NewValue;

            if ( oldValue != null ) {
                canvas.ClearGameObjects( oldValue );
            }

            if ( args.NewValue != null ) {
                canvas.AddGameObjects( newValue );
            }
        }

        public ReadOnlyObservableCollection<(Color?[][] data, int left, int top)> GameObjectsSource
        {
            get => ( ReadOnlyObservableCollection<(Color?[][] data, int left, int top)> )GetValue( GameObjectsSourceProperty );
            set => SetValue( GameObjectsSourceProperty, value );
        }


        internal void ClearGameObjects( ReadOnlyObservableCollection< (Color?[][], int, int) > col )
        {
            (( INotifyCollectionChanged )col).CollectionChanged -= OnGameObjectCollectionChanged;

            foreach ( var visual in _visuals ) {
                RemoveVisual( visual );
            }

            _visuals.Clear();
        }

        internal void AddGameObjects( ReadOnlyObservableCollection<(Color?[][], int, int)> col )
        {
            foreach (var element in col)
            {
                AddVisual( CreateVisual( element ) );
            }

            ((INotifyCollectionChanged)col).CollectionChanged += OnGameObjectCollectionChanged;
        }


        private Visual CreateVisual( (Color?[][] data, int left, int top) val )
        {
            (Color?[][] data, int left, int top) = val;

            var visual = new DrawingVisual();
            if ( !data.Any() ) return visual;

            using ( var context = visual.RenderOpen() ) 
            {
                // ReSharper disable once ForCanBeConvertedToForeach
                for ( int i = 0; i < data.Length; i++ ) {
                    for ( int j = 0; j < data[i].Length; j++ ) 
                    {
                        if ( data[ i ][ j ].HasValue ) 
                        {
                            if ( !_brushes.ContainsKey( data[ i ][ j ].Value ) ) {
                                _brushes[ data[i][j].Value ] = new SolidColorBrush( data[i][j].Value );
                            }
                            context.PushGuidelineSet( _guidelineSet );
                            context.DrawRectangle( _brushes[ data[i][j].Value], _pen, new Rect( new Point( left + j, top + i ), new Size( 1, 1 )) );
                        }
                    }
                }
            }
            
            return visual;
        }

        private void OnGameObjectCollectionChanged( object sender, NotifyCollectionChangedEventArgs args )
        {
            if ( args.OldStartingIndex == args.NewStartingIndex ) 
            {
                var visual = args.NewItems?[ 0 ] != null 
                                    ? CreateVisual( ( (Color?[][] data, int left, int top) )args.NewItems[ 0 ] ) 
                                    : new DrawingVisual();

                RemoveVisualChild( _visuals[args.NewStartingIndex] );
                RemoveLogicalChild( _visuals[args.NewStartingIndex] );
                _visuals[args.NewStartingIndex] = visual;
                AddVisualChild( _visuals[args.NewStartingIndex] );
                AddLogicalChild( _visuals[args.NewStartingIndex] );

                return;
            }

            if ( args.NewItems?[ 0 ] != null ) {
                var visual = CreateVisual( ( (Color?[][] data, int left, int top) )args.NewItems[ 0 ] );
                AddVisual( visual );

            }

            if ( args.OldItems?[ 0 ] != null ) {
                RemoveVisual( args.OldStartingIndex );
            }
        }

        #endregion


        #region Visuals

        protected override Visual GetVisualChild(int index) => _visuals[index];
        protected override int VisualChildrenCount => _visuals.Count;
        
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
        private void RemoveVisual(int index)
        {
            var visual = _visuals[ index ];
            _visuals.RemoveAt(index);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        #endregion
    }
}
