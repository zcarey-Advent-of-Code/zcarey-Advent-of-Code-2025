using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_04 {
    internal class Program : ProgramStructure<Tuple<int[], BingoCard[]>> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .Accumulate(
                // First item, the first line of the input (bingo numbers)
                new Parser<IEnumerable<string>>()
                .Parse(x => x.First()) // Grab the first line
                .Filter(new SeparatedParser(",")) // Split the elements
                .Filter(int.Parse) // Parse into numbers
                .ToArray(),

                // Second item, parsing the bingo cards
                new Parser<IEnumerable<string>>()
                .Parse(x => x.Skip(1))
                .Filter(new TextBlockFilter()) // Get each card
                .ForEach(
                    // Convert arrays of strings into arrays of ints
                    new Parser<string[]>()
                    .ForEach(
                        // Convert one line of the input into an array of ints
                        new Parser<string>()
                        .Filter(x => Regex.Split(x.Trim(), @"\s+")) // Regex required to ignore whitespace around numbers
                        .Filter(int.Parse)
                        .ToArray()
                    )
                )
                .FilterCreate<BingoCard>()
                .ToArray()
            )
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(Tuple<int[], BingoCard[]> input) {
            foreach(int bingoNumber in input.Item1) {
                foreach(BingoCard card in input.Item2) {
                    if (card.CheckForWin(bingoNumber)) {
                        return card.UnmarkedNumbers().Sum() * bingoNumber;
                    }
                }
            }

            return null;
        }

        protected override object SolvePart2(Tuple<int[], BingoCard[]> input) {
            List<BingoCard> cards = new(input.Item2);
            List<BingoCard> winningCards = new List<BingoCard>();

            foreach(int bingoNumber in input.Item1) {
                // Find winning cards for this number
                foreach(BingoCard card in cards) {
                    if(card.CheckForWin(bingoNumber)) {
                        winningCards.Add(card);

                        // Check if we found the answer
                        if (cards.Count == 1) {
                            return bingoNumber * card.UnmarkedNumbers().Sum();
                        }
                    }
                }

                // Remove any winning cards from our list
                foreach(BingoCard card in winningCards) {
                    cards.Remove(card);
                }
                winningCards.Clear();
            }

            return null;
        }

    }
}
