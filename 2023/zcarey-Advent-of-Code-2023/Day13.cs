using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace zcarey_Advent_of_Code_2023 {
    internal class Day13 : AdventOfCodeProblem {
        public object Part1(string input) {
            long count = 0;
            foreach(var block in Parse(input)) {
                int hMirror = findHorizonalSymmetry(block.Image);
                if (hMirror >= 0) {
                    count += (hMirror + 1);
                }

                int vMirror = findVerticalSymmetry(block.Image);
                if (vMirror >= 0) {
                    count += (vMirror + 1) * 100;
                }
            }

            return count;
        }

        int findHorizonalSymmetry(string[] image) {
            HashSet<int> lines = new HashSet<int>();
            for (int i = 0; i < image[0].Length - 1; i++) {
                lines.Add(i);
            }

            for (int y = 0; y < image.Length; y++) {
                HashSet<int> copy = new HashSet<int>(lines);
                foreach (int line in copy) {
                    // Check symmetry
                    if (!checkHorizontalSymmetry(image, line, y)) {
                        lines.Remove(line);
                    }
                }
            }

            if (lines.Count > 1) throw new Exception();
            if (lines.Count == 1) {
                return lines.First();
            } else {
                return -1;
            }
        }

        IEnumerable<int> findHorizonalSymmetry(string[] image, int y) {
            for (int line = 0; line < image[0].Length - 1; line++) {
                if (checkHorizontalSymmetry(image, line, y)) {
                    yield return line;
                }
            }
        }

        bool checkHorizontalSymmetry(string[] image, int line, int y) {
            for (int x = line; x >= 0; x--) {
                int xx = line * 2 + 1 - x;
                if (xx >= image[y].Length) {
                    continue;
                }
                if (image[y][x] != image[y][xx]) {
                    return false;
                }
            }

            return true;
        }

        int findVerticalSymmetry(string[] image) {
            HashSet<int> lines = new HashSet<int>();
            for (int i = 0; i < image.Length - 1; i++) {
                lines.Add(i);
            }

            for (int x = 0; x < image[0].Length; x++) {
                HashSet<int> copy = new HashSet<int>(lines);
                foreach (int line in copy) {
                    // Check symmetry
                    if (!checkVerticalSymmetry(image, line, x)) {
                        lines.Remove(line);
                    }
                }
            }

            if (lines.Count > 1) throw new Exception();
            if (lines.Count == 1) {
                return lines.First();
            } else {
                return -1;
            }
        }

        IEnumerable<int> findVerticalSymmetry(string[] image, int x) {
            for (int line = 0; line < image.Length - 1; line++) {
                if (checkVerticalSymmetry(image, line, x)) {
                    yield return line;
                }
            }
        }

        bool checkVerticalSymmetry(string[] image, int line, int x) {
            for (int y = line; y >= 0; y--) {
                int yy = line * 2 + 1 - y;
                if (yy >= image.Length) {
                    continue;
                }
                if (image[y][x] != image[yy][x]) {
                    return false;
                }
            }

            return true;
        }

        public object Part2(string input) {
            long count = 0;
            foreach (var block in Parse(input)) {
                
            }

            return count;
        }

        int findHorizontalSmudge(string[] image) {

        }

        IEnumerable<(string[] Image, int Width, int Height)> Parse(string input) {
            foreach(var block in input.GetBlocks()) {
                string[] image = block.ToArray();
                yield return (image, image[0].Length, image.Length);
            }
        }
    }
}
