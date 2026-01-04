using AdventOfCode.Parsing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_08 {
    public class Wires : IObjectParser<string>, IEnumerable<char> {

        private string Data;

        public bool A { get; private set; }
        public bool B { get; private set; }
        public bool C { get; private set; }
        public bool D { get; private set; }
        public bool E { get; private set; }
        public bool F { get; private set; }
        public bool G { get; private set; }

        public int Count { get => Data.Length; }

        public void Parse(string input) {
            Data = input;
            A = input.Contains('a');
            B = input.Contains('b');
            C = input.Contains('c');
            D = input.Contains('d');
            E = input.Contains('e');
            F = input.Contains('f');
            G = input.Contains('g');
        }

        public Wires() { }
        public Wires(string input) {
            this.Parse(input);
        }

        public bool Contains(char c) {
            return Data.Contains(c);
        }

        public bool[] AsArray() {
            return new bool[] { A, B, C, D, E, F, G };
        }

        public int AsInt() {
            return 0
                | (A ? 1 : 0) << 6
                | (B ? 1 : 0) << 5
                | (C ? 1 : 0) << 4
                | (D ? 1 : 0) << 3
                | (E ? 1 : 0) << 2
                | (F ? 1 : 0) << 1
                | (G ? 1 : 0) << 0;
        }

        public IEnumerator<char> GetEnumerator() {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Data.GetEnumerator();
        }
    }
}
