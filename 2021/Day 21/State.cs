using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_21 {
    public struct State {

        // 1-10 (0-9)
        public int Position1 { get => (hash >> 15) & 0b1111; }
        // 0-21
        public int Score1 { get => (hash >> 10) & 0b11111; }

        public int Position2 { get => (hash >> 6) & 0b1111; }
        public int Score2 { get => (hash >> 1) & 0b11111; }

        // True is player 2's turn
        public bool Turn { get => (hash & 0b1) == 1; }

        // 1111222223333444445
        private int hash;

        public State(int pos1, int score1, int pos2, int score2, bool turn) {
            hash =
                ((pos1 & 0b1111) << 15) |
                ((score1 & 0b11111) << 10) |
                ((pos2 & 0b1111) << 6) |
                ((score2 & 0b11111) << 1) |
                (turn ? 1 : 0);
        }

        public override int GetHashCode() {
            return hash;
        }

    }
}
