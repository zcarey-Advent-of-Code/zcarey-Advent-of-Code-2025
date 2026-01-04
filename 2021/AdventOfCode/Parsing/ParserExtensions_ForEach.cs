using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Parsing {
	public static class ParserExtensions_ForEach {

		/// <summary>
		/// Applys a function to each element in the input array.
		/// </summary>
		public static ParserFilter<TInput, T> ForEach<TInput, TOutput, T>(this IParser<TInput, IEnumerable<TOutput>> source, Func<TOutput, T> filter) {
			return source.Filter(
				(IEnumerable<TOutput> input) => ForEach(input, filter)
			);
		}

		/// <summary>
		/// Applys a <see cref="IParser{TInput, TOutput}"/> to each element in the input array.
		/// </summary>
		public static ParserFilter<TInput, T> ForEach<TInput, TOutput, T>(this IParser<TInput, IEnumerable<TOutput>> source, IParser<TOutput, T> filter) {
			return source.Filter(
				(IEnumerable<TOutput> input) => ForEach(input, filter.Parse)
			);
		}

		/// <summary>
		/// Applys a function to each element in the input array.
		/// </summary>
		public static ParserFilter<TInput, T> ForEach<TInput, TOutput, T>(this IParser<TInput, TOutput[]> source, Func<TOutput, T> filter) {
			return source.Filter(
				(TOutput[] input) => ForEach(input, filter)
			);
		}

		/// <summary>
		/// Applys a <see cref="IParser{TInput, TOutput}"/> to each element in the input array.
		/// </summary>
		public static ParserFilter<TInput, T> ForEach<TInput, TOutput, T>(this IParser<TInput, TOutput[]> source, IParser<TOutput, T> filter) {
			return source.Filter(
				(TOutput[] input) => ForEach(input, filter.Parse)
			);
		}

		private static IEnumerable<T> ForEach<TOutput, T>(IEnumerable<TOutput> inputs, Func<TOutput, T> filter) {
			foreach (TOutput input in inputs) {
				yield return filter.Invoke(input);
			}
		}

		public static ParserFilter<TInput, T> ForEach<TInput, TOutput, T>(this IParser<TInput, TOutput> source) where TOutput : IEnumerable<T> {
			return source.Filter(
				(TOutput input) => input
			);
        }

		/// <summary>
		///  Returns each char in the string.
		/// </summary>
		public static ParserFilter<TInput, char> ForEach<TInput>(this IParser<TInput, string> source) {
			return source.Filter(
				(string input) => (IEnumerable<char>)input
			);
		}

		/// <summary>
		/// Applies a function to every char in the string
		/// </summary>
		public static ParserFilter<TInput, T> ForEach<TInput, T>(this IParser<TInput, string> source, Func<char, T> filter) {
			return source.Filter(
				(string input) => input.Select(filter)
			);
        }

		/// <summary>
		/// Applies a parser to every char in the string
		/// </summary>
		public static ParserFilter<TInput, T> ForEach<TInput, T>(this IParser<TInput, string> source, IParser<char, T> filter) {
			return source.Filter(
				(string input) => input.Select(filter.Parse)
			); ;
        }

	}
}
