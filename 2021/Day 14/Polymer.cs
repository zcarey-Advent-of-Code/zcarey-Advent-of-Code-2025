using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_14 {
    public class Polymer : IObjectParser<string> {

        public char FirstChar { get; private set; }
        public char LastChar { get; private set; }

        private Dictionary<string, long> Pairs = new();
        public IEnumerable<KeyValuePair<string, long>> AllPairs { get => Pairs; }

        public void Parse(string input) {
            FirstChar = input[0];
            LastChar = input[input.Length - 1];

            for(int i = 0; i < input.Length - 1; i++) {
                string pair = input[i] + "" + input[i + 1];
                if (!Pairs.ContainsKey(pair)) {
                    Pairs[pair] = 1;
                } else {
                    Pairs[pair]++;
                }
            }
        }

        public long this[string pair]{
            get {
                long value;
                if(!Pairs.TryGetValue(pair, out value)) {
                    value = 0;
                }

                return value;
            }

            set {
                Pairs[pair] = value;
            }
        }

    }

}
