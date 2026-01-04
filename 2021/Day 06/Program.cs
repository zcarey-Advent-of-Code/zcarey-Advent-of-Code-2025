using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_06 {
    internal class Program : ProgramStructure<IEnumerable<int>> {

        Program() : base(new Parser()
            .Parse(new StringReader())
            .Filter(new SeparatedParser(","))
            .Filter(int.Parse)
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(IEnumerable<int> input) {
            return FishySimulator(input, 80);
        }

        protected override object SolvePart2(IEnumerable<int> input) {
            return FishySimulator(input, 256);
        }

        private long FishySimulator(IEnumerable<int> input, int days) {
            Dictionary<int, long> fishes = new();
            Dictionary<int, long> nextDay = new();
            foreach (int fish in input) {
                if (fishes.ContainsKey(fish)) {
                    fishes[fish]++;
                } else {
                    fishes[fish] = 1;
                }
            }

            int spawnTime = 7;
            int matureTime = 2;
            for (int day = 0; day < days; day++) {
                foreach (KeyValuePair<int, long> fish in fishes) {
                    int fishClock = fish.Key;
                    long fishCount = fish.Value;

                    if (fishClock == 0) {
                        // Subtract 1 from clocks to account of "off-by-one" error
                        fishClock = spawnTime - 1;
                        nextDay[spawnTime + matureTime - 1] = fishCount;
                    } else {
                        fishClock--;
                    }

                    if (!nextDay.ContainsKey(fishClock)) {
                        nextDay[fishClock] = 0;
                    }
                    nextDay[fishClock] += fishCount;
                }

                var temp = fishes;
                fishes = nextDay;
                nextDay = temp;
                nextDay.Clear();
            }

            return fishes.Select(x => x.Value).Sum();
        }

    }
}
