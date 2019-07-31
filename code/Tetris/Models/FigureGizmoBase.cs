﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Tetris.Models.Contracts;

namespace Tetris.Models
{
    public abstract class FigureGizmoBase : ILiveFigureGizmo, IFigureGizmo
    {
        protected int _angle;

        public abstract IFigure Figure { get; }
        public abstract Point Center { get; set; }
        public int Width => GetWidth( _angle );
        public int Height => GetHeight( _angle );
        public abstract Color Color { get; }

        public abstract int Angle { get; }

        public void CounterclockwiseRotate()
        {
            _angle = Angle + 90;
            if (_angle >= 360)
            {
                _angle %= 360;
            }
        }

        public void ClockwiseRotate()
        {
            _angle = Angle - 90;
            if (_angle < 0)
            {
                _angle = 270;
            }
        }

        public int GetWidth( int angle )
        {
            switch (angle)
            {
                case 0:
                case 180:
                    return Figure.Width;
                case 90:
                case 270:
                    return Figure.Height;
            }

            throw new InvalidOperationException("Wrong angle");
        }
        public int GetHeight( int angle )
        {
            switch (angle)
            {
                case 0:
                case 180:
                    return Figure.Height;
                case 90:
                case 270:
                    return Figure.Width;
            }

            throw new InvalidOperationException("Wrong angle");
        }

        public int Top => (int)(Center.Y - Height / 2.0);
        public int Bottom => Top + Height; 
        public int Left => (int)(Center.X - Width / 2.0);
        public int Right => Left + Width;

        public Color? this[int i, int j]
        {
            get {
                switch (_angle)
                {
                    case 0:
                        return Figure[i, j];
                    case 90:
                        return Figure[j, Figure.Width - i - 1];
                    case 180:
                        return Figure[Figure.Height - i - 1, Figure.Width - j - 1];
                    case 270:
                        return Figure[Figure.Height - j - 1, i];
                }

                throw new InvalidOperationException("Wrong angle");
            }
        }

        public bool IsEmptyGizmo => Figure.Width == 0 && Figure.Height == 0;

        public void MoveTo(Point point)
        {
            Center = point;
        }
    }
}
