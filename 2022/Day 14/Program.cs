using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Day_14 {
    internal class Program : ProgramStructure<Map> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .ForEach( line => {
                Point[] points = line.Split(" -> ").Select(ParsePoint).ToArray();
                Line[] lines = new Line[points.Length - 1];
                for(int i = 0; i < points.Length - 1; i++) {
                    lines[i] = new Line(points[i], points[i + 1]);
                }
                return lines;
            })
            .Parse(x => x.SelectMany(a => a))
            .ToArray()
            .Create<Map>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(Map input) {
            int count = 0;
            while (input.DropSand() != null) {
                count++;
            }
            return count;
        }

        protected override object SolvePart2(Map input) {
            // Ok we have to be a bit smarter this time
            int startX = 500 - input.Offset.Width;
            int startY = 0 - input.Offset.Height;

            BigInteger count = 1;
            bool[,] sand = new bool[input.Width, input.Height];
            sand[startX, startY] = true;

            for (int dy = 1; dy < input.FloorHeight; dy++) {
                int y = startY + dy;
                for (int x = startX - dy; x <= startX + dy; x++) {
                    if (input[x, y] == true) {
                        continue;
                    }

                    if (sand[x, y - 1]) {
                        // Check sand above
                        sand[x, y] = true;
                        count++;
                    } else if (sand[x + 1, y - 1]) {
                        // Check diag right
                        sand[x, y] = true;
                        count++;
                    } else if (sand[x - 1, y - 1]) {
                        // Check diag left
                        sand[x, y] = true;
                        count++;
                    }
                }
            }

            return count;
        }

        private static Point ParsePoint(string s) {
            string[] str = s.Split(",");
            return new Point(int.Parse(str[0]), int.Parse(str[1]));
        }

    }

    internal class Map : IObjectParser<Line[]> {

        public Size Offset { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int InfinityY;
        public int FloorY;
        public int FloorHeight;

        private bool[,] map;

        public void Parse(Line[] input) {
            // Find the smallest bounds
            int minX = 500;
            int maxX = 500;
            int minY = 0;
            int maxY = 0;
            
            foreach(Line line in input) {
                Rectangle bounds = line.Region;
                minX = Math.Min(minX, bounds.Left);
                maxX = Math.Max(maxX, bounds.Right);
                minY = Math.Min(minY, bounds.Top);
                maxY = Math.Max(maxY, bounds.Bottom);
            }

            this.InfinityY = maxY;
            this.FloorY = maxY + 2;
            this.FloorHeight = (FloorY - 0);

            Point floorLeft = new Point(500 - FloorHeight, FloorY);
            Point floorRight = new Point(500 + FloorHeight, FloorY);
            foreach(Point p in new Point[] { floorLeft, floorRight }) {
                minX = Math.Min(minX, p.X);
                maxX = Math.Max(maxX, p.X);
                minY = Math.Min(minY, p.Y);
                maxY = Math.Max(maxY, p.Y);
            }

            this.Offset = new Size(minX, minY);
            this.InfinityY -= Offset.Height;
            this.FloorY -= Offset.Height;
            this.Width = (maxX - minX) + 1;
            this.Height = (maxY - minY) + 1 + 2;
            this.map = new /*State*/bool[Width, Height];

            // Draw lines
            foreach (Line line in input.Append(new Line(floorLeft, floorRight))) {
                foreach (Point p in line.Select(x => x - this.Offset)) {
                    map[p.X, p.Y] = true; //State.Rock;
                }
            }

            
/*
            Console.WriteLine();
            for(int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    Console.Write(map[x, y] ? '#' : '.');
                }
                Console.WriteLine();
            }
*/
        }

        public bool this[int x, int y] {
            get {
                if (x < 0 || x >= Width || y < 0 || y >= Height) {
                    return false;
                } else {
                    return map[x, y];
                }
            }
        }

        public Point? DropSand(bool checkInfinity = true) {
            Point p = new Point(500, 0) - this.Offset;

            while (true) {
                if (this[p.X, p.Y + 1] == false) {
                    // Down
                    p.Y++;
                } else if (this[p.X - 1, p.Y + 1] == false) {
                    // Diag left
                    p.X--;
                    p.Y++;
                } else if (this[p.X + 1, p.Y + 1] == false) {
                    // Diag right
                    p.X++;
                    p.Y++;
                } else {
                    map[p.X, p.Y] = true;
                    return p;
                }

                if (checkInfinity) {
                    if (p.Y >= InfinityY || p.X < 0 || p.X >= Width) {
                        return null;
                    }
                }
            }
        }
    }
    /*
    internal enum State {
        Empty,
        Rock,
        Sand
    }*/

    internal struct Line : IEnumerable<Point> {

        public Point P1;
        public Point P2;

        public Line(Point p1, Point p2) {
            this.P1 = p1;
            this.P2 = p2;
        }

        public Rectangle Region {
            get {
                int minX = Math.Min(P1.X, P2.X);
                int maxX = Math.Max(P1.X, P2.X);
                int minY = Math.Min(P1.Y, P2.Y);
                int maxY = Math.Max(P1.Y, P2.Y);

                return new Rectangle(minX, minY, maxX - minX, maxY - minY);
            }
        }

        public IEnumerator<Point> GetEnumerator() {
            return Enumerate().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Enumerate().GetEnumerator();
        }

        private IEnumerable<Point> Enumerate() {
            int dx = 0;
            int dy = 0;
            if (P1.X > P2.X) {
                dx = -1;
            } else if (P1.X < P2.X) {
                dx = 1;
            }
            if (P1.Y > P2.Y) {
                dy = -1;
            } else if (P1.Y < P2.Y) {
                dy = 1;
            }

            Point p = P1;
            while (p.X != P2.X || p.Y != P2.Y) {
                yield return p;
                p.X += dx;
                p.Y += dy;
            }

            yield return p;
        }
    }
}
