using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Day23 {
	class Program : ProgramStructure<Cups> {

		Program() : base(new Parser()
			.Parse(new StringReader())
			.Create<Cups>()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("Example.txt");
		}

		protected override object SolvePart1(Cups cups) {
			//Only use listed cups with 100 moves
			GetCrabby(ref cups, cups.Count, 100);

			//Find the cup labeled 1
			IEnumerable<int> oneCup = cups[1].AsCircularEnumerable().Select(node => node.Value);

			//Start with the cup AFTER the cup labeled 1 and return each cup in order as a string.
			return new string(oneCup.Skip(1).Select(x => (char)('0' + x)).ToArray());
		}

		protected override object SolvePart2(Cups cups) {
			//Add additional cups (up to one million) and with ten million moves
			GetCrabby(ref cups, 1000000, 10000000);

			//Find the cup labeled 1
			IEnumerable<int> oneCup = cups[1].AsCircularEnumerable().Select(node => node.Value);

			//Start with the cup AFTER the cup labeled 1 and return the next two cup labels multiplied.
			return (new BigInteger(oneCup.Skip(1).First()) * new BigInteger(oneCup.Skip(2).First())).ToString();
		}

		static void GetCrabby(ref Cups cups, int numCups, int numMoves) {
			int minCup = cups.Min();
			int maxCup = cups.Max();
			if (numCups > cups.Count) {
				int cupsToAdd = numCups - cups.Count;
				cups = new Cups(cups, Enumerable.Range(maxCup + 1, cupsToAdd));
				maxCup += cupsToAdd;
			}


			LinkedListNode<int> currentCup = cups.First;
			for (int i = 0; i < numMoves; i++) {
				//Pick up the next 3 cups for later use
				List<LinkedListNode<int>> pickedUp = currentCup.CircularRemoveAfter(3).ToList(); //cups.Skip(1).Take(3).ToList();

				//Calcluate our destination cup
				int destination = currentCup.Value;
				do {
					destination--;
					if (destination < minCup) destination = maxCup;
				} while (pickedUp.Select(x => x.Value).Contains(destination));

				//Find the destination cup
				LinkedListNode<int> destinationCup = cups[destination];

				//Insert the picked up cups after the destination cup
				destinationCup.AddAfter(pickedUp);

				//Move the crab to the next cup for the next move.
				currentCup = currentCup.CircularNext(); //cups.Next();
			}
		}
	}
}
