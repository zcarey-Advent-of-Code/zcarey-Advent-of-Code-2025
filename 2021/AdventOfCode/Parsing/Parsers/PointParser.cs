using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Parsing {
    public class PointParser : IParser<string, Point> {

        private readonly string separator;

        public PointParser(string separator = ",") {
            this.separator = separator;
        }

        internal override Point Parse(string input) {
            if (input == null) throw new ArgumentNullException(nameof(input), "Can't parse Point from null string.");

            string[] points = input.Split(separator);
            if (points.Length != 2) throw new FormatException("Unable to parse Point from string.");

            int x;
            if(!int.TryParse(points[0].Trim(), out x)) {
                throw new FormatException("Unable to parse Point from string.");
            }

            int y;
            if(!int.TryParse(points[1].Trim(), out y)) {
                throw new FormatException("Unable to parse Point from string.");
            }

            return new Point(x, y);
        }
    }
}
