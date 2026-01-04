using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Day_03 {
	public struct WireDirection : IObjectParser<string> {

		public int dx;
		public int dy;

		public void Parse(string input) {
			dx = 0;
			dy = 0;

			if(input[0] == 'R') {
				dx = 1;
			}else if(input[0] == 'L') {
				dx = -1;
			}else if(input[0] == 'U') {
				dy = 1;
			}else if(input[0] == 'D') {
				dy = -1;
			}

			int n = int.Parse(input.Substring(1));
			dx *= n;
			dy *= n;
		}

		public static Point operator +(Point left, WireDirection right) {
			return new Point(left.X + right.dx, left.Y + right.dy);
		}

	}
}
