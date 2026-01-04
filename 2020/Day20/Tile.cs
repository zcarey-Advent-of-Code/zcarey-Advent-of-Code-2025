using Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day20 {
	//These should really be readable-only, but I couldn't bother to make two separate classes so here you go.
	class Tile : SquareImage<bool> {

		public int ID { get; private set; }

		public Tile(string[] input) : base(input.Length - 1) {
			ID = int.Parse(input[0].Split()[1].TrimEnd(':')); //Ex. "Tile 3079:"
			IEnumerable<bool> elements = input.Skip(1).SelectMany(x => x.Select(y => y == '#'));
			IEnumerator<bool> enumerator = elements.GetEnumerator();
			for (int y = 0; y < Size; y++) {
				for(int x = 0; x < Size; x++) {
					if (!enumerator.MoveNext()) throw new IndexOutOfRangeException("Read outside of input bounds.");
					this[x, y] = enumerator.Current;
				}
			}
		}

		public IEnumerable<bool> Top { get => this.GetData(new Rectangle(0, 0, Size, 1)); }
		public IEnumerable<bool> Bottom { get => this.GetData(new Rectangle(0, Size - 1, Size, 1)); }
		public IEnumerable<bool> Left { get => this.GetData(new Rectangle(0, 0, 1, Size)); }
		public IEnumerable<bool> Right { get => this.GetData(new Rectangle(Size - 1, 0, 1, Size)); }

	}
}
