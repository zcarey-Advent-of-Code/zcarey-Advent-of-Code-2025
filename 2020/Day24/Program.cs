using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

//NOTE: Points stored in the set are considered black since white is the default tile color
using Grid = System.Collections.Generic.HashSet<System.Drawing.Point>;

namespace Day24 {
	class Program : ProgramStructure<Directions[]> {

		static Grid part1Result;

		Program() : base(new Parser()
			.Filter(new LineReader())
			.FilterCreate<Directions>()
			.ToArray()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("Example.txt");
		}

		protected override object SolvePart1(Directions[] input) {
			//Using axial coordinates for grid system
			//https://www.redblobgames.com/grids/hexagons/#coordinates-axial
			Grid grid = new Grid();
			foreach(Directions directions in input) {
				Point result = directions.GetAbsoluteOffset();

				//If removal is successful, it means the tile was black and is now white.
				//If removal was unsuccessful, it means the tile was white so now we have to make it black
				if (!grid.Remove(result)) {
					grid.Add(result);
				}
			}

			part1Result = grid;
			return grid.Count.ToString();
		}

		protected override object SolvePart2(Directions[] input) {
			if(part1Result == null) {
				SolvePart1(input);
			}

			//Using axial coordinates for grid system
			//https://www.redblobgames.com/grids/hexagons/#coordinates-axial
			Grid currentState = part1Result;
			for(int i = 0; i < 100; i++) {
				currentState = ConwaysGameOfLife(currentState);
			}

			return currentState.Count.ToString();
		}

		private Grid ConwaysGameOfLife(Grid currentState) {
			Grid nextState = new Grid();
			Grid possibleNewCells = new Grid();

			foreach(Point blackCell in currentState) {
				//Keep track of surrounding white cells to check later
				foreach (Point p in blackCell.HexagonNeighbors().Where(x => !nextState.Contains(x))) {
					possibleNewCells.Add(p);
				}
				int blackTileCount = blackCell.HexagonNeighbors().Count(x => currentState.Contains(x)); //Count black tiles
				if((blackTileCount == 0) || (blackTileCount > 2)) {
					//It's flipped white so don't add to the next state
				} else {
					//It stays black, so we have to add the point to the next state
					nextState.Add(blackCell);
				}
			}

			foreach(Point whiteCell in possibleNewCells) {
				int blackTileCount = whiteCell.HexagonNeighbors().Count(x => currentState.Contains(x)); //Count black tiles
				if(blackTileCount == 2) {
					//Flips to black, so add to the next state
					nextState.Add(whiteCell);
				}
			}

			return nextState;
		}

	}
}
