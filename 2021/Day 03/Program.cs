using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_03 {
    internal class Program : ProgramStructure<string[]> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(string[] input) {
            int[] numberOfOnes = new int[input[0].Length];
            int half = input.Length / 2;

            foreach(string binary in input) {
                for (int i = 0; i < binary.Length; i++) {
                    if (binary[i] == '1') {
                        numberOfOnes[i]++;
                    }
                }
            }

            int gamma = 0;
            int epsilon = 0;
            for(int i = 0; i < numberOfOnes.Length; i++) {
                int bit = (numberOfOnes[i] > half) ? 1 : 0;
                gamma = (gamma << 1) | bit;
                epsilon = (epsilon << 1) | (1 - bit);
            }

            return gamma * epsilon;
        }

        protected override object SolvePart2(string[] input) {
            Func<bool, bool, bool, bool> oxygenBitFilter = (bool ZeroMostCommon, bool OneMostCommon, bool bit) => {
                if (ZeroMostCommon && OneMostCommon) {
                    // If both most common, keep only 1 bits
                    return bit;
                } else {
                    // Only keep "most common" bits
                    return (ZeroMostCommon && !bit) || (OneMostCommon && bit);
                }
            };

            Func<bool, bool, bool, bool> scrubberBitFilter = (bool ZeroMostCommon, bool OneMostCommon, bool bit) => {
                if (ZeroMostCommon && OneMostCommon) {
                    // If both most common, keep only 0 bits
                    return !bit;
                } else {
                    // Only keep "least common" bits
                    return (!ZeroMostCommon && !bit) || (!OneMostCommon && bit);
                }
            };

            long oxygenRating = Convert.ToInt64(FilterRating(input, 0, oxygenBitFilter), 2);
            long scrubberRating = Convert.ToInt64(FilterRating(input, 0, scrubberBitFilter), 2);

            Console.WriteLine("Oxygen rating: {0}", oxygenRating);
            Console.WriteLine("Scrubber rating: {0}", scrubberRating);

            return oxygenRating * scrubberRating;
        }

        // In case of ties, both values will return true.
        private (bool ZeroMostCommon, bool OneMostCommon) FindMostCommonBit(IEnumerable<string> values, int bitPosition) {
            int total = values.Count(); // Note: We know that the IEnumerable is always a list<> so the Count() method is optimized
            int numberOfZeros = values.Where(x => x[bitPosition] == '0').Count();
            int numberOfOnes = total - numberOfZeros;

            return (numberOfZeros >= numberOfOnes, numberOfOnes >= numberOfZeros);
        }

        private string FilterRating(IEnumerable<string> input, int bitPosition, Func<bool, bool, bool, bool> filter) {
            var mostCommonBit = FindMostCommonBit(input, bitPosition);
            List<string> results = input.Where(x => filter(mostCommonBit.ZeroMostCommon, mostCommonBit.OneMostCommon, x[bitPosition] == '1')).ToList();

            if(results.Count > 1) {
                return FilterRating(results, bitPosition + 1, filter);
            } else {
                return results[0];
            }
        }

    }
}
