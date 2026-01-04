using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Parsing {
    public static class ParserExtensions_Accumulate {

		/// <summary>
		/// Returns multiple items by parsing the same input differently
		/// </summary>
		public static Parser<TInput, Tuple<T1, T2>> Accumulate<TInput, TOutput, T1, T2>(this IParser<TInput, TOutput> source, Func<TOutput, T1> filter1, Func<TOutput, T2> filter2) {
			return new Parser<TInput, Tuple<T1, T2>>(
				(TInput input) => {
					TOutput value = source.Parse(input);
					return new Tuple<T1, T2>(
						filter1(value),
						filter2(value)
					);
				}
			);
		}

		public static Parser<TInput, Tuple<T1, T2>> Accumulate<TInput, TOutput, T1, T2>(this IParser<TInput, TOutput> source, Func<TOutput, T1> filter1, IParser<TOutput, T2> filter2) {
			return source.Accumulate(filter1, filter2.Parse);
		}

		public static Parser<TInput, Tuple<T1, T2>> Accumulate<TInput, TOutput, T1, T2>(this IParser<TInput, TOutput> source, IParser<TOutput, T1> filter1, Func<TOutput, T2> filter2) {
			return source.Accumulate(filter1.Parse, filter2);
		}

		public static Parser<TInput, Tuple<T1, T2>> Accumulate<TInput, TOutput, T1, T2>(this IParser<TInput, TOutput> source, IParser<TOutput, T1> filter1, IParser<TOutput, T2> filter2) {
			return source.Accumulate(filter1.Parse, filter2.Parse);
		}

	}
}
