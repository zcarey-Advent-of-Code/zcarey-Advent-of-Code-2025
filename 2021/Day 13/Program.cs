using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day_13 {
    internal class Program : ProgramStructure<FoldInstructions> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .Filter(new TextBlockFilter()) // Separate input into dots and folding instruction blocks
            .ToArray()
            .Accumulate(
                // Parse dots
                new Parser<string[][]>()
                .Parse(x => x[0]) // Select dots input block 
                .ForEach(new PointParser()) // Parse each line into a point
                .ToArray()
                ,
                // Parse folding instructions
                new Parser<string[][]>()
                .Parse(x => x[1]) // Select folding instruction block
                .ForEach(
                    // Parse each line into a fold
                    new Parser<string>()
                    .Filter(new SeparatedParser("=")) // Find fold data (e.x. "x=7")
                    .ToArray()
                    .Accumulate(
                        // Get the axis (e.x. "x")
                        new Parser<string[]>()
                        .Parse(x => x[0]) // Select the first string from the separation e.x. "fold along x"
                        .Parse(x => x[x.Length - 1]) // Get the last char from that string, which should be the axis
                        ,
                        // Get the position e.x. "7" in "x=7"
                        new Parser<string[]>()
                        .Parse(x => x[1]) // Select the second string from the separation
                        .Parse(int.Parse) 
                    )
                    .Create<Fold>()
                )
                .ToArray()
            )
            .Create<FoldInstructions>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(FoldInstructions input) {
            HashSet<Point> paper = new();
            foreach(Point dot in input.Dots) {
                paper.Add(dot);
            }

            Fold firstFold = input.Folds.First();
            if (firstFold.FoldX)
                FoldX(paper, firstFold.Position);
            else
                FoldY(paper, firstFold.Position);

            return paper.Count;
        }

        protected override object SolvePart2(FoldInstructions input) {
            HashSet<Point> paper = new();
            foreach (Point dot in input.Dots) {
                paper.Add(dot);
            }

            foreach (Fold fold in input.Folds) {
                if (fold.FoldX)
                    FoldX(paper, fold.Position);
                else
                    FoldY(paper, fold.Position);
            }

            // Print out the points
            Size size = GetBounds(paper);
            bool[,] output = new bool[size.Width + 1, size.Height + 1];
            foreach (Point dot in paper) {
                output[dot.X, dot.Y] = true;
            }
            for(int y = 0; y <= size.Height; y++) {
                for(int x = 0; x <= size.Width; x++) {
                    Console.Write(output[x, y] ? '#' : '.');
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            return paper.Count;
        }

        private static void FoldX(HashSet<Point> paper, int x) {
            List<Point> dots = paper.Where(p => p.X >= x).ToList();
            // Remove these points from the paper then add them back in their folded position
            foreach (Point p in dots) {
                paper.Remove(p);
                int distFromFold = p.X - x;
                paper.Add(new Point(x - distFromFold, p.Y));
            }
        }

        private static void FoldY(HashSet<Point> paper, int y) {
            List<Point> dots = paper.Where(p => p.Y >= y).ToList();
            // Remove these points from the paper then add them back in their folded position
            foreach (Point p in dots) {
                paper.Remove(p);
                int distFromFold = p.Y - y;
                paper.Add(new Point(p.X, y - distFromFold));
            }
        }

        private static Size GetBounds(HashSet<Point> paper) {
            int boundsX = 0;
            int boundsY = 0;

            foreach (Point p in paper) {
                boundsX = Math.Max(boundsX, p.X);
                boundsY = Math.Max(boundsY, p.Y);
            }

            return new Size(boundsX, boundsY);
        }

    }
}
