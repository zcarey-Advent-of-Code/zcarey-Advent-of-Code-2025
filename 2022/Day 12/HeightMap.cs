using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_12 {
    public class HeightMap : IObjectParser<int[][]> {

        public int[][] Map;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Point Start { get; private set; }
        public Point End { get; private set; }

        public IEnumerable<Point> AllIndicies {
            get {
                for (int y = 0; y < Height; y++) {
                    for (int x = 0; x < Width; x++) {
                        yield return new Point(x, y);
                    }
                }
            }
        }

        public int[] this[int index] {
            get => Map[index];
        }

        public void Parse(int[][] input) {
            this.Map = input;
            this.Height = input.Length;
            this.Width = input[0].Length;

            this.Start = new Point(-1, -1);
            this.End = new Point(-1, -1);

            for(int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    if (Map[y][x] == 'S' - 'a') {
                        Start = new Point(x, y);
                        Map[y][x] = -1;
                    } else if (input[y][x] == 'E' - 'a') {
                        End = new Point(x, y);
                        Map[y][x] = ('z' - 'a') + 1;
                    }
                }
            }

            if (Start.X == -1 || End.X == -1) {
                throw new Exception("Could not find start or end location.");
            }
        }
    }
}
