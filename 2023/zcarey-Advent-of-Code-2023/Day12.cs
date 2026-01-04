using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day12 : AdventOfCodeProblem
    {
        public object Part1(string input)
        { 
            long count = 0;
            foreach((string springs, int[] notes, int lastSpringIndex) in Parse(input))
            {
                //count += getArrangements(springs, notes);
                DFA dfa = new(notes);
                count += dfa.count(springs);
            }

            return count;
        }

        public int getArrangements(string springs, int[] notes) {
            return getArrangements(springs, 0, notes, 0, 0);
        }

        public int getArrangements(string springs, int springIndex, int[] notes, int currentNoteIndex, int currentNoteCount) {
            if (springIndex >= springs.Length) {
                // Ensure all notes have been met
                if (currentNoteIndex >= notes.Length || (currentNoteIndex == notes.Length - 1 && notes[currentNoteIndex] == currentNoteCount)) {
                    // End of input, notes were met, valid arrangement
                    return 1;
                } else {
                    // End of string, but notes weren't met. Invalid arrangement
                    return 0;
                }
            } else {
                Func<string, int, int[], int, int, int> func;
                switch(springs[springIndex]) {
                    case '.':
                        func = getArrangementsEmpty;
                        break;
                    case '#':
                        func = getArrangementsSpring;
                        break;
                    case '?':
                        func = getArrangementsUnknown;
                        break;
                    default:
                        return 0;
                }

                return func(springs, springIndex, notes, currentNoteIndex, currentNoteCount);
            }
        }

        public int getArrangementsEmpty(string springs, int springIndex, int[] notes, int currentNoteIndex, int currentNoteCount) {
            if (currentNoteIndex < notes.Length) {
                if (currentNoteCount < notes[currentNoteIndex]) {
                    if (currentNoteCount != 0) {
                        // Invalid arrangement
                        return 0;
                    }
                } else if (currentNoteCount == notes[currentNoteIndex]) {
                    // next note
                    currentNoteIndex++;
                    currentNoteCount = 0;
                } else {
                    // Invalid state
                    return 0;
                }
            }

            return getArrangements(springs, springIndex + 1, notes, currentNoteIndex, currentNoteCount);
        }

        public int getArrangementsSpring(string springs, int springIndex, int[] notes, int currentNoteIndex, int currentNoteCount) {
            if (currentNoteIndex >= notes.Length) {
                // Invalid arrangement
                return 0;
            }

            if (currentNoteCount >= notes[currentNoteIndex]) {
                // Invalid arrangement, group is too large
                return 0;
            }

            currentNoteCount++;

            return getArrangements(springs, springIndex + 1, notes, currentNoteIndex, currentNoteCount);
        }

        public int getArrangementsUnknown(string springs, int springIndex, int[] notes, int currentNoteIndex, int currentNoteCount) {
            // Try both values
            return getArrangementsSpring(springs, springIndex, notes, currentNoteIndex, currentNoteCount)
                + getArrangementsEmpty(springs, springIndex, notes, currentNoteIndex, currentNoteCount);
        }

        public object Part2(string input)
        {
            // Alright, we will have to use some Deterministic Finite Automata for this one
            long count = 0;
            foreach((string springs, int[] notes, int lastSpringIndex) in Unfold(Parse(input)))
            {
                DFA dfa = new(notes);
                count += dfa.count(springs);
            }

            return count;
        }

        class state {
            public state dot = null;
            public state hash = null;
        };

        class DFA {

            List<state> states;

            public DFA(int[] broken_pattern) {
                int count = broken_pattern.Sum() + broken_pattern.Length;
                states = new List<state>(count);
                for(int j = 0; j < count; j++) {
                    states.Add(new state());
                }
                states[0].dot = states[0];
                states[0].hash = states[1];

                int i = 1;
                foreach (var b in broken_pattern) {
                    for (int j = 0; j<b - 1; ++i, ++j) {
                        states[i].hash = states[i + 1];
                    }

                    if (i + 2 < states.Count) {
                        states[i].dot = states[i + 1];
                        ++i;
                        states[i].dot = states[i];
                        states[i].hash = states[i + 1];
                    }
                    ++i;
                }

                states.Last().dot = states.Last();
            }

            public long count(string match) {
                Dictionary<state, long> curr = new() {
                    { states[0], 1 }
                };
                foreach (char c in match) {
                    Dictionary<state, long> next = new(states.Count);
                    foreach (var pair in curr) {
                        if ((c == '.' || c == '?') && pair.Key.dot != null) {
                            if (next.ContainsKey(pair.Key.dot)) {
                                next[pair.Key.dot] += pair.Value;
                            } else {
                                next[pair.Key.dot] = pair.Value;
                            }
                        }
                        if ((c == '#' || c == '?') && pair.Key.hash != null) {
                            if (next.ContainsKey(pair.Key.hash)) {
                                next[pair.Key.hash] += pair.Value;
                            } else {
                                next[pair.Key.hash] = pair.Value;
                            }
                        }
                    }
                    curr = next;
                }
                var x = curr[states.Last()];
                return x;
            }
        }

        IEnumerable<(string Springs, int[] Notes, int LastSpringIndex)> Parse(string input)
        {
            foreach (string line in input.GetLines())
            {
                string[] args = line.Split();
                int[] notes = args[1]
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();

                yield return (args[0], notes, args[0].LastIndexOf('#'));
            }
        }

        IEnumerable<(string Springs, int[] Notes, int LastSpringIndex)> Unfold(IEnumerable<(string Springs, int[] Notes, int LastSpringIndex)> inputs) {
            foreach(var input in inputs) {
                string springs = input.Springs + "?" + input.Springs + "?" + input.Springs + "?" + input.Springs + "?" + input.Springs;
                int[] notes = new int[input.Notes.Length * 5];
                for(int i = 0; i < input.Notes.Length; i++) {
                    notes[i] = input.Notes[i];
                    notes[input.Notes.Length + i] = input.Notes[i];
                    notes[input.Notes.Length * 2 + i] = input.Notes[i];
                    notes[input.Notes.Length * 3 + i] = input.Notes[i];
                    notes[input.Notes.Length * 4 + i] = input.Notes[i];
                }
                int lastSpringIndex = input.Springs.Length * 4 + input.LastSpringIndex;
                yield return (springs, notes, lastSpringIndex);
            }
        }
    }
}
