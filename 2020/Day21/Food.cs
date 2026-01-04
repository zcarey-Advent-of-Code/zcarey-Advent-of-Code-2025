using System;
using System.Collections.Generic;
using System.Linq;

namespace Day21 {
	class Food {

		private string[] ingredients;
		public IEnumerable<string> Ingredients { get => ingredients; }

		private string[] allergens;
		public IEnumerable<string> Allergens { get => allergens; }

		public Food(string input) {
			int index = input.IndexOf("(");
			this.ingredients = input.Substring(0, index - 1).Split();
			this.allergens = input.Substring(index + "(contains ".Length).TrimEnd(')').Split(", ");
		}

		public static Food Parse(string input) {
			return new Food(input);
		}

		public override string ToString() {
			return string.Join(", ", ingredients) + " (" + string.Join(", ", allergens) + ")";
		}

	}
}
