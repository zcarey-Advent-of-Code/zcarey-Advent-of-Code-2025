using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day_12 {
    internal class Program : ProgramStructure<HeightMap> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .ForEach(s => s.Select(c => c - 'a').ToArray())
            .ToArray()
            .Create<HeightMap>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        private static int DistanceFormula((Point p, int h) A, (Point p, int h) B) {
            // Dont forget we are mapping in reverse
            return ((A.h - B.h) <= 1) ? 1 : int.MaxValue;
        }

        protected override object SolvePart1(HeightMap input) {
            //return Dijkstra(input).Count - 1;
            Dijkstra<int> map = Dijkstra<int>.Generate(input.Map, input.End, DistanceFormula);
            return map[input.Start].Distance;
        }

        protected override object SolvePart2(HeightMap input) {
            Dijkstra<int> map = Dijkstra<int>.Generate(input.Map, input.End, DistanceFormula);
            return input.AllIndicies.Where(p => input[p.Y][p.X] == 0).Select(p => map[p].Distance).Min();
        }

    }
}
