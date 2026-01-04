using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_07 {
    internal class Program : ProgramStructure<Directory> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .ForEach(x => x.Split(" "))
            .Create<Directory>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(Directory root) {
            return Part1Recursive(root);
        }

        private long Part1Recursive(Directory search) {
            long sum = 0;
            if (search.Size <= 100000) {
                sum += search.Size;
            }

            foreach(IFileSystem fileSystem in search.Children) {
                if (fileSystem is Directory subdir) {
                    sum += Part1Recursive(subdir);
                }
            }

            return sum;
        }

        protected override object SolvePart2(Directory root) {
            return Part2Recursive(root, 30000000L - (70000000L - root.Size));
        }

        private long Part2Recursive(IFileSystem search, long requiredSpace) {
            long smallestSize = long.MaxValue;
            if (search.Size >= requiredSpace) {
                if (search.Size < smallestSize) {
                    smallestSize = search.Size;
                }
            }

            foreach(IFileSystem fileSystem in search.Children) {
                smallestSize = Math.Min(smallestSize, Part2Recursive(fileSystem, requiredSpace));
            }

            return smallestSize;
        }

    }
}
