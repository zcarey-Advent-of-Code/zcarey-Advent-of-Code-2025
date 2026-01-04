using AdventOfCode;
using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Day20 {
	class Program : ProgramStructure<Input[]> {

		Program() : base(new Parser()
			.Filter(new LineReader())
			.Filter(new TextBlockFilter())
			.FilterCreate<Input>()
			.ToArray()
		) { }

		static void Main(string[] args) {
			new Program().Run(args);
			//new Program().Run("Example.txt");
		}

		static Operation[] ValidOperations = Operation.GetAllOperations().ToArray();
		/*static Operation[] ValidOperations = {
			Operation.Rotate90,
			Operation.Rotate180,
			Operation.Rotate270,
			Operation.HorizontalFlip,
			Operation.VerticalFlip
		};*/

		static Map solvedMap;

		protected override object SolvePart1(Input[] input) {
			int size = (int)Math.Sqrt(input.Length);
			if ((size * size) != input.Length) throw new Exception("Tiles can't form a square!");
			Map map = new Map(size);
			List<Tile> tiles = input.Select(x => x.Tile).ToList();

			if (!placeTile(map, tiles, new Point(0,0))) {
				throw new Exception("Could not solve!");
			}
			solvedMap = map;
			//Console.WriteLine(map);

			return map.Corners.Select(x => new BigInteger(x.ID)).Aggregate((x, y) => x * y).ToString();
		}

		private bool placeTile(Map map, List<Tile> tiles, Point location) {
			if (location.Y >= map.Size) {
				//Base case
				return tiles.Count == 0;
			} else if (tiles.Count == 0) {
				//Base case
				return false; 
			}

			foreach (Tile tile in tiles) {
				List<Tile> remaining = tiles.Where(x => x != tile).ToList();

				foreach (Operation op in ValidOperations) {
					tile.Transform = op;
					if (map.ValidTileLocation(location, tile)) {
						map[location] = tile;
						Point nextLocation = new Point(location.X + 1, location.Y);
						if (nextLocation.X >= map.Size) nextLocation = new Point(0, location.Y + 1);
						if (placeTile(map, remaining, nextLocation)) return true;
						map[location] = null;
					}
				}
			}

			return false;
		}

		protected override object SolvePart2(Input[] input) {
			//"Cheating" on run time by using using the result from Part1
			if (solvedMap == null) SolvePart1(input);
			SquareImage<bool> image = solvedMap.GetImage(); //Image image = solvedMap.Image;
			Pattern pattern = Pattern.SeaMonster;
			SquareImage<bool> result = new SquareImage<bool>(image.Size); 
	
			foreach (Operation op in ValidOperations) {
				image.Transform = op;
				result.Transform = op;
				foreach (Point p in image.GetIndices()) {
					pattern.Origin = p;
					if (pattern.Match(image)) {
						pattern.Or(result);
					}
				}
			}

			//Now our result is any space marked as a wave in the original image and not a sea monster in our current results
			foreach (Point p in image.GetIndices()) {
				result[p] = image[p] && !result[p];
			}
			
			return result.GetData().Count(x => x == true).ToString();
		}
	}
}
