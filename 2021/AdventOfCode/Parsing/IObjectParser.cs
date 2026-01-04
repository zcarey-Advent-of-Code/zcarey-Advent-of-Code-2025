using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Parsing {

	/// <summary>
	/// Used for parsing objects, since a new object needs to be created for every input needed.
	/// </summary>
	/// <typeparam name="I"></typeparam>
	public interface IObjectParser<I> {

		/// <summary>
		/// Parse the object using the given input. Assume it is a new object.
		/// </summary>
		/// <param name="input"></param>
		public void Parse(I input);

	}

}
