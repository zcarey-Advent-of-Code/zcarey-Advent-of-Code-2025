using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_08 {
    public class WireLinks {

        public static Dictionary<int, int[]> PossibleNumbersFromLength = new() {
            { 2, new int[] { 1 } },
            { 3, new int[] { 7 } },
            { 4, new int[] { 4 } },
            { 5, new int[] { 2, 3, 5 } },
            { 6, new int[] { 0, 6, 9 } },
            { 7, new int[] { 8 } }
        };

        private Dictionary<char, char> Linker = new();

        private WireLinks() {

        }

        public Wires Unscramble(Wires wires) {
            StringBuilder sb = new StringBuilder();
            foreach(char wire in wires) {
                sb.Append(Linker[wire]);
            }
            return new Wires(sb.ToString());
        }

        public static WireLinks Solve(Wires[] input) {
            if (input.Length != 10) throw new ArgumentException("Length of input must be 10.", nameof(input));

            WireLinks links = new();

            // Possible links for the wires (i.e. a could be segment a, b, c, etc).
            // When there is only 1 character left in the list, then we would know the wire (key) should be linked to that segment (value)
            // We really only need this to find segment 'a' (using digit 7 and 1) then find the wire pairs.
            Dictionary<char, List<char>> possibleLinks = new() {
                { 'a', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'b', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'c', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'd', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'e', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'f', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'g', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } }
            };

            // First, search for the 4 easy numbers (8 does not help at all though, stupid 8)
            Dictionary<int, Wires> knownNumbers = new();
            foreach(Wires wires in input) {
                if (PossibleNumbersFromLength[wires.Count].Length == 1) {
                    int digit = PossibleNumbersFromLength[wires.Count][0];
                    knownNumbers[digit] = wires;
                    foreach(char c in wires) {
                        IsThisDigit(possibleLinks[c], digit);
                    }
                }
            }
            if (knownNumbers.Count != 4) 
                throw new Exception("Unable to find all the easy numbers");

            // Seven is special, we can identify it because it shares all but 1 segment with the digit 1
            // This will find us segment 'a'
            foreach(char c in knownNumbers[7]) {
                if (!knownNumbers[1].Contains(c)) {
                    CompleteLink(possibleLinks, c, 'a'); // I literally made a function just to remove 'a' from all other links lol
                    possibleLinks.Remove(c);
                    links.Linker[c] = 'a';
                    break;
                }
            }


            // At this point, we should know segment 'a' and have 3 "pairs" of wires.
            // A "pair" of wires is two wires that have exactly 2 possible segments, and the 2 wires share the same 2 possible segments
            // Pair CF: Two wires that could be either 'c' or 'f'
            // Pair BD: Two wires that could be either 'b' or 'd'
            // Pair EG: Two wires that could be either 'e' or 'g'

            // Find wire pairs. Store in list for convenience
            List<char> allSegments = new() { 'c', 'f', 'b', 'd', 'e', 'g' };
            List<char> pairCF = null;
            List<char> pairBD = null;
            List<char> pairEG = null;

            bool changes = true;
            List<char> removeLinks = new();
            while (changes) {
                changes = false;
                List<char> pair;
                foreach (var kvPair in possibleLinks) {
                    kvPair.Value.RemoveAll(removeLinks.Contains);
                }
                removeLinks.Clear();

                if (pairCF == null) {
                    pair = FindPair(possibleLinks, 'c', 'f');
                    if(pair.Count == 2) {
                        pairCF = pair;
                        possibleLinks.Remove(pair[0]);
                        possibleLinks.Remove(pair[1]);
                        removeLinks.Add('c');
                        removeLinks.Add('f');
                        changes = true;
                        continue;
                    }
                }

                if (pairBD == null) {
                    pair = FindPair(possibleLinks, 'b', 'd');
                    if (pair.Count == 2) {
                        pairBD = pair;
                        possibleLinks.Remove(pair[0]);
                        possibleLinks.Remove(pair[1]);
                        removeLinks.Add('b');
                        removeLinks.Add('d');
                        changes = true;
                        continue;
                    }
                }

                if (pairEG == null) {
                    pair = FindPair(possibleLinks, 'e', 'g');
                    if (pair.Count == 2) {
                        pairEG = pair;
                        possibleLinks.Remove(pair[0]);
                        possibleLinks.Remove(pair[1]);
                        removeLinks.Add('e');
                        removeLinks.Add('g');
                        changes = true;
                        continue;
                    }
                }

            }
            if (pairCF == null || pairBD == null | pairEG == null)
                throw new Exception("Could not find all pairs");
            
            // possibleLinks no longer needed past this point, we only need the pairs to solve
            possibleLinks.Clear();


            // Find digits 0, 6, and 9
            Wires[] digitSet069 = input.Where(x => x.Count == 6).ToArray();

            // For pair BD, the wire that's missing from this group corresponds to output 'd'
            if (!SolvePair(digitSet069, pairBD[0], pairBD[1], 'd', 'b', links)) {
                throw new Exception("Could not find BD");
            }


            // Same thing, but for the EG pair corresponding to 'e'
            if (!SolvePair(digitSet069, pairEG[0], pairEG[1], 'e', 'g', links)) {
                throw new Exception("Could not find EG");
            }


            // Same thing, but for the CF pair corresponding to 'c'
            if (!SolvePair(digitSet069, pairCF[0], pairCF[1], 'c', 'f', links)) {
                throw new Exception("Could not find CF");
            }

            // One final sanity check
            for(char c = 'a'; c <= 'g'; c++) {
                if (!links.Linker.ContainsKey(c)) {
                    throw new Exception("Bad link");
                }
            }

            return links;
        }

        /// <summary>
        /// The to keys, key1 and key2, are a "pair" because they could both be the same 2 segments (e.x. segment 'c' and segment 'f')
        /// Whichever key is missing from one of the numbers (0, 6, 9) is linked to the "missingLink", and the other key gets linked to "link2"
        /// </summary>
        /// <param name="digitSet069">The wires for numbers 0, 6, and 9, found by the length of their input.</param>
        /// <param name="key1">One of the keys in the pair</param>
        /// <param name="key2">The other key in the pair</param>
        /// <param name="missingLink">The segment that gets linked to the key that is missing</param>
        /// <param name="link2">The segment that gets linked to the other key</param>
        /// <param name="links"></param>
        /// <returns></returns>
        private static bool SolvePair(Wires[] digitSet069, char key1, char key2, char missingLink, char link2, WireLinks links) {
            foreach (Wires wire in digitSet069) {
                if (!wire.Contains(key1)) {
                    links.Linker[key1] = missingLink;
                    links.Linker[key2] = link2;
                    return true;
                } else if (!wire.Contains(key2)) {
                    links.Linker[key2] = missingLink;
                    links.Linker[key1] = link2;
                    return true;
                }
            }
            return false;
        }

        // if we know that wire 'a' is a part of the number '1', remove any other connections that aren't apart of '1'
        private static void IsThisDigit(List<char> possibilities, int digit) {
            foreach(char c in Segment.InverseSegments[digit]) {
                possibilities.Remove(c);
            }
        }

        // Yay, you found that "input" matched to "output"!
        // Now we can remove that wire from all the other possible links
        // I literally made a function just to remove 'a' from all other links lol
        private static void CompleteLink(Dictionary<char, List<char>> links, char input, char output) {
            links[input] = new() { output };
            foreach (var pair in links) {
                if (pair.Key != input) {
                    pair.Value.Remove(output);
                }
            }
        }

        // A "pair" of wires is two wires that have exactly 2 possible segments, and the 2 wires share the same 2 possible segments
        // Pair CF: Two wires that could be either 'c' or 'f'
        // Pair BD: Two wires that could be either 'b' or 'd'
        // Pair EG: Two wires that could be either 'e' or 'g'
        // We just need to ignore characters from the other known pairs when looking for a particular pair.
        private static List<char> FindPair(Dictionary<char, List<char>> possibleLinks, char segment1, char segment2) {
            List<char> pairs = new();

            foreach (var pair in possibleLinks) {
                List<char> segments = new List<char>(pair.Value);

                if (segments.Count == 2) {
                    if ((segments[0] == segment1 && segments[1] == segment2)
                        || (segments[1] == segment1 && segments[0] == segment2)
                    ) {
                        pairs.Add(pair.Key);
                    }
                }
            }

            return pairs;
        }

    }
}
