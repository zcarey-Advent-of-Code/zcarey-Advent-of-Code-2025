using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day22 {
	class Program : ProgramStructure<Deck[]> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.Filter(new TextBlockFilter())
			.FilterCreate<Deck>()
			.ToArray()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("Example.txt");
			//new Program().Run("ExampleInfinite.txt", true);
		}

		protected override object SolvePart1(Deck[] input) {
			while (!input[0].Empty && !input[1].Empty) {
				int card1 = input[0].Draw();
				int card2 = input[1].Draw();

				Deck roundsWinner = (card1 > card2) ? input[0] : input[1];
				roundsWinner.PushBack(new int[] { card1, card2 });
			}
			Deck winner = (!input[0].Empty) ? input[0] : input[1];
			return winner.Cards.Reverse().WithIndex().Select(pair => pair.Element * (pair.Index + 1)).Sum().ToString();
		}

		protected override object SolvePart2(Deck[] input) {
			Deck winner = RecursiveCombat(input);
			return winner.Cards.Reverse().WithIndex().Select(pair => pair.Element * (pair.Index + 1)).Sum().ToString();
		}

		static Deck RecursiveCombat(Deck[] decks) {
			HashSet<string> previousStates = new HashSet<string>();

			while (true) {
				string stateHash = decks[0].GetHash() + "|" + decks[1].GetHash();

				//Check to be sure this state hasn't already been played to prevent infinite recursions
				if (previousStates.Contains(stateHash)) {
					return decks[0]; //By default player 1 wins the game
				} else {
					previousStates.Add(stateHash);
					int[] cards = new int[] { decks[0].Draw(), decks[1].Draw() };
					Deck winner = null;

					if ((decks[0].Count >= cards[0]) && (decks[1].Count >= cards[1])) {
						//Both players have enough cards for a recursive game to determine the winner
						Deck[] newDecks = PrepareRecursiveDecks(decks, cards);
						winner = RecursiveCombat(newDecks);
					} else {
						//At least one player didn't have enough cards to play recursively, so just play with the standard rules
						winner = (cards[0] > cards[1]) ? decks[0] : decks[1];
					}

					//Give the winner their rightfully owed cards!
					decks[winner.Player].PushBack(new int[] { cards[winner.Player], cards[1 - winner.Player] }, false);
				}

				//If a player runs out of cards the other player wins the game
				if (decks[0].Empty) return decks[1];
				if (decks[1].Empty) return decks[0];
			}
		}

		static Deck[] PrepareRecursiveDecks(Deck[] decks, int[] cards) {
			return decks.Zip(cards, (deck, card) => new Deck(deck, card)).ToArray();
		}

	}
}
