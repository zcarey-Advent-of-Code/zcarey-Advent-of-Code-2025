using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day04 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            return ParseInput(input)
                .Select(x => x.Left.Intersect(x.Right).Count()) // Count the number of matching numbers on the card
                .Select(Score) // Get the score with the given number of matches
                .Sum();
        }

        public object Part2(string input)
        {
            Dictionary<int, int> CardCounts = new();
            foreach(Card card in ParseInput(input))
            {
                CardCounts.Increment(card.ID, 1);

                int numberOfCards = CardCounts[card.ID]; // How many of this card ID we have
                int matching = card.Left.Intersect(card.Right).Count();
                for(int i = card.ID + 1; i <= card.ID + matching; i++)
                {
                    CardCounts.Increment(i, numberOfCards);
                }
            }

            return CardCounts.Values.Sum(); // Cound the number of cards we have
        }

        static long Score(int matching)
        {
            if (matching == 0)
            {
                return 0;
            }
            else
            {
                return 1L << (matching - 1);
            }
        }

        struct Card
        {
            public int ID;
            public List<int> Left = new();
            public List<int> Right = new();
            public Card() { }
        }

        static IEnumerable<Card> ParseInput(string input)
        {
            foreach(string line in input.GetLines())
            {
                Card card = new();
                const string prefix = "Card ";
                int colon = input.IndexOf(':');
                card.ID = int.Parse(line.Substring(prefix.Length, colon - prefix.Length));

                int bar = input.IndexOf('|');
                card.Left.AddRange(line
                    .Substring(colon + 1, bar - colon - 1) // Get left list of numbers
                    .Split() // Split numbers up to parse separate
                    .Where(x=> x.Length > 0 && x != " ") // Filter out blank spaces
                    .Select(int.Parse) // Parse the string as a number
                    );
                card.Right.AddRange(line // Same thing but the right set of numbers
                    .Substring(bar + 1)
                    .Split()
                    .Where(x => x.Length > 0 && x != " ")
                    .Select(int.Parse)
                    );

                yield return card;
            }
        }
    }
}
