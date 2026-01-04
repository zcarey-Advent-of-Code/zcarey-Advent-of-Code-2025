using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Linq;
using Common;
using AdventOfCode.Parsing;

namespace Day4 {
	class Passport : IObjectParser<string[]> {

		private static readonly string[] EyeColors = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

		public string BirthYear { get; protected set; }
		public string IssueYear { get; protected set; }
		public string ExpirationYear { get; protected set; }
		public string Height { get; protected set; }
		public string HairColor { get; protected set; }
		public string EyeColor { get; protected set; }
		public string PassportID { get; protected set; }
		public string CountryID { get; protected set; }

		public bool IsValid { get; private set; } = false;

		public void Parse(string[] input) {
			foreach (string element in input/*.GetElements()*/) {
				int index = element.IndexOf(':');
				string key = element.Substring(0, index);
				string value = element.Substring(index + 1);

				switch (key) {
					case "byr":
						BirthYear = value;
						break;
					case "iyr":
						IssueYear = value;
						break;
					case "eyr":
						ExpirationYear = value;
						break;
					case "hgt":
						Height = value;
						break;
					case "hcl":
						HairColor = value;
						break;
					case "ecl":
						EyeColor = value;
						break;
					case "pid":
						PassportID = value;
						break;
					case "cid":
						CountryID = value;
						break;
				}
			}
		}

		public void Validate() {
			IsValid = (BirthYear != null)
				&& (IssueYear != null)
				&& (ExpirationYear != null)
				&& (Height != null)
				&& (HairColor != null)
				&& (EyeColor != null)
				&& (PassportID != null);
				//&& ((ignoreCID) ? true : (CountryID != null));
		}

		public void Validate2() {
			BirthYear = parseIntRange(BirthYear, 1920, 2002);
			IssueYear = parseIntRange(IssueYear, 2010, 2020);
			ExpirationYear = parseIntRange(ExpirationYear, 2020, 2030);
			Height = parseHeight(Height);
			HairColor = parseHairColor(HairColor);
			EyeColor = parseEyeColor(EyeColor);
			PassportID = parsePID(PassportID);
			Validate();
		}

		private string parseIntRange(string input, int min, int max) {
			int result;
			if (input != null && int.TryParse(input, out result)) {
				if (result >= min && result <= max) {
					return input;
				}
			}
			return null;
		}

		private string parseHeight(string input) {
			int result;
			if (input == null || !int.TryParse(input.Substring(0, input.Length - 2), out result)) {
				return null;
			}

			if (input.EndsWith("cm")) {
				return (result >= 150 && result <= 193) ? input : null;
			} else if (input.EndsWith("in")) {
				return (result >= 59 && result <= 76) ? input : null;
			} else {
				return null;
			}
		}

		private string parseHairColor(string input) {
			if (input != null && input.Length == 7 && input.StartsWith("#")) {
				for (int i = 1; i < input.Length; i++) {
					char c = input[i];
					if (!((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f'))) {
						return null;
					}
				}
				return input;
			}
			return null;
		}

		private string parseEyeColor(string input) {
			if (input != null && EyeColors.Contains(input)) {
				return input;
			} else {
				return null;
			}
		}

		private string parsePID(string input) {
			int id;
			if (input != null && input.Length == 9 && int.TryParse(input, out id)) {
				return input;
			} else {
				return null;
			}
		}

	}
}
