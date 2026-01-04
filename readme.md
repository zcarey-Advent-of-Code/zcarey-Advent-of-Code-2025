# Advent of Code
https://adventofcode.com/about

## Adding your puzzle input
Starting in 2025, your puzzle data should be added to the "inputs" folder, usually named "Day01_Input.txt" but using the correct day number.

## Added new days
Starting in 2025, create a new class using the following format and the name of the class will be used to determine the day/part number:

```C#
public class Day01_Part1 : ISolution
{
    public object? Solve(string input_str)
    {

    }
} 
```

The program will automatically read the input file (found in the "inputs" folder) based on the latest day/part found. 
If multiple files are found for the day (usually an example and the actual input) then you will be prompted in the console to select which
input to use.

If you wish to run previous days, edit Program.cs and change ```TestSelection``` the variable to null.
 ```TestSelection = null;```