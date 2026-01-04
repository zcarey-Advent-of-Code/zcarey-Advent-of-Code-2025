# Advent-of-Code-Template

This is my personal template for creating [Advent of Code](https://adventofcode.com/) C# projects, but anyone may use it if they wish.

The way I structure my projects, is I create a solution called "zcarey-Advent-of-Code-yyyy" where yyyy is the year. 
Then I create a C# project for each day, using a common C# library to structure the code.

Solution 'zcarey-Advent-of-Code-2020'\
-----Project 'AdventOfCode'\
----------ProgramStructure.cs\
-----Project 'Day 01'\
----------Program.cs\
-----Project 'Day 02'\
----------Program.cs

To install, simply copy "AoC Day Project.zip" and "AoC Starter Solution.zip" into Visual Studio's project template folder (C:\Users\[user]\Documents\Visual Studio 2019\Templates\ProjectTemplates)

Parsing input data can be done either through the custom "Parser" system I created, or just by parsing the string that is included in the "Day Project" by default.

An great example of the parsing system is [Day 04 from AoC 2020](https://github.com/zcarey-Advent-of-Code/zcarey-Advent-of-Code-2020/blob/master/Day04/Program.cs)

Here is a sample input data:
```
ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in
```

Here is the parser used:
```
Program() : base(new Parser()
			.Filter(new LineReader())
			.Filter(new TextBlockFilter())
			.ForEach(
				// For each text block:
				new Parser<string[]>()
				.ForEach(
					// For each line in a text block...
					new SeparatedParser()
				) //So now we have a list of each line which contains a list of the elements in that line
				.Combine() // Turns it into a single list containing all the elements
				.ToArray() // Change the list into an array
				.Create<Passport>()
			) //ForEach returns a list of passports
			.ToArray()
		) { }
```

Which should (apart from sending the data to a Passport object first) parse out and return this data:
```
IEnumerable<string> {
  string[] { "ecl:gry", "pid:860033327", "eyr:2020", "hcl:#fffffd", "byr:1937", "iyr:2017", "cid:147", "hgt:183cm" },
  string[] { "iyr:2013", "ecl:amb", "cid:350", "eyr:2023", "pid:028048884", "hcl:#cfa07d", "byr:1929" },
  string[] { "hcl:#ae17e1", "iyr:2013", "eyr:2024", "ecl:brn", "pid:760753108", "byr:1931", "hgt:179cm" },
  string[] { "hcl:#cfa07d", "eyr:2025", "pid:166559648", "iyr:2011", "ecl:brn", "hgt:59in" }
}
```

When finished, you can delete all the above from the README and edit below:

# zcarey-Advent-of-Code-2021

https://adventofcode.com/2021

Written in C# .NET 6 with Visual Studio 2022
