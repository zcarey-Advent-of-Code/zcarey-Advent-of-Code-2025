using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_19 {
    public record Scanner : IObjectParser<IEnumerable<Point>> {

        public Point Location { get; private set; }
        public int Rotation { get; private set; }
        private List<Point> LocalBeacons;

        public Scanner() {
            this.LocalBeacons = new();
        }

        public Scanner(Point location, int rotation, List<Point> localBeacons) {
            this.Location = location;
            this.Rotation = rotation;
            this.LocalBeacons = localBeacons;
        }

        public void Parse(IEnumerable<Point> input) {
            this.Location = new();
            this.Rotation = 0;
            this.LocalBeacons = input.ToList();
        }

        public Scanner Rotate() => new Scanner(Location, Rotation + 1, LocalBeacons);
        
        public Scanner Translate(Point delta) => new Scanner(Location + delta, Rotation, LocalBeacons);

        public IEnumerable<Point> GetGlobalBeacons() {
            Point loc = this.Location;
            int rot = this.Rotation;
            return LocalBeacons.Select(x => x.Transform(loc, rot));
        }

    }

}
