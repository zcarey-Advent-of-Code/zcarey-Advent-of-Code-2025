using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_14 {
    internal class Program : ProgramStructure<Tuple<Polymer, InsertionRule[]>> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .Accumulate(
                // Parse the first line
                new Parser<IEnumerable<string>>()
                .Parse(x => x.First())
                .Create<Polymer>()
                ,
                // Parse the rules
                new Parser<IEnumerable<string>>()
                .Filter(x => x.Skip(2)) // Skip the first 2 lines to get to the input
                .ForEach(
                    // Split the string into 2 strings for each rule
                    new Parser<string>()
                    .Parse(new SeparatedParser("->"))
                    .ForEach(x => x.Trim())
                    .ToArray()
                )
                .FilterCreate<InsertionRule>()
                .ToArray()
            )
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(Tuple<Polymer, InsertionRule[]> input) {
            for(int i = 0; i < 10; i++) {
                ApplyStep(input.Item1, input.Item2);
            }

            var ElementCount = CountElements(input.Item1);

            return ElementCount.MostCommon - ElementCount.LeastCommon;
        }

        protected override object SolvePart2(Tuple<Polymer, InsertionRule[]> input) {
            for (int i = 0; i < 40; i++) {
                ApplyStep(input.Item1, input.Item2);
                //Console.WriteLine("Step {0} completed.", i); // Just so I know it's still running T_T
            }

            var ElementCount = CountElements(input.Item1);

            return ElementCount.MostCommon - ElementCount.LeastCommon;
        }

        private void ApplyStep(Polymer polymer, InsertionRule[] rules) {
            Dictionary<string, long> newPairs = new();
            foreach(InsertionRule rule in rules) {
                long matches = polymer[rule.Rule];
                if (matches > 0) {
                    if (!newPairs.ContainsKey(rule.Rule)) newPairs[rule.Rule] = 0;
                    if (!newPairs.ContainsKey(rule.Pair1)) newPairs[rule.Pair1] = 0;
                    if (!newPairs.ContainsKey(rule.Pair2)) newPairs[rule.Pair2] = 0;

                    newPairs[rule.Rule] += -matches; // We want the old pair to get removed.
                    newPairs[rule.Pair1] += matches;
                    newPairs[rule.Pair2] += matches;
                }
            }

            foreach(var pair in newPairs) {
                polymer[pair.Key] += pair.Value;
            }
        }

        // Returns the count of the least common element followed by the count of the most common element.
        private (long LeastCommon, long MostCommon) CountElements(Polymer input) {

            // Count the number of each element
            Dictionary<char, long> elementCount = new();

            // Add the first and last element again, since they don't get two pairs like the other elements
            if (input.FirstChar == input.LastChar) {
                elementCount[input.FirstChar] = 2;
            } else {
                elementCount[input.FirstChar] = 1;
                elementCount[input.LastChar] = 1;
            }

            // Add up all the other pairs
            foreach (var pair in input.AllPairs) {
                char element1 = pair.Key[0];
                if (!elementCount.ContainsKey(element1)) {
                    elementCount[element1] = 0;
                }
                elementCount[element1] += pair.Value;

                char element2 = pair.Key[1];
                if (!elementCount.ContainsKey(element2)) {
                    elementCount[element2] = 0;
                }
                elementCount[element2] += pair.Value;
            }

            // Find the most and least common element
            long mostCommonCount = long.MinValue;
            long leastCommonCount = long.MaxValue;
            foreach (var pair in elementCount) {
                mostCommonCount = Math.Max(mostCommonCount, pair.Value);
                leastCommonCount = Math.Min(leastCommonCount, pair.Value);
            }

            return (leastCommonCount / 2, mostCommonCount / 2);
        }

    }
}
