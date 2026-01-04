using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_05 {
    internal class Program : ProgramStructure<Tuple<CrateStacks, CraneOperation[]>> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .Filter(new TextBlockFilter())
            .ToArray()
            .Parse( x => {
                var ops = x[1].Select(x => {
                    var op = new CraneOperation();
                    op.Parse(x);
                    return op;
                }).ToArray();

                var t = new Tuple<CrateStacks, CraneOperation[]>(
                    new CrateStacks(),
                    ops
                );
                t.Item1.Parse(x[0]);
                return t;
            })

        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(Tuple<CrateStacks, CraneOperation[]> input) {
            foreach(CraneOperation op in input.Item2) {
                input.Item1.Apply(op);
            }

            return input.Item1.Stacks.Select(x => x.Peek()).GetString();
        }

        protected override object SolvePart2(Tuple<CrateStacks, CraneOperation[]> input) {
            foreach (CraneOperation op in input.Item2) {
                input.Item1.Apply(op, true);
            }

            return input.Item1.Stacks.Select(x => x.Peek()).GetString();
        }

    }

    struct CrateStacks : IObjectParser<string[]> {

        public List<Stack<char>> Stacks = new();

        public CrateStacks() {
            for (int i = 0; i < 9; i++) {
                Stacks.Add(new Stack<char>());
            }
        }

        public void Apply(CraneOperation op, bool keepOrder = false) {
            Stack<char> source = Stacks[op.From];
            Stack<char> dest = Stacks[op.To];
            if (keepOrder) {
                Stack<char> doubleStack = new(op.Count);
                for (int i = 0; i < op.Count; i++) {
                    doubleStack.Push(source.Pop());
                }
                for (int i = 0; i < op.Count; i++) {
                    dest.Push(doubleStack.Pop());
                }
            } else {
                for (int i = 0; i < op.Count; i++) {
                    dest.Push(source.Pop());
                }
            }
        }

        public void Parse(string[] input) {
            for(int stack = input.Length - 2; stack >= 0; stack--) {
                string line = input[stack];
                if (line[1] == '1')
                    break;

                // Read crates in row
                for(int i = 1; i < line.Length; i += 4) {
                    if (line[i] != ' ')
                        Stacks[(i - 1) / 4].Push(line[i]);
                }
            }
        }
    }

    struct CraneOperation : IObjectParser<string> {

        public int Count;
        public int From;
        public int To;

        public void Parse(string input) {
            input = input.Substring("move ".Length);

            string[] moves = input.Split(" from ");
            this.Count = int.Parse(moves[0]);

            string[] stacks = moves[1].Split(" to ");
            this.From = int.Parse(stacks[0]) - 1; // Minus 1 to account for index starting at 0
            this.To = int.Parse(stacks[1]) - 1; // Minus 1 to account for index starting at 0
        }
    }
}
