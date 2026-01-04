using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_02 {
    public enum RockPaperScissors {

        Rock,
        Paper,
        Scissors

    }

    public static class RockPaperScissorsExtensions {

        // 0 for lose, 1 for draw, 2 for win
        public static int Beats(this RockPaperScissors A, RockPaperScissors B) {
            if (A == B)
                return 1;

            // Not pretty, but works
            if (A == RockPaperScissors.Rock && B == RockPaperScissors.Scissors) {
                return 2;
            } else if (A == RockPaperScissors.Paper && B == RockPaperScissors.Rock) {
                return 2;
            } else if (A == RockPaperScissors.Scissors && B == RockPaperScissors.Paper) {
                return 2;
            } else {
                return 0;
            }
        }

        public static int ScoreValue(this RockPaperScissors A) {
            return (int)A + 1;
        }

    }
}
