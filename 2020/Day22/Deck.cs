using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day22 {
	class Deck : IObjectParser<string[]> {

		public int Player { get; private set; }
		public IEnumerable<int> Cards { get => cards; }
		public int Count { get => cards.Count; }
		public bool Empty { get => cards.Count == 0; }

		private Queue<int> cards;

		public Deck() { }

		public Deck(Deck copy, int count) {
			this.Player = copy.Player;
			this.cards = new Queue<int>(copy.cards.Take(count));
		}

		public void Parse(string[] input) {
			this.Player = int.Parse(input[0].Substring("Player ".Length, 1)) - 1;
			this.cards = new Queue<int>(input.Skip(1).Select(int.Parse));
		}

		public int Draw() {
			return cards.Dequeue();
		}

		public void PushBack(IEnumerable<int> cards, bool sort = true) {
			if (sort) cards = cards.OrderByDescending(x => x);
			foreach(int card in cards) {
				this.cards.Enqueue(card);
			}
		}

		public override string ToString() {
			return string.Join(", ", this.cards);
		}

		public string GetHash() {
			return new string(this.cards.Select(x => (char)('!' + x)).ToArray());
		}
	}
}
