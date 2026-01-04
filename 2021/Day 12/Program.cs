using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_12 {
    internal class Program : ProgramStructure<Graph> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .Filter(new SeparatedParser("-"))
            .ForEach(
                new Parser<IEnumerable<string>>()
                .ToArray()
                .Parse(x => new Tuple<string, string>(x[0], x[1]))
            )
            .Create<Graph>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(Graph input) {
            return FindPaths(input.StartNode, input.EndNode);
        }

        protected override object SolvePart2(Graph input) {
            return FindPaths2(input.StartNode, input.EndNode);
        }

        public int FindPaths(Cave searchCave, Cave targetCave) {
            if (searchCave == targetCave) return 1; // We found a path!

            int totalPaths = 0;
            searchCave.Traversed = true;
            {
                foreach (Cave cave in searchCave.Connections) {
                    if (!cave.Traversed) {
                        totalPaths += FindPaths(cave, targetCave);
                    }
                }
            }
            searchCave.Traversed = false;

            return totalPaths;
        }

        public int FindPaths2(Cave startCave, Cave endCave) {
            return FindPaths2(startCave, endCave, startCave, false, true);
        }

        public int FindPaths2(Cave searchCave, Cave targetCave, Cave ignoreCave, bool visitedTwice, bool markAsTraversed) {
            if (searchCave == targetCave) return 1; // We found a path!

            int totalPaths = 0;
            if (markAsTraversed) searchCave.Traversed = true;
            {
                foreach (Cave cave in searchCave.Connections.Where(x => x != ignoreCave)) {
                    if (!cave.Traversed) {
                        totalPaths += FindPaths2(cave, targetCave, ignoreCave, visitedTwice, true);
                    } else if (cave.Traversed && !visitedTwice) {
                        totalPaths += FindPaths2(cave, targetCave, ignoreCave, true, false);
                    }
                }
            }
            if (markAsTraversed) searchCave.Traversed = false;

            return totalPaths;
        }

    }
}
