using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_07 {
    internal class Program : ProgramStructure<int[]> {

        Program() : base(new Parser()
            .Parse(new StringReader())
            .Filter(new SeparatedParser(","))
            .Filter(int.Parse)
            .ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(int[] input) {
            int skip = input.Length / 2;
            int median = 0;
            if (input.Length % 2 == 0) {
                median = input.OrderBy(x => x).Skip(skip - 1).Take(2).Sum() / 2;
            } else {
                median = input.OrderBy(x => x).Skip(skip).First();
            }

            // Calculate fuel usage
            return input.Select(x => Math.Abs(x - median)).Sum();
        }

        protected override object SolvePart2(int[] input) {
            int average = input.Sum() / input.Length;
            // Since this is a discrete problem, we need to round depending on the number of crabs....
            // But since this is programming and I am lazy, we are just going to take the smaller of average and average + 1

            Func<int, long> calculateFuel = (int target) => { return input.Select(x => CalculateComplexFuel(Math.Abs(x - target))).Sum(); };
            long floorFuel = calculateFuel(average);
            long ceilFuel = calculateFuel(average + 1);

            return Math.Min(floorFuel, ceilFuel);
        }

        private static long CalculateComplexFuel(long distance) {
            // Calculate 1 + 2 + 3 + 4 + ... + distance
            // y = 0.5x^{2}+0.5x+0
            return (distance * (distance + 1)) / 2;
        }

    }
}
