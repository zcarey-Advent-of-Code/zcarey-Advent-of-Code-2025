using AdventOfCode.Parsing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_05 {
    public struct Line : IObjectParser<Point[]> {

        public Point Point1;
        public Point Point2;
        public LineType Type;

        public void Parse(Point[] input) {
            Point1 = input[0];
            Point2 = input[1];

            if (Point1.X == Point2.X) {
                Type = LineType.Vertical;
            } else if (Point1.Y == Point2.Y) {
                Type = LineType.Horizontal;
            } else {
                Type = LineType.Diagonal;
            }
        }

        public IEnumerable<Point> GetPoints(bool IncludeDiagonal = false) {
            if (Type == LineType.Diagonal && !IncludeDiagonal) {
                yield break; // Part 1 only do horizontal or vertical lines
            }

            int dx = (Point1.X == Point2.X) ? 0 : ((Point2.X - Point1.X) / Math.Abs(Point2.X - Point1.X));
            int dy = (Point1.Y == Point2.Y) ? 0 : ((Point2.Y - Point1.Y) / Math.Abs(Point2.Y - Point1.Y));
            int x = Point1.X;
            int y = Point1.Y;
            while (x != Point2.X || y != Point2.Y) {
                yield return new Point(x, y);
                x += dx;
                y += dy;
            }

            yield return Point2;
        }
    }

    public enum LineType {
        Horizontal,
        Vertical,
        Diagonal
    }
}
