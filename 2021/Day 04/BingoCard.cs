using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_04 {
    public class BingoCard : IObjectParser<IEnumerable<int[]>> {

        public int[][] Numbers;
        public bool[][] Markers;

        public BingoCard() {
            Numbers = new int[][] { };
            Markers = new bool[][] { };
        }

        public void Parse(IEnumerable<int[]> input) {
            Numbers = input.ToArray();
            Markers = new bool[Numbers.Length][];
            for(int i = 0; i < Numbers.Length; i++) {
                Markers[i] = new bool[Numbers[i].Length];
            }
        }

        public int this[int x,int y] {
            get {
                return Numbers[y][x];
            }

            set {
                Numbers[y][x] = value;
            }
        }

        // If the card contains the number, it is marked off.
        // If this causes the card to win, true is returned.
        public bool CheckForWin(int number) {
            // I know this is not efficient, hate me lol
            for (int y = 0; y < Numbers.Length; y++) {
                for (int x = 0; x < Numbers[y].Length; x++) {
                    if(Numbers[y][x] == number) {
                        Markers[y][x] = true;
                        return CheckRowWin(y) || CheckColumnWin(x);
                    }
                }
            }

            return false;
        }

        public bool CheckColumnWin(int column) {
            for (int y = 0; y < Markers.Length; y++) {
                if (Markers[y][column] == false) {
                    return false;
                }
            }

            return true;
        }

        public bool CheckRowWin(int row) {
            for (int x = 0; x < Markers[row].Length; x++) {
                if(Markers[row][x] == false) {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<int> UnmarkedNumbers() {
            for(int y = 0; y < Numbers.Length; y++) {
                for(int x = 0; x < Numbers[y].Length; x++) {
                    if(!Markers[y][x]) {
                        yield return Numbers[y][x];
                    }
                }
            }
        }
    }
}
