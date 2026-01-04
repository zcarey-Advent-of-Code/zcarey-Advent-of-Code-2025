using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using AdventOfCode.Visualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day_03 {
	class Program : ProgramStructure<Wire[]> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.ForEach(new Parser<string>()
				.Filter(new SeparatedParser(","))
				.FilterCreate<WireDirection>()
				.Create<Wire>()
			)
			.ToArray()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run(args, "Example1.txt");
		}

		private Rectangle Bounds(Rectangle bounds1, Rectangle bounds2) {
			int x1 = Math.Min(bounds1.X, bounds2.X);
			int y1 = Math.Min(bounds1.Y, bounds2.Y);
			int x2 = Math.Max(bounds1.Right, bounds2.Right);
			int y2 = Math.Max(bounds1.Bottom, bounds2.Bottom);

			return new Rectangle(x1, y1, x2 - x1, y2 - y1);
		}

		protected override object SolvePart1(Wire[] input, Visualizer visualizer) {
			if (visualizer != null) {
				Rectangle bounds = Bounds(input[0].Bounds(), input[1].Bounds());
				visualizer.InitializeImage(bounds.Width, bounds.Height);
				visualizer.OffsetX = bounds.X;
				visualizer.OffsetY = bounds.Y;
				visualizer.FlipY = true;
				visualizer.DPI = 70;

				input[0].Visualize(visualizer);
				input[1].Visualize(visualizer);
			}

			int closest = int.MaxValue;
			foreach(Point p in input[0].Intersections(input[1])) {
				if (visualizer != null) {
					visualizer[p] = 'X';
				}
				int dist = Math.Abs(p.X) + Math.Abs(p.Y);
				if(dist != 0 && dist < closest) {
					closest = dist;
				}
			}

			if (visualizer != null) {
				visualizer[0, 0] = 'O';
			}

			//Console.WriteLine(input[0]);
			//Console.WriteLine(input[1]);

			return closest;
		}

		protected override object SolvePart2(Wire[] input, Visualizer visualizer) {
			List<Point> intersections = input[0].Intersections(input[1]).ToList();
			Dictionary<Point, int>[] dicts = new Dictionary<Point, int>[2];
			for(int i = 0; i < 2; i++) {
				Dictionary<Point, int> distances = new Dictionary<Point, int>();
				dicts[i] = distances;
				Wire wire = input[i];

				int dist = 0;
				foreach(Line line in wire.WireLines) {
					foreach(Point intersect in line.Intersection(intersections)) {
                        if (!distances.ContainsKey(intersect)) {
							distances[intersect] = dist + new Line(line.Start, intersect).Distance;
						}
                    }
					dist += line.Distance;
                }
			}

			int bestScore = int.MaxValue;
			foreach(Point intersection in intersections) {
				int wire1Dist = dicts[0][intersection];
				int wire2Dist = dicts[1][intersection];
				int score = wire1Dist + wire2Dist;
				bestScore = Math.Min(bestScore, score);
            }

			return bestScore;
		}

	}
}
