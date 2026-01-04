using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Parsing {
	public static class ParserExtensions_Filter {

		/// <summary>
		/// Outputs an array of data by applying a function to the input data.
		/// </summary>
		public static ParserFilter<TInput, T> Filter<TInput, TOutput, T>(this IParser<TInput, TOutput> source, Func<TOutput, IEnumerable<T>> filter) {
			return new ParserFilter<TInput, T>(
				(TInput input) => filter.Invoke(source.Parse(input))
				);
		}

		/// <summary>
		/// Outputs an array of data by applying an <see cref="IParser{I, T}"/> to the input data.
		/// </summary>
		public static ParserFilter<TInput, T> Filter<TInput, TOutput, T>(this IParser<TInput, TOutput> source, IParser<TOutput, IEnumerable<T>> filter) {
			return new ParserFilter<TInput, T>(
				(TInput input) => filter.Parse(source.Parse(input))
			);
		}

		/// <summary>
		/// Outputs an array of data by applying a function to each element of an array of input data.
		/// </summary>
		public static ParserFilter<TInput, T> Filter<TInput, TOutput, T>(this ParserFilter<TInput, TOutput> source, Func<TOutput, T> filter) {
			return source.Filter(
				(IEnumerable<TOutput> input) => input.Select(filter)
			);
		}

		/// <summary>
		/// Outputs an arra of data by applying an <see cref="IParser{I, T}"/> to each element of an array of input data.
		/// </summary>
		public static ParserFilter<TInput, T> Filter<TInput, TOutput, T>(this ParserFilter<TInput, TOutput> source, IParser<TOutput, T> filter) {
			return source.Filter(
				(IEnumerable<TOutput> input) => input.Select(filter.Parse)
			);
		}

	}
}
