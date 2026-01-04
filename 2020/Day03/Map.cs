using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Day3 {
	class Map : IObjectParser<string[]> {

		private bool[,] map;
		private int width;
		private int height;

		private Point position;

		/// <summary>
		/// i.e. have not slid off the bottom of the map yet
		/// </summary>
		public bool OnSlope { get => position.Y < height; }

		/// <summary>
		/// Returns true if the current position on the map contains a tree.
		/// </summary>
		public bool HitTree { get => map[position.X, position.Y]; }

		//Parses a map from the entire input data (read as lines)
		public void Parse(string[] input) {
			width = input[0].Length;
			height = input.Length;
			map = new bool[width, height];

			for (int y = 0; y < height; y++) {
				string line = input[y];
				for (int x = 0; x < width; x++) {
					char cell = line[x];
					if (cell == '.') {
						map[x, y] = false;
					} else if (cell == '#') {
						map[x, y] = true;
					} else {
						throw new Exception("Bad input data.");
					}
				}
			}
		}

		//Moves the position down the slope.
		public void Slide() {
			position.X = (position.X + 3) % width;
			position.Y++;
		}

		//Added for part 2, change how the sled moves downhill
		public void Slide(Size slope) {
			position.X = (position.X + slope.Width) % width;
			position.Y += slope.Height;
		}

		//Reset the position to the top of the slope (0,0)
		public void Reset() {
			position = new Point();
		}

	}
}
