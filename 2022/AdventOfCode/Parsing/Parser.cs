using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Parsing {
	public abstract class IParser<TInput, TOutput> {

		internal IParser() {
		}

		// Actually compute the final parsed input
		abstract internal TOutput Parse(TInput input);

		/// <summary>
		/// Creates a new object using a class that implements <see cref="IObjectParser{I}"/>. 
		/// Parsing is handled by the class, using the output from this parser as it's input.
		/// </summary>
		public Parser<TInput, T> Create<T>() where T : IObjectParser<TOutput>, new() {
			return this.Parse(
				(TOutput input) => {
					T obj = new T();
					obj.Parse(input);
					return obj;
				}
			);
		}

	}

	public class Parser<TInput, TOutput> : IParser<TInput, TOutput> {

		private Func<TInput, TOutput> func;

		internal Parser(Func<TInput, TOutput> filter) {
			this.func = filter;
		}

		internal override TOutput Parse(TInput input) {
			return func.Invoke(input);
		}

	}

	public sealed class ParserFilter<TInput, TOutput> : Parser<TInput, IEnumerable<TOutput>> {

		internal ParserFilter(Func<TInput, IEnumerable<TOutput>> filter) : base(filter) {
		}

		/// <summary>
		/// Creates an array of new objects from an array of data using a class that implements <see cref="IObjectParser{I}"/>.
		/// Parsing is handles by the class, using the output from this parser as it's input.
		/// </summary>
		public ParserFilter<TInput, T> FilterCreate<T>() where T : IObjectParser<TOutput>, new() {
			return this.Filter(FilterCreate<T>);
		}

		private static IEnumerable<T> FilterCreate<T>(IEnumerable<TOutput> inputs) where T : IObjectParser<TOutput>, new() {
			foreach (TOutput input in inputs) {
				T obj = new T();
				obj.Parse(input);
				yield return obj;
			}
		}

	}

	public class Parser<T> : IParser<T, T> {

		public Parser() { }

		internal override T Parse(T input) {
			return input;
		}
	}

	public sealed class Parser : Parser<StreamReader> {

		public Parser() { }

	}

}
