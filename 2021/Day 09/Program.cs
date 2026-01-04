using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_09 {
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

        protected override object SolvePart1(int[][] input) {
            IEnumerable<int> LowPoints = FindLowPoints(input);
            IEnumerable<int> RiskLevel = LowPoints.Select(x => 1 + x);
            return RiskLevel.Sum();
        }

        protected override object SolvePart2(int[][] input) {
            List<Basin> basins = new();
            Basin[][] basinMap = new Basin[input.Length][];
            for(int y = 0; y < input.Length; y++) {
                basinMap[y] = new Basin[input[y].Length];
            }

            // Let's find those basins!
            for(int y = 0; y < input.Length; y++) {
                for(int x = 0; x < input[y].Length; x++) {
                    int height = input[y][x];
                    if (height < 9) {
                        // We are in a basin! Can we find an existing basin?
                        Basin basin = null;
                        if(y >= 1 && basinMap[y - 1][x] != null) {
                            basin = basinMap[y - 1][x];
                        }
                        if(x >= 1 && basinMap[y][x - 1] != null) {
                            if (basin != null && basin != basinMap[y][x - 1]) {
                                // We need to combine the basins into one.
                                basin = CombineBasins(basin, basinMap[y][x - 1], basinMap, basins);
                            } else {
                                basin = basinMap[y][x - 1];
                            }
                        }
                        
                        if(basin == null){
                            // I guess we have to start a new basin
                            basin = new Basin();
                            basins.Add(basin);
                        }

                        //Increase the size of the found basin and add it to the map
                        basin.Size++;
                        basinMap[y][x] = basin;
                    }
                }
            }
            
            return basins.OrderByDescending(x => x.Size).Take(3).Select(x => x.Size).Aggregate((x, y) => x * y);
        }

        private static IEnumerable<int> FindLowPoints(int[][] map) {
            for(int y = 0; y < map.Length; y++) {
                for(int x = 0; x < map[y].Length; x++) {
                    int height = map[y][x];
                    bool foundLower = false;

                    if (y >= 1) foundLower |= map[y - 1][x] <= height;
                    if (y < map.Length - 1) foundLower |= map[y + 1][x] <= height;
                    if(x >= 1) foundLower |= map[y][x - 1] <= height;
                    if(x < map[y].Length - 1) foundLower |= map[y][x + 1] <= height;

                    if (!foundLower) {
                        yield return height;
                    }
                }
            }
        }

        private static Basin CombineBasins(Basin basin1, Basin basin2, Basin[][] basinMap, List<Basin> basins) {
            Basin newBasin = new Basin();
            newBasin.Size = basin1.Size + basin2.Size;
            basins.Add(newBasin);

            basins.Remove(basin1);
            basins.Remove(basin2);

            for(int y = 0; y < basinMap.Length; y++) {
                for(int x = 0; x < basinMap[y].Length; x++) {
                    Basin basin = basinMap[y][x];
                    if(basin == basin1 || basin == basin2) {
                        basinMap[y][x] = newBasin;
                    }
                }
            }

            return newBasin;
        }

    }
}
