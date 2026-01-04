using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_11 {
    internal class Program : ProgramStructure<int[][]> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .ForEach(
                new Parser<string>()
                .ForEach<string, string, char>()
                .Filter(x => x.ToString())
                .Filter(int.Parse)
                .ToArray()
            ).ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(int[][] map) {
            int flashes = 0;
            for(int i = 0; i < 100; i++) {
                flashes += Simulate(map);
            }
            return flashes;
        }

        protected override object SolvePart2(int[][] map) {
            int squidCount = map.Length * map[0].Length;

            for(int step = 1; step < int.MaxValue; step++) { //i.e. infinite loop that increments step for us
                int flashes = Simulate(map);
                if(flashes == squidCount) {
                    return step;
                }
            }

            throw new OverflowException();
        }

        private int Simulate(int[][] map) {
            int flashes = 0;
            bool[][] flashed = new bool[map.Length][];
            for(int y = 0; y < map.Length; y++) {
                flashed[y] = new bool[map[y].Length];
            }

            // Increase everyone's energy level
            for(int y = 0; y < map.Length; y++) {
                for(int x = 0; x < map.Length; x++) {
                    map[y][x]++;
                }
            }

            // Things are about to get bright... (flashing)
            bool keepFlashing = true;
            while (keepFlashing) {
                keepFlashing = false;
                for (int y = 0; y < map.Length; y++) {
                    for (int x = 0; x < map.Length; x++) {
                        if (!flashed[y][x]) {
                            if(map[y][x] >= 10) {
                                flashed[y][x] = true;
                                flashes++;
                                keepFlashing = true;

                                // Energize neighbors
                                int lenX = map[y].Length - 1;
                                int lenY = map.Length - 1;
                                for (int dy = ((y >= 1) ? -1 : 0); dy <= ((y < lenY) ? 1 : 0); dy++) {
                                    for (int dx = ((x >= 1) ? -1 : 0); dx <= ((x < lenX) ? 1 : 0); dx++) {
                                        // Note: We don't really care that we increased our own energy again since we already flashed
                                        map[y + dy][x + dx]++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Anyone that has flashed gets reset to 0
            for (int y = 0; y < map.Length; y++) {
                for (int x = 0; x < map.Length; x++) {
                    if (flashed[y][x]) {
                        map[y][x] = 0;
                    }
                }
            }

            return flashes;
        }

    }
}
