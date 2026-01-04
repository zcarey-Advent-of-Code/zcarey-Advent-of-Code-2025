using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day05 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            (Almanac almanac, List<long> seeds) = ParseInput1(input);
            return almanac.GetLocations(seeds.Select(x => new LargeRange(x, x))).Min(x => x.Start);
        }

        public object Part2(string input)
        { 
            (Almanac almanac, List<LargeRange> seeds) = ParseInput2(input);
            return almanac.GetLocations(seeds).Min(x => x.Start);
        }

        static (Almanac, List<LargeRange>) ParseInput2(string input)
        {
            (Almanac almanac, List<long> seeds) = ParseInput1(input);

            // Comvert the seeds into ranges
            List<LargeRange> ranges = new();
            for (int i = 0; i < seeds.Count() - 1; i += 2)
            {
                ranges.Add(LargeRange.FromLength(seeds[i], seeds[i + 1]));
            }

            // Convert the seeds to ranges
            return (almanac, ranges);
        }

        static (Almanac, List<long>) ParseInput1(string input)
        {
            List<long> seeds = new();
            Almanac almanac = new Almanac();

            IEnumerator<string> iterator = input.GetLines().GetEnumerator();
            while(iterator.MoveNext())
            {
                string line = iterator.Current;
                if (line.StartsWith("seeds:"))
                {
                    seeds.AddRange(line.Substring("seeds: ".Length).Split().Select(long.Parse));
                } else if (line == "seed-to-soil map:")
                {
                    almanac.SeedToSoil = Converter.Parse(iterator);
                } else if (line == "soil-to-fertilizer map:")
                {
                    almanac.SoilToFertilizer = Converter.Parse(iterator);
                } else if (line == "fertilizer-to-water map:")
                {
                    almanac.FertilizerToWater = Converter.Parse(iterator);
                } else if (line == "water-to-light map:")
                {
                    almanac.WaterToLight = Converter.Parse(iterator);
                } else if (line == "light-to-temperature map:")
                {
                    almanac.LightToTemperature = Converter.Parse(iterator);
                } else if (line == "temperature-to-humidity map:")
                {
                    almanac.TemperatureToHumidity = Converter.Parse(iterator);
                } else if (line == "humidity-to-location map:")
                {
                    almanac.HumidityToLocation = Converter.Parse(iterator);
                }
            }

            return (almanac, seeds);
        }

        struct ConverterRange
        {
            public LargeRange Source;
            public LargeRange Destination;

            public void TryConvert(LargeRange input, List<LargeRange> converted, List<LargeRange> outOfRange)
            {
                if (input.End < Source.Start || input.Start > Source.End)
                {
                    // Not in range!
                    outOfRange.Add(input);
                    return;
                }

                // Trim start section
                if (input.Start < Source.Start)
                {
                    outOfRange.Add(new LargeRange(input.Start, Source.Start - 1));
                    input.Start = Source.Start;
                }

                // Trim end section
                if (input.End > Source.End)
                {
                    outOfRange.Add(new LargeRange(Source.End + 1, input.End));
                    input.End = Source.End;
                }

                // With the input trimmed to this range, we can now safetly convert the input range
                input.Start = (input.Start - Source.Start) + Destination.Start;
                input.End = (input.End - Source.Start) + Destination.Start;
                converted.Add(input);
            }

            public static ConverterRange Parse(string input)
            {
                ConverterRange range = new();
                long[] values = input.Split().Select(long.Parse).ToArray();
                range.Source = LargeRange.FromLength(values[1], values[2]);
                range.Destination = LargeRange.FromLength(values[0], values[2]);
                return range;
            }
        }

        struct Converter
        {
            List<ConverterRange> ranges = new();
            public Converter() { }

            public List<LargeRange> Convert(List<LargeRange> inputs)
            {
                List<LargeRange> completed = new();
                List<LargeRange> notConverted = new();
                List<LargeRange> temp;

                foreach(ConverterRange range in ranges)
                {
                    foreach (LargeRange input in inputs)
                    {
                        range.TryConvert(input, completed, notConverted);
                    }
                    inputs.Clear();
                    temp = notConverted;

                    notConverted = inputs;
                    inputs = temp;
                }

                // Now that all conversions have been completed,
                // Any remaining inputs are mapped 1 to 1
                completed.AddRange(inputs);

                return completed;
            }

            public static Converter Parse(IEnumerator<string> input)
            {
                Converter converter = new();
                while (input.MoveNext())
                {
                    string line = input.Current;
                    if (line.Length == 0)
                    {
                        break;
                    }
                    converter.ranges.Add(ConverterRange.Parse(line));
                }

                return converter;
            }
        }

        struct Almanac
        {
            public Converter[] Converters = new Converter[7];
            public ref Converter SeedToSoil => ref Converters[0];
            public ref Converter SoilToFertilizer => ref Converters[1];
            public ref Converter FertilizerToWater => ref Converters[2];
            public ref Converter WaterToLight => ref Converters[3];
            public ref Converter LightToTemperature => ref Converters[4];
            public ref Converter TemperatureToHumidity => ref Converters[5];
            public ref Converter HumidityToLocation => ref Converters[6];
            public Almanac() { }

            public List<LargeRange> GetLocations(IEnumerable<LargeRange> seeds)
            {
                List<LargeRange> results = new (seeds);
                foreach (Converter converter in Converters)
                {
                    results = converter.Convert(results);
                }
                return results;
            }
        }
    }

}
