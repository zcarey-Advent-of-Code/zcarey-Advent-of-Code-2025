using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day_09 {
    internal class Program : ProgramStructure<IEnumerable<Direction>> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .ForEach(x => x.Split(" "))
            .FilterCreate<Direction>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(IEnumerable<Direction> input) {
            HashSet<Point> positions = new();
            Point head = new();
            Point tail = new();
            positions.Add(tail);

            foreach(Direction dir in input) {
                for(int i = 0; i < dir.Count; i++) {
                    head += dir;
                    Simulate(head, ref tail);
                    positions.Add(tail);
                }
            }

            return positions.Count;
        }

        private static void Simulate(Point head, ref Point tail) {
            int dx = (head.X - tail.X);
            int absX = Math.Abs(dx);
            int dy = (head.Y - tail.Y);
            int absY = Math.Abs(dy);

            if (dx == 0 && dy != 0) {
                // Check vertical movements
                if (absY > 1) {
                    tail.Y += (dy / absY); ;
                }
            } else if (dy == 0 && dx != 0) {
                // Check horizontal movements
                if (absX > 1) {
                    tail.X += (dx / absX);
                }
            } else if (absX > 1 || absY > 1) {
                // Check diagonal movement
                if (dx != 0) {
                    tail.X += (dx / absX);
                }
                if (dy != 0) {
                    tail.Y += (dy / absY);
                }
            }
            // Otherwise, no movement is needed!
        }

        protected override object SolvePart2(IEnumerable<Direction> input) {
            HashSet<Point> positions = new();
            Point[] knots = new Point[10];
            positions.Add(knots[9]); // Track the tail!

            foreach (Direction dir in input) {
                for (int i = 0; i < dir.Count; i++) {
                    knots[0] += dir;
                    //Simulate all the knots
                    for (int knot = 1; knot < knots.Length; knot++) {
                        Simulate(knots[knot - 1], ref knots[knot]);
                    }
                    positions.Add(knots[9]); // Track the tail!
                }
            }

            return positions.Count;
        }

    }
}
