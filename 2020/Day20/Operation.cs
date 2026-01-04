using System;
using System.Collections.Generic;
using System.Text;

namespace Day20 {
	public struct Operation {

		public static readonly Operation Original = new Operation(false, false, false);
		public static readonly Operation Rotate90 = new Operation(true, false, true);
		public static readonly Operation Rotate180 = new Operation(false, true, true);
		public static readonly Operation Rotate270 = new Operation(true, true, false);
		public static readonly Operation VerticalFlip = new Operation(false, true, false);
		public static readonly Operation HorizontalFlip = new Operation(false, false, true);
		public static readonly Operation RotatedVerticalFlip = new Operation(true, true, true);
		public static readonly Operation RotatedHorizontalFlip = new Operation(true, false, false);

		public static IEnumerable<Operation> GetAllOperations() {
			for (int x = 0; x <= 1; x++) {
				for (int y = 0; y <= 1; y++) {
					for (int z = 0; z <= 1; z++) {
						yield return new Operation(x == 1, y == 1, z == 1);
					}
				}
			}
		}

		public readonly bool Transpose;
		public readonly bool FlipVertical;
		public readonly bool FlipHorizontal;

		public Operation(bool T, bool V, bool H) {
			this.Transpose = T;
			this.FlipVertical = V;
			this.FlipHorizontal = H;
		}

		public static bool operator ==(Operation left, Operation right) {
			return (left.Transpose == right.Transpose) && (left.FlipVertical == right.FlipVertical) && (left.FlipHorizontal == right.FlipHorizontal);
		}

		public static bool operator !=(Operation left, Operation right) {
			return (left.Transpose != right.Transpose) || (left.FlipVertical != right.FlipVertical) || (left.FlipHorizontal != right.FlipHorizontal);
		}

	}
}
