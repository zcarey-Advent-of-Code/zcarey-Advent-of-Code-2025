using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Common {
	public static class LinqExtensions {

		/// <summary>
		/// Gets all string elements separated by either space or newlines
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetElements(this IEnumerable<string> source) {
			foreach(string line in source) {
				string[] elements = line.Split();
				foreach(string element in elements) {
					yield return element;
				}
			}
		}

		public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T newElement) {
			if (source == null) throw new ArgumentNullException("source", "Enumberable was null.");
			foreach (T element in source) yield return element;
			yield return newElement;
		}

		public static IEnumerable<Point> Indices<T>(this T[,] source) {
			for(int y = 0; y < source.GetLength(1); y++) {
				for(int x = 0; x < source.GetLength(0); x++) {
					yield return new Point(x, y);
				}
			}
		}

		public static IEnumerable<T> Values<T>(this T[,] source) {
			for (int y = 0; y < source.GetLength(1); y++) {
				for (int x = 0; x < source.GetLength(0); x++) {
					yield return source[x, y];
				}
			}
		}

		public static IEnumerable<(int Index, T Element)> WithIndex<T>(this IEnumerable<T> source) {
			int index = 0;
			foreach(T element in source) {
				yield return (index++, element);
			}
		}
	}
}
