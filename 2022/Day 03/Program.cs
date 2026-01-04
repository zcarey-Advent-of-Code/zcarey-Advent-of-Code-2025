using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_03 {
    internal class Program : ProgramStructure<IEnumerable<string[]>> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .ForEach(
                x => {
                    return new string[] { x.Substring(0, x.Length / 2), x.Substring(x.Length / 2) };
                }
            )
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(IEnumerable<string[]> input) {
            return input
                .Select( x => x[0].Intersect(x[1]).First())
                .Select(Priority)
                .Sum();
        }

        protected override object SolvePart2(IEnumerable<string[]> input) {
            int sum = 0;

            // For each group of three,
            Dictionary<char, int> items = CreateEmptyDictionary();
            int rucksackCount = 0;
            foreach(string[] rucksack in input) {
                rucksackCount++;

                // Count occurances of all items in the rucksack
                foreach(char item in rucksack[0].Concat(rucksack[1]).Distinct()) {
                    items[item]++;
                }

                if (rucksackCount == 3) {
                    // Found a group of 3 elves, find the item with only a count of 3
                    foreach(KeyValuePair<char, int> pair in items) {
                        if (pair.Value == 3) {
                            // Item found!
                            sum += Priority(pair.Key);
                            break;
                        }
                    }

                    // Reset for next group of 3
                    rucksackCount = 0;
                    items = CreateEmptyDictionary();
                }
            }

            return sum;
        }

        public static int Priority(char c) {
            if (c >= 'a') {
                return (c - 'a') + 1;
            } else {
                return (c - 'A') + 27;
            }
        }

        private static Dictionary<char, int> CreateEmptyDictionary() {
            Dictionary<char, int> items = new();
            for(char c = 'a'; c <= 'z'; c++) {
                items[c] = 0;
            }
            for(char c = 'A'; c <= 'Z'; c++) {
                items[c] = 0;
            }
            return items;
        }

    }
}
