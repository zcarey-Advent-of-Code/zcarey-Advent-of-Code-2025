using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_11 {
    public class Monkey : IObjectParser<string[]> {

        public int ID { get; private set; }

        Queue<long> Items;
        Operation WorryOperation;
        public int Test { get; private set; }
        int MonkeyIfTrue;
        int MonkeyIfFalse;


        public void Simulate(Monkey[] monkeys, ref long inspectionCount, Func<long, long> WorryReducer) {
            while (Items.Count > 0) {
                long worryLevel = Items.Dequeue();

                // INSPECT!
                worryLevel = WorryOperation.Calculate(worryLevel);
                inspectionCount++;

                // Phew, the item didn't break!
                worryLevel = WorryReducer(worryLevel);

                // Now what does the monkey do?
                int nextMonkey = ((worryLevel % Test) == 0) ? MonkeyIfTrue : MonkeyIfFalse;
                monkeys[nextMonkey].Items.Enqueue(worryLevel);
            }
        }

        public void Parse(string[] input) {
            if (!input[0].StartsWith("Monkey "))
                throw new ArgumentException("Bad input.");
            this.ID = int.Parse(input[0].Substring("Monkey ".Length).TrimEnd(':'));

            if (!input[1].StartsWith("  Starting items: "))
                throw new ArgumentException("Bad input.");
            Items = new Queue<long>(input[1].Substring("  Starting items: ".Length).Split(", ").Select(long.Parse));

            if (!input[2].StartsWith("  Operation: "))
                throw new ArgumentException("Bad input.");
            WorryOperation = Operation.Parse(input[2].Substring("  Operation: ".Length));

            if (!input[3].StartsWith("  Test: divisible by "))
                throw new ArgumentException("Bad input.");
            Test = int.Parse(input[3].Substring("  Test: divisible by ".Length));

            if (!input[4].StartsWith("    If true: throw to monkey "))
                throw new ArgumentException("Bad input.");
            MonkeyIfTrue = int.Parse(input[4].Substring("    If true: throw to monkey ".Length));

            if (!input[5].StartsWith("    If false: throw to monkey "))
                throw new ArgumentException("Bad input.");
            MonkeyIfFalse = int.Parse(input[5].Substring("    If false: throw to monkey ".Length));
        }
    }
}
