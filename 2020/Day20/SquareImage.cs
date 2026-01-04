using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Day20 {
	public class SquareImage<T> {

		public Operation Transform { get; set; }

		public int Size { get; }

		private T[,] data;

		public SquareImage(int Size) {
			this.Size = Size;
			data = new T[Size, Size];
		}

		public SquareImage(SquareImage<T> Copy) : this(Copy.Size) {
			for(int y = 0; y < Size; y++) {
				for(int x = 0; x < Size; x++) {
					this.data[x, y] = Copy.data[x, y];
				}
			}
		}

		public T this[int x, int y] {
			get {
				if ((x < 0) || (y < 0) || (x >= Size) || (y >= Size)) throw new IndexOutOfRangeException("Coordinate outside of image bounds.");

				if (this.Transform.Transpose) {
					return data[Transform.FlipVertical ? (Size - y - 1) : y, Transform.FlipHorizontal ? (Size - x - 1) : x];
				} else {
					return data[Transform.FlipHorizontal ? (Size - x - 1) : x, Transform.FlipVertical ? (Size - y - 1) : y];
				}
			}

			set {
				if ((x < 0) || (y < 0) || (x >= Size) || (y >= Size)) throw new IndexOutOfRangeException("Coordinate outside of image bounds.");

				if (this.Transform.Transpose) {
					data[Transform.FlipVertical ? (Size - y - 1) : y, Transform.FlipHorizontal ? (Size - x - 1) : x] = value;
				} else {
					data[Transform.FlipHorizontal ? (Size - x - 1) : x, Transform.FlipVertical ? (Size - y - 1) : y] = value;
				}
			}
		}

		public IEnumerable<T> this[Rectangle region] {
			get => GetData(region);
			set => SetData(value, region);
		}

		public T this[Point p] {
			get => this[p.X, p.Y];
			set => this[p.X, p.Y] = value;
		}

		public IEnumerable<Point> GetIndices() => GetIndices(new Rectangle(0, 0, Size, Size));
		public IEnumerable<Point> GetIndices(Rectangle region) {
			if ((region.Left < 0) || (region.Top < 0) || (region.Right > Size) || (region.Bottom > Size)) throw new IndexOutOfRangeException("Region must fit within the image bounds.");

			for(int y = region.Top; y < region.Bottom; y++) {
				for(int x = region.Left; x < region.Right; x++) {
					yield return new Point(x, y);
				}
			}
		}

		public IEnumerable<T> GetData() => GetData(new Rectangle(0, 0, Size, Size));
		public IEnumerable<T> GetData(Rectangle region) {
			if ((region.Left < 0) || (region.Top < 0) || (region.Right > Size) || (region.Bottom > Size)) throw new IndexOutOfRangeException("Region must fit within the image bounds.");

			for (int y = region.Top; y < region.Bottom; y++) {
				for (int x = region.Left; x < region.Right; x++) {
					yield return this[x, y];
				}
			}
		}

		public bool RegionWithinBounds(Rectangle region) {
			return (region.Left >= 0) && (region.Top >= 0) && (region.Right <= Size) && (region.Bottom <= Size);
		}

		public void SetData(IEnumerable<T> data) => SetData(data, new Rectangle(0, 0, Size, Size));
		public void SetData(IEnumerable<T> data, Rectangle region) {
			if ((region.Left < 0) || (region.Top < 0) || (region.Right > Size) || (region.Bottom > Size)) throw new IndexOutOfRangeException("Region must fit within the image bounds.");

			IEnumerator<T> enumerator = data.GetEnumerator();
			for(int y = region.Top; y < region.Bottom; y++) {
				for(int x = region.Left; x < region.Right; x++) {
					if (!enumerator.MoveNext()) throw new ArgumentOutOfRangeException("data", "Not enough elements in data for the given region.");
					this[x, y] = enumerator.Current;
				}
			}
		}

		public string ToString(Func<T, string> converter) {
			StringBuilder sb = new StringBuilder();
			for (int y = 0; y < Size; y++) {
				for (int x = 0; x < Size; x++) {
					sb.Append(converter(this[x, y]));//this[x, y] ? '#' : '.');
				}
				if (y < (Size - 1)) sb.AppendLine();
			}
			return sb.ToString();
		}

	}
}
