using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day02 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            const int maxRed = 12;
            const int maxGreen = 13;
            const int maxBlue = 14;
            long total = 0;
 
            foreach(GameInfo game in ParseInput(input))
            {
                bool possible = true;
                foreach(Handful handful in game.Handfuls)
                {
                    if (handful.Red > maxRed || handful.Green > maxGreen || handful.Blue > maxBlue)
                    {
                        possible = false;
                        break;
                    }
                }

                if (possible)
                {
                    total += game.ID;
                }
            }

            return total;
        }

        public object Part2(string input)
        {
            long total = 0;
            foreach(GameInfo game in ParseInput(input))
            {
                Handful minCubes = new();
                foreach(Handful handful in game.Handfuls)
                {
                    minCubes.Red = Math.Max(minCubes.Red, handful.Red);
                    minCubes.Green = Math.Max(minCubes.Green, handful.Green);
                    minCubes.Blue = Math.Max(minCubes.Blue, handful.Blue);
                }
                total += power(minCubes);
            }

            return total;
        }

        private static long power(Handful cubes)
        {
            return cubes.Red * cubes.Green * cubes.Blue;
        }

        private static IEnumerable<GameInfo> ParseInput(string input)
        {
            const string GamePrefix = "Game ";

            foreach (string line in input.GetLines())
            {
                GameInfo info = new();

                int colon = line.IndexOf(':');
                info.ID = int.Parse(line.Substring(GamePrefix.Length, colon - GamePrefix.Length));

                // First split the game by handfuls
                string[] handfuls = line.Substring(colon + 2).Split("; ");
                foreach(string handful in handfuls)
                {
                    Handful result = new Handful();
                    // Next parse each color of marble in the handful
                    foreach(string color in handful.Split(", "))
                    {
                        string[] args = color.Split(' ');
                        int number = int.Parse(args[0]);
                        if (args[1] == "red")
                        {
                            result.Red = number;
                        } else if (args[1] == "green")
                        {
                            result.Green = number;
                        } else if (args[1] == "blue")
                        {
                            result.Blue = number;
                        } else
                        {
                            throw new Exception("Bad color");
                        }
                    }
                    info.Handfuls.Add(result);
                }

                yield return info;
            }
        }

        private struct GameInfo
        {
            public int ID;
            public List<Handful> Handfuls = new();
            public GameInfo() { }
        }

        private struct Handful
        {
            public int Red;
            public int Green;
            public int Blue;
        }
    }
}
