using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Day_15 {
    internal class Program : ProgramStructure<Map> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .ForEach(
                new Parser<string>()
                .Create<Sensor>()
            )
            .Create<Map>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(Map input) {
            int y = 2000000;
            HashSet<Point> locations = new();
            foreach(Sensor sensor in input.Sensors) {
                int dist = sensor.BeaconDistance;
                int dy = Math.Abs(y - sensor.Location.Y);
                if (dy <= dist) {
                    int dx = dist - dy;
                    for(int x = sensor.Location.X - dx; x <= sensor.Location.X + dx; x++) {
                        locations.Add(new Point(x, y));
                    }
                }
            }

            return locations.Count - input.Beacons.Where(p => p.Y == y).Count();
        }

        protected override object SolvePart2(Map input) { 
            for (int y = 0; y <= 4000000; y++) {
                for (int x = 0; x <= 4000000; x++) {
                    bool skip = false;
                    foreach (Sensor sensor in input.Sensors) {
                        if (sensor.BeaconDistance >= sensor.Location.ManhattanDistance(new Point(x, y))) {
                            x = sensor.Location.X + sensor.BeaconDistance - Math.Abs(sensor.Location.Y - y);
                            skip = true;
                            break;
                        }
                    }

                    if (!skip) {
                        return new BigInteger(x) * 4000000 + y;
                    }
                }
            }

            return "Could not find solution.";
        }

    }

    public class Map : IObjectParser<IEnumerable<Sensor>> {

        public List<Sensor> Sensors = new();
        public HashSet<Point> Beacons = new();

        public void Parse(IEnumerable<Sensor> input) {
            foreach(Sensor sensor in input) {
                this.Sensors.Add(sensor);
                this.Beacons.Add(sensor.Beacon);
            }
        }
    }

    public struct Sensor : IObjectParser<string> {

        public Point Location;
        public Point Beacon;
        public int BeaconDistance;

        public void Parse(string input) {
            //Sensor at x=2, y=18: closest beacon is at x=-2, y=15
            if (!input.StartsWith("Sensor at x="))
                throw new ArgumentException();
            input = input.Substring("Sensor at x=".Length);
            int comma = input.IndexOf(',');
            int x = int.Parse(input.Substring(0, comma));
            input = input.Substring(comma + 2);
            if (!input.StartsWith("y="))
                throw new ArgumentException();
            input = input.Substring("y=".Length);
            comma = input.IndexOf(':');
            int y = int.Parse(input.Substring(0, comma));
            this.Location = new Point(x, y);

            input = input.Substring(comma + 2);
            if (!input.StartsWith("closest beacon is at x="))
                throw new ArgumentException();
            input = input.Substring("closest beacon is at x=".Length);
            comma = input.IndexOf(',');
            x = int.Parse(input.Substring(0, comma));
            input = input.Substring(comma + 2);
            if (!input.StartsWith("y="))
                throw new ArgumentException();
            y = int.Parse(input.Substring("y=".Length));
            this.Beacon = new Point(x, y);

            this.BeaconDistance = this.Location.ManhattanDistance(this.Beacon);
        }
    }

}
