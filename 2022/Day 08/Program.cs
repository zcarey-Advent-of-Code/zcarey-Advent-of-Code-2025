using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day_08 {
    internal class Program : ProgramStructure<int[][]> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .ForEach(
                new Parser<string>()
                .Parse(x => x.ToCharArray())
                .ForEach(x => int.Parse(x.ToString()))
                .ToArray()
            ).ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        private static readonly IEnumerable<Size> AdjacentOffsets = new Size[] { new Size(-1, 0), new Size(0, -1), new Size(1, 0), new Size(0, 1) };

        /*        protected override object SolvePart1(int[][] input) {
                    int width = input[0].Length;
                    int height = input.Length;
                    int[,] treeState = new int[width, height]; //x, y
                    //States:
                    //0 = None/null
                    //1 = visible
                    //-1 = not visible

                    // Mark outside trees first for our initial state
                    for (int y = 0; y < height; y++) {
                        for (int x = 0; x < width; x++) {
                            if (x == 0 || x == (width - 1) || y == 0 || y == (height - 1)) {
                                treeState[x, y] = 1;
                            }
                        }
                    }

                    // repeat calculation as needed
                    bool changesMade = true;
                    bool stateSolved = false;
                    while (changesMade && !stateSolved) {
                        changesMade = false;
                        stateSolved = true;

                        for(int y = 1; y < height - 1; y++) {
                            for(int x = 1; x < width - 1; x++) {
                                if (treeState[x, y] == 0) {
                                    int treeHeight = input[y][x];
                                    bool stateKnows = true;
                                    // Check surrounding trees
                                    foreach (Point neighbor in AdjacentOffsets.Select(o => new Point(x, y) + o)) {
                                        int neighborHeight = input[neighbor.Y][neighbor.X];
                                        if (neighborHeight >= treeHeight) {
                                            // Neighbor hides tree
                                        } else {
                                            // Neighbor does not hide trr
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return input;
                }*/

        protected override object SolvePart1(int[][] input) {
            int width = input[0].Length;
            int height = input.Length;
            bool[,] treeVisible = new bool[width, height]; //x, y

            // Mark outside trees first for our initial state
            /*            for (int y = 0; y < height; y++) {
                            for (int x = 0; x < width; x++) {
                                if (x == 0 || x == (width - 1) || y == 0 || y == (height - 1)) {
                                    treeState[x, y] = 1;
                                }
                            }
                        }
            */

            for(int y = 0; y < height; y++) {
                // Check left-to-right
                int tallestTree = -1;
                for (int x = 0; x < width; x++) {
                    int treeHeight = input[y][x];
                    if (treeHeight > tallestTree) {
                        tallestTree = treeHeight;
                        treeVisible[x, y] = true;
                    }
                }

                // Check right-to-left
                tallestTree = -1;
                for(int x = width - 1; x >= 0; x--) {
                    int treeHeight = input[y][x];
                    if (treeHeight > tallestTree) {
                        tallestTree = treeHeight;
                        treeVisible[x, y] = true;
                    }
                }
            }

            for(int x = 0; x < width; x++) {
                // Check top-to-bottom
                int tallestTree = -1;
                for(int y = 0; y < height; y++) {
                    int treeHeight = input[y][x];
                    if (treeHeight > tallestTree) {
                        tallestTree = treeHeight;
                        treeVisible[x, y] = true;
                    }
                }

                // Check bottom-to-top
                tallestTree = -1;
                for(int y = height - 1; y >= 0; y--) {
                    int treeHeight = input[y][x];
                    if(treeHeight > tallestTree) {
                        tallestTree = treeHeight;
                        treeVisible[x, y] = true;
                    }
                }
            }

            // Count number of visible trees
            return treeVisible.Values().Where(x => x == true).Count();
        }

        protected override object SolvePart2(int[][] input) {
            int width = input[0].Length;
            int height = input.Length;
            int[,] scenicScore = new int[width, height];

            for(int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    scenicScore[x, y] = calculateScenicScore(input, width, height, x, y);
                }
            }

            return scenicScore.Values().Max();
        }

        private static int calculateScenicScore(int[][] input, int width, int height, int treeX, int treeY) {
            int treeHeight = input[treeY][treeX];
            int scenicScore = 1;
            int distance;

            // Check left
            distance = 0;
            for(int x = treeX - 1; x >= 0; x--) {
                distance++;
                if (input[treeY][x] >= treeHeight) {
                    break;
                }
            }
            scenicScore *= distance;
            if (scenicScore == 0)
                return 0;

            // Check right
            distance = 0;
            for (int x = treeX + 1; x < width; x++) {
                distance++;
                if (input[treeY][x] >= treeHeight) {
                    break;
                }
            }
            scenicScore *= distance;
            if (scenicScore == 0)
                return 0;

            // Check up
            distance = 0;
            for (int y = treeY - 1; y >= 0; y--) {
                distance++;
                if (input[y][treeX] >= treeHeight) {
                    break;
                }
            }
            scenicScore *= distance;
            if (scenicScore == 0)
                return 0;

            // Check down
            distance = 0;
            for (int y = treeY + 1; y < height; y++) {
                distance++;
                if (input[y][treeX] >= treeHeight) {
                    break;
                }
            }
            scenicScore *= distance;

            return scenicScore;
        }

    }
}
