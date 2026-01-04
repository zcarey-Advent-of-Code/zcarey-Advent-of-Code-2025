using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils {
    public class Bounds {

        ///<summary> Inclusive of the left-most point of the bounds </summary>
        public int X { get => Left; }

        ///<summary> Inclusive of the top-most point of the bounds </summary>
        public int Y { get => Top; }

        /// <summary> Width of the bounds. When Left == Right, width would equal 1 (1 grid cell of width) </summary>
        public int Width { get => Right - Left + 1; }

        /// <summary> Height of the bounds. When Top == Bottom, height would equal 1 (1 grid cell of height) </summary>
        public int Height { get => Bottom - Top + 1; }

        public Point Location { get => new Point(X, Y); }
        public Size Size { get => new Size(Width, Height); }
        public Rectangle Region {get => new Rectangle(X, Y, Width, Height); }

        ///<summary> Inclusive of the left-most point </summary>
        public int Left { get; private set; } = 0;

        ///<summary> Inclusive of the right-most point </summary>
        public int Right { get; private set; } = 0;

        ///<summary> Inclusive of the top-most point </summary>
        public int Top { get; private set; } = 0;

        ///<summary> Inclusive of the right-most point </summary>
        public int Bottom { get; private set; } = 0;

        public Bounds(int x, int y) {
            this.Left = x;
            this.Right = x;
            this.Top = y;
            this.Bottom = y;
        }

        public Bounds(Point point) : this(point.X, point.Y) {

        }

        public void Add(int x, int y) {
            Left = Math.Min(Left, x);
            Right = Math.Max(Right, x);
            Top = Math.Min(Top, y);
            Bottom = Math.Max(Bottom, y);
        }

        public void Add(Point p) => Add(p.X, p.Y);

    }
}
