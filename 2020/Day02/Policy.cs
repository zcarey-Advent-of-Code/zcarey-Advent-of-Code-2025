using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Day2 {
	class Policy {

		private int minimum;
		private int maximum;
		private char letter;

		/// <summary>
		/// Parses the given input data line and returns the policy
		/// </summary>
		public Policy(string input) {
			string policy = input.Substring(0, input.IndexOf(':'));
			letter = policy[policy.Length - 1];
			int numberSeparator = policy.IndexOf('-');
			minimum = int.Parse(policy.Substring(0, numberSeparator));
			maximum = int.Parse(policy.Substring(numberSeparator + 1,  policy.Length - numberSeparator - 3));
		}

		public bool isValidPassword(string password) {
			int count = password.Where(x => x == letter).Count();
			return count >= minimum && count <= maximum;
		}

		public bool isValidPart2Password(string password) {
			return password[minimum - 1] == letter ^ password[maximum - 1] == letter;
		}

	}
}
