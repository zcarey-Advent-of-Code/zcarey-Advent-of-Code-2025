using AdventOfCode.Parsing;
using AdventOfCode.Visualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Day_03 {
	public class Wire : IObjectParser<IEnumerable<WireDirection>> {

		public List<Line> WireLines = new List<Line>();
		private string debugString;

		public void Parse(IEnumerable<WireDirection> input) {
			List<string> debug = new List<string>();
			Point loc = new Point(); // Start at origin, 0, 0
			debug.Add(loc.ToString());
			foreach(WireDirection dir in input) {
				Point p1 = loc;
				loc += dir;
				WireLines.Add(new Line(p1, loc));
				debug.Add(loc.ToString());
			}
			debugString = string.Join(", ", debug);
		}

		public IEnumerable<Point> Intersections(Wire wire) {
			foreach(Line thisLine in WireLines) {
				foreach(Line thatLine in wire.WireLines) {
					Point? intersection = thisLine.Intersection(thatLine);
					if(intersection != null) {
						yield return (Point)intersection;
					}
				}
			}
		}

		public Rectangle Bounds() {
			int x1 = 0;
			int x2 = 0;
			int y1 = 0;
			int y2 = 0;

			foreach(Point p in WireLines.SelectMany(x => new List<Point>() { x.Point1, x.Point2 })) {
				x1 = Math.Min(x1, p.X);
				x2 = Math.Max(x2, p.X);
				y1 = Math.Min(y1, p.Y);
				y2 = Math.Max(y2, p.Y);
			}

			return new Rectangle(x1, y1, x2 - x1, y2 - y1);
		}

		public void Visualize(Visualizer visualizer) {
			foreach (Line line in WireLines) {
				line.Visualize(visualizer);
			}
		}

        public override string ToString() {
            return debugString;
        }
    }
}
