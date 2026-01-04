using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_13 {
    public class FoldInstructions : IObjectParser<Tuple<Point[], Fold[]>> {

        public List<Point> Dots = new();
        public List<Fold> Folds = new();

        public void Parse(Tuple<Point[], Fold[]> input) {
            Dots = new(input.Item1);
            Folds = new(input.Item2);
        }
    }
}
