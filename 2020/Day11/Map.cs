using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Day11 {
	class Map : IObjectParser<string[]> {

		public int Width { get; private set; }
		public int Height { get; private set; }

		private int[,] map;

		public void Parse(string[] input) {
			Width = input[0].Length;
			Height = input.Length;
			map = new int[Width, Height];
			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Width; x++) {
					char c = input[y][x];
					if (c == '.') {
						map[x, y] |= (int)SeatState.Floor;
					} else if (c == 'L') {
						map[x, y] |= (int)SeatState.Empty;
					} else if (c == '#') {
						map[x, y] |= (int)SeatState.Occupied;
					} else {
						throw new Exception("Unknown seat character.");
					}
				}
			}
		}

		public int CountOccupiedSeats() {
			int count = 0;
			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Width; x++) {
					if (isOccupied(x, y)) {
						count++;
					}
				}
			}
			return count;
		}

		private bool isValidLocation(int x, int y) {
			return (y >= 0) && (y < Height) && (x >= 0) && (x < Width);
		}

		private bool isSeat(int x, int y) {
			return isValidLocation(x, y) && ((map[x, y] & (int)(SeatState.Empty | SeatState.Occupied)) != 0);
		}

		private bool isOccupied(int x, int y) {
			return isValidLocation(x, y) && ((map[x, y] & (int)SeatState.Occupied) != 0);
		}

		private void setOccupied(int x, int y, bool occupied) {
			map[x, y] = (int)(occupied ? SeatState.Occupied : SeatState.Empty) | (map[x, y] & 0x0F);
		}

		private void setSeatCount(int x, int y, int count) {
			map[x, y] = (map[x, y] & (int)(SeatState.Empty | SeatState.Occupied)) | (count & 0x0F);
		}

		private int getSeatCount(int x, int y) {
			return map[x, y] & 0x0F;
		}

		public void UpdateOccupiedCount() {
			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Width; x++) {
					if (isSeat(x, y)) {
						//Count surrounding seats
						int count = 0;
						for (int j = -1; j <= 1; j++) {
							for (int i = -1; i <= 1; i++) {
								if (i == 0 && j == 0) continue;
								if (isOccupied(x + i, y + j)) {
									count++;
								}
							}
						}
						setSeatCount(x, y, count);
					}
				}
			}
		}

		//Returns true if at least one seat state updated
		public bool UpdateSeatState(int tolerance) {
			bool updated = false;
			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Width; x++) {
					if (isSeat(x, y)) {
						int count = getSeatCount(x, y);
						if (isOccupied(x, y)) {
							if (count >= tolerance) {
								setOccupied(x, y, false);
								updated = true;
							}
						} else {
							if (count == 0) {
								setOccupied(x, y, true);
								updated = true;
							}
						}
					}
				}
			}
			return updated;
		}

		public void UpdateOccupiedCount2() {
			for (int y = 0; y < Height; y++) {
				for (int x = 0; x < Width; x++) {
					if (isSeat(x, y)) {
						//Count surrounding seats
						setSeatCount(x, y, getPart2SeatCount(x, y));
					}
				}
			}
		}

		private int getPart2SeatCount(int x, int y) {
			int count = 0;
			foreach(Direction dir in AllDirections) {
				Point? hit = FindFirstSeat(x, y, dir);
				if((hit != null) && (isOccupied(hit.Value.X, hit.Value.Y))) {
					count++;
				}
			}
			return count;
		}

		private Point? FindFirstSeat(int x, int y, Direction dir) {
			int dx = (int)(sbyte)(((int)dir >> 8) & 0xFF);
			int dy = (int)(sbyte)((int)dir & 0xFF);

			x += dx;
			y += dy;
			bool insideMap = isValidLocation(x, y);
			while (insideMap) {
				if (isSeat(x, y)) {
					return new Point(x, y);
				}
				x += dx;
				y += dy;
				insideMap = isValidLocation(x, y);
			}
			return null;
		}


		private static Direction[] AllDirections = new Direction[]{
			Direction.North,
			Direction.North | Direction.East,
			Direction.East,
			Direction.South | Direction.East,
			Direction.South,
			Direction.South | Direction.West,
			Direction.West,
			Direction.North | Direction.West
		};

		[Flags]
		enum Direction {
			North = 0x00FF,
			South = 0x0001,
			East = 0x0100,
			West = 0xFF00
		}

	}
}
