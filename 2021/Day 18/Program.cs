using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_18 {
    internal class Program : ProgramStructure<SnailfishNumber[]> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .FilterCreate<SnailfishNumber>()
            .ToArray()
        ) { }

        static void Main(string[] args) {
            //new Program().Run(args);
            //new Program().Run(args, "Example1.txt");
            //new Program().Run(args, "Example2.txt");
            new Program().Run(args, "Example3.txt");
        }

        protected override object SolvePart1(SnailfishNumber[] input) {
            //SnailfishNumber result = input.Aggregate((x, y) => x + y);
            SnailfishNumber result = input[0];
            foreach(SnailfishNumber n in input.Skip(1)) {
                string a = result.ToString();
                string b = n.ToString();
                result += n;
                Console.WriteLine("{0} + {1} = {2}", a, b, result);
            }
            //input[0].ExplodeAll(0);
            return result;
        }

        protected override object SolvePart2(SnailfishNumber[] input) {
            return null;
        }

    }
}
