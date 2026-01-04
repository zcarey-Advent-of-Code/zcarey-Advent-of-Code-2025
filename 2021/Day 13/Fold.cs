using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_13 {
    public struct Fold : IObjectParser<Tuple<char, int>> {

        public bool FoldX;
        public int Position;

        public void Parse(Tuple<char, int> input) {
            if(input.Item1 == 'x') {
                FoldX = true;
            }else if(input.Item1 == 'y') {
                FoldX = false;
            } else {
                throw new Exception("Invalid axis.");
            }

            Position = input.Item2;
        }
    }
}
