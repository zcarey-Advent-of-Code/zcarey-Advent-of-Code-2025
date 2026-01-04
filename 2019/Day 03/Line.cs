using AdventOfCode.Visualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Day_03 {
	public class Line {

		public Point Point1;
		public Point Point2;
		public bool IsHorizontal = true;

		public Point Start;
		public Point End;

		// Manhattan distance
		public int Distance {
			get {
				return Math.Abs(Point1.X - Point2.X) + Math.Abs(Point1.Y + Point2.Y);
			}
		}

		public Line(Point p1, Point p2) {
			this.Start = p1;
			this.End = p2;
			bool reversed = false;

			// Attempt to organize points
			if(p1.Y == p2.Y) {
				// Organize horizontal line
				reversed = (p2.X < p1.X);
				IsHorizontal = true;
			} else if(p1.X == p2.X) {
				// Organize vertical line
				reversed = (p2.Y < p1.Y);
				IsHorizontal = false;
			}

			if (!reversed) {
				this.Point1 = p1;
				this.Point2 = p2;
			} else {
				this.Point1 = p2;
				this.Point2 = p1;
			}
		}

		public bool Intersection(Point p) {
            if (IsHorizontal) {
				return p.Y == this.Point1.Y && p.X >= this.Point1.X && p.X <= this.Point2.X;
            } else {
				return p.X == this.Point1.X && p.Y >= this.Point1.Y && p.Y <= this.Point2.Y;
            }
        }

		public IEnumerable<Point> Intersection(IEnumerable<Point> points) {
			foreach(Point p in points) {
                if (Intersection(p)) {
					yield return p;
                }
            }
        }

		public Point? Intersection(Line line) {
			if(IsHorizontal == line.IsHorizontal) {
				// Parallel lines don't intersect
				return null;
			}

			if (IsHorizontal) {
				Point p = new Point(line.Point1.X, this.Point1.Y);
				if(    p.X >= this.Point1.X 
					&& p.X <= this.Point2.X
					&& p.Y >= line.Point1.Y
					&& p.Y <= line.Point2.Y
				) {
					return p;
				}
			} else {
				// Saved me a copy/paste lol
				return line.Intersection(this);
			}

			return null;
		}

		public void Visualize(Visualizer visualizer) {
			visualizer[Point1] = '+';
			visualizer[Point2] = '+';

			if (IsHorizontal) {
				for (int x = Point1.X + 1; x < Point2.X; x++) {
					visualizer[x, Point1.Y] = '-';
				}
			} else {
				for(int y = Point1.Y + 1; y < Point2.Y; y++) {
					visualizer[Point1.X, y] = '|';
				}
			}
		}

	}
}
