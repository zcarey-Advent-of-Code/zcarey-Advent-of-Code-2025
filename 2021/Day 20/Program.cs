using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_20 {
    internal class Program : ProgramStructure<Tuple<bool[], Image>> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .Filter(new TextBlockFilter())
            .Accumulate(
                // Parse the algorithm,
                new Parser<IEnumerable<string[]>>()
                .Parse(x => x.First()) // The first text block
                .Parse(x => x[0]) // There is only one line in this block
                .ForEach() // Get each char of the string
                .Filter(x => x == '#') // Convert to pixels (bool)
                .ToArray()
                ,
                // Parse initial image
                new Parser<IEnumerable<string[]>>()
                .Filter(x => x.Skip(1).First()) // Get the second text block
                .Filter( // Parse each line into a bool array
                    new Parser<string>()
                    .ForEach() // For each char in the string (for this line)
                    .Filter(x => x == '#') // Convert to pixels (bool)
                    .ToArray()
                )
                .ToArray()
                .Create<Image>()
            )
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(Tuple<bool[], Image> input) {
            Image result = input.Item2.Enhance(input.Item1).Enhance(input.Item1);
            Console.WriteLine(result);
            return result.Where(pixel => pixel == true).Count();
        }

        protected override object SolvePart2(Tuple<bool[], Image> input) {
            Image result = input.Item2;
            for(int i = 0; i < 50; i++) {
                result = result.Enhance(input.Item1);
            }
            Console.WriteLine(result);
            return result.Where(pixel => pixel == true).Count();
        }

    }
}
