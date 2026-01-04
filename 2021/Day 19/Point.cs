using AdventOfCode.Parsing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_19 {
    public record Point(int X = 0, int Y = 0, int Z = 0) : IEnumerable<int> {

        public Point(Point copy) {
            this.X = copy.X;
            this.Y = copy.Y;
            this.Z = copy.Z;
        }

        public static Point Parse(string input) {
            int[] data = input.Split(',').Select(int.Parse).ToArray();
            return new Point(data[0], data[1], data[2]);
        }

        public Point Abs {
            get => new Point(
                Math.Abs(this.X),
                Math.Abs(this.Y),
                Math.Abs(this.Z)
            );
        }

        public Point Transform(Point center, int rotation) {
            var (x, y, z) = (this.X, this.Y, this.Z);

            // rotate coordinate system so that x-axis points in the possible 6 directions
            switch (rotation % 6) {
                case 0: (x, y, z) = (x, y, z); break;
                case 1: (x, y, z) = (-x, y, -z); break;
                case 2: (x, y, z) = (y, -x, z); break;
                case 3: (x, y, z) = (-y, x, z); break;
                case 4: (x, y, z) = (z, y, -x); break;
                case 5: (x, y, z) = (-z, y, x); break;
            }

            // rotate around x-axis:
            switch ((rotation / 6) % 4) {
                case 0: (x, y, z) = (x, y, z); break;
                case 1: (x, y, z) = (x, -z, y); break;
                case 2: (x, y, z) = (x, -y, -z); break;
                case 3: (x, y, z) = (x, z, -y); break;
            }

            return center + new Point(x, y, z);
        }

        public static Point operator -(Point pt) {
            return new Point(-pt.X, -pt.Y, -pt.Z);
        }

        public static Point operator +(Point left, Point right) {
            return new Point(
                left.X + right.X,
                left.Y + right.Y,
                left.Z + right.Z
            );
        }

        public static Point operator -(Point left, Point right) {
            return new Point(
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z
            );
        }

        public IEnumerator<int> GetEnumerator() {
            return GetValues().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetValues().GetEnumerator();
        }

        private IEnumerable<int> GetValues() {
            yield return this.X;
            yield return this.Y;
            yield return this.Z;
        }
    }
}
