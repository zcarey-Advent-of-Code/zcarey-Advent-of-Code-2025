using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_08 {
    public static class Segment {

        // Mostly used for finding wire links
        public static Dictionary<int, IEnumerable<char>> Segments = new() {
            { 0, "abcefg" },
            { 1, "cf" },
            { 2, "acdeg" },
            { 3, "acdfg" },
            { 4, "bcdf" },
            { 5, "abdfg" },
            { 6, "abdefg" },
            { 7, "acf" },
            { 8, "abcdefg" },
            { 9, "abcdfg" }
        };

        // Mostly used for finding wire links
        public static Dictionary<int, IEnumerable<char>> InverseSegments = new() {
            { 0, "d" },
            { 1, "abdeg" },
            { 2, "bf" },
            { 3, "be" },
            { 4, "aeg" },
            { 5, "ce" },
            { 6, "c" },
            { 7, "bdeg" },
            { 8, "" },
            { 9, "e" }
        };

        // A is left most bit, g is right most bit, so
        // 0b1100000 means A=1, B=1, rest are 0
        private static Dictionary<int, int> decoder = new() {
            { 0b1110111, 0 },
            { 0b0010010, 1 },
            { 0b1011101, 2 },
            { 0b1011011, 3 },
            { 0b0111010, 4 },
            { 0b1101011, 5 },
            { 0b1101111, 6 },
            { 0b1010010, 7 },
            { 0b1111111, 8 },
            { 0b1111011, 9 }
        };

        public static int Decode(int inputs) {
            int output;
            if (decoder.TryGetValue(inputs, out output)) {
                return output;
            } else {
                throw new Exception("Not a valid sigit.");
            }
        }

        /// <summary>
        /// Inputs = [A, B, C, D, E, F, G]
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static int Decode (bool[] inputs) {
            if(inputs.Length != 7) {
                throw new ArgumentException("Array must contain exactly 7 elements.", nameof(inputs));
            }

            int value = 0;
            foreach(bool bit in inputs) {
                value <<= 1;
                if (bit) {
                    value |= 1;
                }
            }

            return Decode(value);
        }

        public static int Decode(bool A, bool B, bool C, bool D, bool E, bool F, bool G) {
            return Decode(new bool[] { A, B, C, D, E, F, G });
        }

        public static int Decode(Wires wires) {
            return Decode(wires.AsInt());
        }

    }
}
