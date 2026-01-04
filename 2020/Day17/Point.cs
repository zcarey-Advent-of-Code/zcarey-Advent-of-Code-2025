using System;
using System.Collections.Generic;
using System.Text;

namespace Day17 {
	struct Point {

		public int X;
		public int Y;
		public int Z;
		public int W;

		public Point(int x, int y, int z, int w) {
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		public void Offset(int dx, int dy, int dz, int dw) {
			X += dx;
			Y += dy;
			Z += dz;
			W += dw;
		}

		public void Offset(Point delta) {
			X += delta.X;
			Y += delta.Y;
			Z += delta.Z;
			W += delta.W;
		}

		public static Point operator +(Point left, Point right) {
			return new Point(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		}

		public static Point operator -(Point left, Point right) {
			return new Point(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		}

		public override int GetHashCode() {
			return unchecked(X + (31 * Y) + (31 * 31 * Z) + (31 * 31 * 31 * W));
		}

	}
}
