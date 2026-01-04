using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day_15 {
    internal class Program : ProgramStructure<int[][]> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .ForEach(
                new Parser<string>()
                .ForEach<string, string, char>()
                .Filter(x => x.ToString())
                .Filter(int.Parse)
                .ToArray()
            )
            .ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(int[][] input) {
            int y = input.Length;
            int x = input[y - 1].Length;
            Point target = new(x - 1, y - 1);

            IEnumerable<Point> path = FindSafestPath(input, new Point(0, 0), target, false);
            // Skip the start position danger
            int danger = path.Skip(1).Select(p => GetDangerLevel(input, p, false)).Sum();
            return danger;
        }

        protected override object SolvePart2(int[][] input) {
            int y = input.Length;
            int x = input[y - 1].Length;
            Point target = new(x * 5 - 1, y * 5 - 1);
            
            IEnumerable<Point> path = FindSafestPath(input, new Point(0, 0), target, true);
            // Skip the start position danger
            int danger = path.Skip(1).Select(p => GetDangerLevel(input, p, true)).Sum();
            return danger;
        }

        private static readonly Size[] Directions = new Size[] { 
            // Always check right then down first to find a path quickly and avoid overflow errors
            new Size(1, 0),
            new Size(0, 1),
            new Size(0, -1),
            new Size(-1, 0)
        };

        private static IEnumerable<Point> FindSafestPath(int[][] input, Point start, Point target, bool adjustCave) {
            // As always, my favorite website for pathing (dijkstra):
            // https://www.redblobgames.com/pathfinding/a-star/introduction.html#dijkstra
            PriorityQueue<Point, int> frontier = new();
            Dictionary<Point, Point?> cameFrom = new();
            Dictionary<Point, int> costSoFar = new();

            frontier.Enqueue(start, 0);
            cameFrom[start] = null;
            costSoFar[start] = 0;

            while(frontier.Count > 0) {
                Point current = frontier.Dequeue();
                if(current == target) {
                    break;
                }

                foreach(Size direction in Directions) {
                    Point next = current + direction;
                    int danger = GetDangerLevel(input, next, adjustCave);
                    if(danger < 0) { 
                        // Invalid position
                        continue;
                    }

                    int newCost = costSoFar[current] + danger;
                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
                        costSoFar[next] = newCost;
                        int priority = newCost; // Semantics, am I right?
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            List<Point> reversePath = new List<Point>();
            reversePath.Add(target);
            Point trace = target;
            Point? nextTrace;
            while (cameFrom.TryGetValue(trace, out nextTrace) && nextTrace != null) {
                trace = (Point)nextTrace;
                reversePath.Add(trace);
            }
            if (trace != start) throw new Exception("Failed to trace the path.");

            // Flip the list and return the path
            for(int i = reversePath.Count - 1; i >= 0; i--) {
                yield return reversePath[i];
            }
        }

        // Returns -1 if not a valid position
        private static int GetDangerLevel(int[][] input, Point pos, bool adjustCave) {
            if (!adjustCave) {
                // Just do regular checks
                if (pos.X < 0 || pos.Y < 0 || pos.Y >= input.Length || pos.X >= input[pos.Y].Length) {
                    return -1;
                }

                return input[pos.Y][pos.X];
            } else {
                // This cave gets funky
                int limitY = input.Length * 5;
                if (pos.Y < 0 || pos.Y >= limitY) return -1;
                int y = pos.Y % input.Length;

                int limitX = input[y].Length * 5;
                if (pos.X < 0 || pos.X >= limitX) return -1;
                int x = pos.X % input[y].Length;

                // Calculate danger adjustment
                int chunkX = pos.X / input[y].Length;
                int chunkY = pos.Y / input.Length;
                int dangerAdjustment = chunkX + chunkY;

                int danger = input[y][x] + dangerAdjustment;
                return ((danger - 1) % 9) + 1; // The minus one and plus one is to deal with the wrap around, since mod wraps back to 0
            }
        }


    }
}
