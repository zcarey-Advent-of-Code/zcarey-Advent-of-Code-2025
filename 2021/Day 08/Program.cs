using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_08 {
    internal class Program : ProgramStructure<IEnumerable<Tuple<Wires[], Wires[]>>> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .ForEach(
                new Parser<string>() // For each line
                .Filter(new SeparatedParser(" | "))
                .ToArray()
                .Accumulate(
                    new Parser<string[]>() // Parse input data
                    .Parse(x => x[0])
                    .Filter(new SeparatedParser()) // split string into the individual inputs
                    .FilterCreate<Wires>()
                    .ToArray()
                    ,
                    new Parser<string[]>() // Parse output data
                    .Parse(x =>x [1])
                    .Filter(new SeparatedParser()) // Split string into the individual outputs
                    .FilterCreate<Wires>()
                    .ToArray()
                )
            )
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
            //new Program().Run(args, "LargeExample.txt");
        }

        protected override object SolvePart1(IEnumerable<Tuple<Wires[], Wires[]>> data) {
            // First look for the easy numbers, 1, 4, 7, 8
            int easyDigitCount = 0;
            foreach (var line in data) {
                foreach (Wires output in line.Item2) {
                    if (WireLinks.PossibleNumbersFromLength[output.Count].Length == 1) {
                        easyDigitCount++;
                    }
                }
            }

            return easyDigitCount;
        }

        protected override object SolvePart2(IEnumerable<Tuple<Wires[], Wires[]>> data) {
            int total = 0;
            foreach(var line in data) {
                // Solve for the wire links
                WireLinks links = WireLinks.Solve(line.Item1);
                
                // Calculate what the output value is
                int outputValue = 0;
                foreach(Wires outputDigit in line.Item2) { // Should be most significate sigit first
                    Wires unscrambled = links.Unscramble(outputDigit);
                    int digit = Segment.Decode(unscrambled);
                    outputValue *= 10;
                    outputValue += digit;
                }

                // Add the value to our puzzle answer
                total += outputValue;
            }
            
            return total;
        }

    }
}
