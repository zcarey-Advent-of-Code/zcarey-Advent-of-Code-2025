using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rectangle = System.Drawing.Rectangle;

namespace Day_05 {
    public class Grid : IObjectParser<IEnumerable<Line>> {

        public Line[] Lines = new Line[] { };
        public Dictionary<Point, int> Intersections = new();
        public List<Line> DiagonalLines = new();

        public void Parse(IEnumerable<Line> input) {
            this.Lines = input.ToArray();

            foreach (Line line in this.Lines) {
                if(line.Type == LineType.Diagonal) {
                    DiagonalLines.Add(line);
                }
                foreach (Point point in line.GetPoints()) {
                    if (!Intersections.ContainsKey(point)) {
                        Intersections[point] = 0;
                    }
                    Intersections[point]++;
                }
            }
        }

        public void AddDiagonals() {
            foreach(Line line in DiagonalLines) {
                foreach (Point point in line.GetPoints(true)) {
                    if (!Intersections.ContainsKey(point)) {
                        Intersections[point] = 0;
                    }
                    Intersections[point]++;
                }
            }
        }
    }
}
