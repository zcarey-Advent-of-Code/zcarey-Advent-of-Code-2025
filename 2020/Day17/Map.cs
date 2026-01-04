using AdventOfCode.Parsing;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day17 {
	class Map : IObjectParser<string[]> {

		private Dictionary<Point, bool> map = new Dictionary<Point, bool>();
		private bool use4D = false;

		public void Simulate(int activeMin, int activeMax, int inactiveVal) {
			Dictionary<Point, bool> changes = new Dictionary<Point, bool>();
			
			//Find cell changes
			foreach(KeyValuePair<Point, bool> cell in map) {
				int active = GetSurroundingCells(cell.Key).Where(x => GetValue(x)).Count();
				if (cell.Value) {
					if((active < activeMin) || (active > activeMax)) {
						changes[cell.Key] = false;
					}
				} else {
					if(active == inactiveVal) {
						changes[cell.Key] = true;
					}
				}
			}

			//Apply changes
			foreach(KeyValuePair<Point, bool> change in changes) {
				SetValue(change.Key, change.Value);
			}
		}

		public int CountActive() {
			return map.Where(x => x.Value).Count();
		}

		private void SetValue(Point point, bool value) {
			map[point] = value;
			if (value == true) {
				foreach (Point delta in GetSurroundingCells(point)) {
					if(!map.ContainsKey(delta)) {
						map[delta] = false;
					}
				}
			}
		}

		private bool GetValue(Point point) {
			bool result = false;
			if(map.TryGetValue(point, out result)) {
				return result;
			} else {
				return false;
			}
		}

		private IEnumerable<Point> GetSurroundingCells(Point point) {
			if (this.use4D) {
				for (int dw = -1; dw <= 1; dw++) {
					for (int dz = -1; dz <= 1; dz++) {
						for (int dy = -1; dy <= 1; dy++) {
							for (int dx = -1; dx <= 1; dx++) {
								if (dx == 0 && dy == 0 && dz == 0 && dw == 0) continue;
								yield return point + new Point(dx, dy, dz, dw);
							}
						}
					}
				}
			} else {
				for (int dz = -1; dz <= 1; dz++) {
					for (int dy = -1; dy <= 1; dy++) {
						for (int dx = -1; dx <= 1; dx++) {
							if (dx == 0 && dy == 0 && dz == 0) continue;
							yield return point + new Point(dx, dy, dz, 0);
						}
					}
				}
			}
		}

		public void Parse(string[] input) {
			Point point = new Point();
			foreach(string line in input) {
				point.X = 0;
				foreach(char c in line) {
					if(c == '#') {
						SetValue(point, true);
					}else if(c == '.') {
						SetValue(point, false);
					} else {
						throw new Exception("Bad input.");
					}
					point.X++;
				}
				point.Y++;
			}
		}

		public void Initialize4D() {
			use4D = true;
			List<Point> reset = map.Where(x => x.Value == true).Select(x => x.Key).ToList();
			foreach(Point cell in reset) {
				SetValue(cell, true);
			}
		}
	}
}
