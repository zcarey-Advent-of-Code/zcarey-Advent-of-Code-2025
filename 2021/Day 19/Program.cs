using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_19 {
    internal class Program : ProgramStructure<Scanner[]> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .Filter(new TextBlockFilter())
            .ForEach(
                // Parse each scanner
                new Parser<string[]>()
                .Filter(x => x.Skip(1)) // Skip the "ID" line
                .Filter(Point.Parse)
                .Create<Scanner>()
            )
            .ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(Scanner[] input) {
            return LocateScanners(input)
                .SelectMany(scanner => scanner.GetGlobalBeacons())
                .Distinct()
                .Count();
        }

        protected override object SolvePart2(Scanner[] input) {
            HashSet<Scanner> scanners = LocateScanners(input);
            return scanners.SelectMany(x =>
                    scanners.Where(y => x != y)
                    .Select(y => (x.Location - y.Location).Abs.Sum())
                )
                .Max();
        }

        private HashSet<Scanner> LocateScanners(Scanner[] input) {
            HashSet<Scanner> remainingScanners = new(input);
            HashSet<Scanner> locatedScanners = new();
            Queue<Scanner> searchQueue = new(); // Located scanners that need to search for neighbors

            // Take any scanner as the "root", treating it as the origin
            // It's all relative anyways
            Scanner root = remainingScanners.First();
            locatedScanners.Add(root);
            searchQueue.Enqueue(root);
            remainingScanners.Remove(root);

            while (searchQueue.Count > 0) {
                Scanner source = searchQueue.Dequeue(); // Located scanner that we are using to find another scanner
                foreach (Scanner scanner in remainingScanners.ToArray()) { // We use ToArray to we can modify the HashSet while iterating
                    // Try to locate the scanner as a neighbor to the located scanner
                    Scanner locatedScanner = TryToLocate(source, scanner);
                    if (locatedScanner != null) {
                        // Sweet, we found a neighbor!
                        locatedScanners.Add(locatedScanner);
                        searchQueue.Enqueue(locatedScanner);
                        remainingScanners.Remove(scanner);
                    }
                }
            }

            return locatedScanners;
        }

        private Scanner TryToLocate(Scanner source, Scanner neighbor) {
            IEnumerable<Point> sourceBeacons = source.GetGlobalBeacons();

            foreach (var (sourceBeacon, neighborBeacon) in PotentialMatchingBeacons(source, neighbor)) {
                // See if the neighbor can be rotated to match the source
                Scanner rotatedScanner = neighbor;
                for (int rotation = 0; rotation < 24; rotation++, rotatedScanner = rotatedScanner.Rotate()) {
                    // Moving the rotated scanner so that source and neighbor overlaps.
                    Point rotatedBeacon = neighborBeacon.Transform(rotatedScanner.Location, rotation);
                    Scanner locatedNeighbor = rotatedScanner.Translate(sourceBeacon - rotatedBeacon);

                    // 12 matching beacons means they are neighbors!
                    if (locatedNeighbor.GetGlobalBeacons().Intersect(sourceBeacons).Count() >= 12) {
                        return locatedNeighbor;
                    }
                }
            }

            return null;
        }

        IEnumerable<(Point sourceBeacon, Point neighborBeacon)> PotentialMatchingBeacons(Scanner source, Scanner neighbor) {
            // If we had a maching pair of beacons and move the centers of their respective scanners to this location
            // (the beacons should have the same global location) thwn there should be at least 12 matching beacons

            // The only problem is that the rotation of the neighbor scanner is not fixed yet.
            // We need to make our check invariant to that:

            // After the translation, we could form a set from each scanner 
            // taking the absolute values of the x y and z coordinates of their beacons 
            // and compare those. 

            IEnumerable<int> absCoordinates(Scanner scanner) => scanner.GetGlobalBeacons().SelectMany(x => x.Abs);

            // This is the same no matter how we rotate the neighbor scanner, so the two sets should 
            // have at least 3 * 12 common values.
            foreach (Point sourceBeacon in source.GetGlobalBeacons()) {
                HashSet<int> absSource = absCoordinates(
                    source.Translate(-sourceBeacon)
                ).ToHashSet();

                foreach (var neighborBeacon in neighbor.GetGlobalBeacons()) {
                    IEnumerable<int> absNeighbor = absCoordinates(
                        neighbor.Translate(-neighborBeacon)
                    );

                    if (absNeighbor.Count(d => absSource.Contains(d)) >= 3 * 12) {
                        yield return (sourceBeacon, neighborBeacon);
                    }
                }
            }
        }

    }
 
}
