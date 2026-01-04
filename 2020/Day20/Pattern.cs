using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;

namespace Day20 {
	class Pattern {

		public static Pattern SeaMonster = new Pattern(new string[]{
			"                  # ",
			"#    ##    ##    ###",
			" #  #  #  #  #  #   "
		});

		public Point Origin { get; set; }
		public Size Area { get => new Size(Width, Height); }
		public int Width { get; private set; }
		public int Height { get; private set; }

		private bool[,] pattern;

		public Pattern(string[] pattern) {
			this.Width = pattern[0].Length;
			this.Height = pattern.Length;
			this.pattern = new bool[Width, Height];
			for (int y = 0; y < Height; y++) {
				string line = pattern[y];
				if (line.Length != Width) throw new Exception("String lengths must all match!");
				for (int x = 0; x < Width; x++) {
					char c = line[x];
					if ((c == ' ') || (c == '#')) {
						this.pattern[x, y] = (c == '#');
					} else {
						throw new FormatException("Pattern must only contain \' \' or \'#\'.");
					}
				}
			}
		}

		public void Or(SquareImage<bool> data) {
			foreach (Point p in pattern.Indices()) {
				data[Origin.X + p.X, Origin.Y + p.Y] |= this.pattern[p.X, p.Y];
			}
		}

		public bool Match(SquareImage<bool> image) {
			Rectangle region = new Rectangle(Origin, this.Area);
			if (!image.RegionWithinBounds(region)) return false;
			return image[region].SequenceEqual(pattern.Indices().Select(p => {
				bool debug = pattern[p.X, p.Y];
				return debug;
			}), new PatternComparer());
		}

		//NOTE: The first sequence should be the image and the second sequence should be the pattern
		private class PatternComparer : IEqualityComparer<bool> {
			public bool Equals([AllowNull] bool image, [AllowNull] bool pattern) {
				if (image == null || pattern == null) return false;
				else return !pattern || (pattern && image);
			}

			public int GetHashCode([DisallowNull] bool obj) {
				return obj.GetHashCode();
			}
		}

	}
}
