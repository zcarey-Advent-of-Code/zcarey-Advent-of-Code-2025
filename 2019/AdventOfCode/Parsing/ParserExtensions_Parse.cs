using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Parsing {
	public static class ParserExtensions_Parse {

		/// <summary>
		/// Outputs a new value by applying a function tho the input value.
		/// </summary>
		public static Parser<TInput, T> Parse<TInput, TOutput, T>(this IParser<TInput, TOutput> source, Func<TOutput, T> filter) {
			return new Parser<TInput, T>(
				(TInput input) => filter.Invoke(source.Parse(input))
			);
		}

		/// <summary>
		/// Outputs a new value by applying a <see cref="IParser{I, T}"/> to the input value.
		/// </summary>
		public static Parser<TInput, T> Parse<TInput, TOutput, T>(this IParser<TInput, TOutput> source, IParser<TOutput, T> filter) {
			return new Parser<TInput, T>(
				(TInput input) => filter.Parse(source.Parse(input))
			);
		}

	}
}
