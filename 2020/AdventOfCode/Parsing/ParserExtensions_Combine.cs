using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Parsing {
	public static class ParserExtensions_Combine {

		/// <summary>
		/// Converts a 2D array (arrays within an array) into a single array containing all the elements.
		/// i.e. Converts IEnumerable&lt;IEnumerable&lt;string&gt;&gt; into IEnumerable&lt;string&gt;
		/// </summary>
		public static ParserFilter<TInput, TOutput> Combine<TInput, TOutput>(this ParserFilter<TInput, IEnumerable<TOutput>> source) {
			return source.Filter(ParserExtensions_Combine.Combine);
		}

		/// <summary>
		/// Converts a 2D array (arrays within an array) into a single array containing all the elements.
		/// i.e. Converts IEnumerable&lt;IEnumerable&lt;string&gt;&gt; into IEnumerable&lt;string&gt;
		/// </summary>
		public static ParserFilter<TInput, TOutput> Combine<TInput, TOutput>(this ParserFilter<TInput, TOutput[]> source) {
			return source.Filter(ParserExtensions_Combine.Combine);
		}

		/// <summary>
		/// Converts a 2D array (arrays within an array) into a single array containing all the elements.
		/// i.e. Converts IEnumerable&lt;IEnumerable&lt;string&gt;&gt; into IEnumerable&lt;string&gt;
		/// </summary>
		public static ParserFilter<TInput, TOutput> Combine<TInput, TOutput>(this IParser<TInput, IEnumerable<TOutput>[]> source) {
			return source.Filter(ParserExtensions_Combine.Combine);
		}

		/// <summary>
		/// Converts a 2D array (arrays within an array) into a single array containing all the elements.
		/// i.e. Converts IEnumerable&lt;IEnumerable&lt;string&gt;&gt; into IEnumerable&lt;string&gt;
		/// </summary>
		public static ParserFilter<TInput, TOutput> Combine<TInput, TOutput>(this IParser<TInput, TOutput[][]> source) {
			return source.Filter(ParserExtensions_Combine.Combine);
		}

		private static IEnumerable<T> Combine<T>(IEnumerable<IEnumerable<T>> input) {
			foreach (IEnumerable<T> list in input) {
				foreach (T item in list) {
					yield return item;
				}
			}
		}

	}
}
