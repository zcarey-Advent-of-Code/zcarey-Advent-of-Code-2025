using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Parsing {
	public static class ParserExtensions_Utils {

		/// <summary>
		/// Converts the input IEnumerable into an array.
		/// </summary>
		public static Parser<TInput, TOutput[]> ToArray<TInput, TOutput>(this IParser<TInput, IEnumerable<TOutput>> source) {
			return source.Parse(
				(IEnumerable<TOutput> input) => input.ToArray()
			);
		}

	}
}
