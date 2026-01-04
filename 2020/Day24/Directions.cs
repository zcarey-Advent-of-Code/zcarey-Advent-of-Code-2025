using AdventOfCode.Parsing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Day24 {
	class Directions : IObjectParser<string> {

		//Using the axial coordinate system for grid coordinates
		//https://www.redblobgames.com/grids/hexagons/#coordinates-axial
		private Point[] offsets;

		public void Parse(string input) {
			offsets = convert(input).ToArray();
		}

		private IEnumerable<Point> convert(string input) {
			IEnumerator<char> chars = input.GetEnumerator();
			while (chars.MoveNext()) {
				switch (chars.Current) {
					case 'w': 
						yield return new Point(-1, 0);
						break;
					case 'e':
						yield return new Point(1, 0);
						break;
					case 'n':
						if (!chars.MoveNext()) throw new ArgumentOutOfRangeException("input", "Bad input, out of characters.");
						yield return new Point((chars.Current == 'e') ? 1 : 0, -1);
						break;
					case 's':
						if (!chars.MoveNext()) throw new ArgumentOutOfRangeException("input", "Bad input, out of characters.");
						yield return new Point((chars.Current == 'w') ? -1 : 0, 1);
						break;
					default:
						throw new ArgumentOutOfRangeException("input", "Bad input.");
				}
			}
		}

		public Point GetAbsoluteOffset(Point origin = new Point()) {
			foreach(Point offset in offsets) {
				origin.Offset(offset);
			}
			return origin;
		}

	}

	public static class HexagonalPointExtensions {

		//Using the axial coordinate system for grid coordinates
		//https://www.redblobgames.com/grids/hexagons/#coordinates-axial
		public static IEnumerable<Point> HexagonNeighbors(this Point point) {
			yield return new Point(point.X - 1, point.Y);
			yield return new Point(point.X, point.Y - 1);
			yield return new Point(point.X + 1, point.Y - 1);
			yield return new Point(point.X + 1, point.Y);
			yield return new Point(point.X, point.Y + 1);
			yield return new Point(point.X - 1, point.Y + 1);
		}

	}
}
