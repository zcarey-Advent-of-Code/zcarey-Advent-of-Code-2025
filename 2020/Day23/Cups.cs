using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Day23 {
	class Cups : IObjectParser<string>, IEnumerable<int> {

		/// <summary>
		/// The number of cups stored
		/// </summary>
		public int Count { get => cups.Count; }

		/// <summary>
		/// The first cup in the list.
		/// </summary>
		public LinkedListNode<int> First { get => cups.First; }

		private LinkedList<int> cups;
		private LinkedListNode<int>[] lookup;

		/// <summary>
		/// Parses cup labels from the given input string
		/// </summary>
		/// <param name="input"></param>
		public void Parse(string input) {
			cups = new LinkedList<int>(input.Select(x => int.Parse(x.ToString())));
			createNodeLookup();
		}

		public Cups() { }

		/// <summary>
		/// Copies cups from a previously parsed object and appends additional cup values.
		/// </summary>
		/// <param name="copy"></param>
		/// <param name="additional"></param>
		public Cups(Cups copy, IEnumerable<int> additional) {
			cups = new LinkedList<int>(copy.cups.Concat(additional));
			createNodeLookup();
		}

		//Create a look-up for every number. When looking for cup labeled '2' can be found by calling lookup[2 - 1];
		private void createNodeLookup() {
			lookup = new LinkedListNode<int>[cups.Count];
			foreach(LinkedListNode<int> node in cups.First.AsCircularEnumerable()) {
				lookup[node.Value - 1] = node;
			}
		}

		/// <summary>
		/// Retreive the cup with the given label. This is faster than iterating through the list.
		/// </summary>
		/// <param name="CupLabel"></param>
		/// <returns></returns>
		public LinkedListNode<int> this[int CupLabel] {
			get => this.lookup[CupLabel - 1];
		}

		public IEnumerator<int> GetEnumerator() {
			return cups.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return cups.GetEnumerator();
		}
	}
}
