using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_02 {
    internal struct StrategyGuideEntry : IObjectParser<string[]> {

        public RockPaperScissors Opponent;
        public RockPaperScissors Choice; // Used in part 1
        public int Result; // Used in part 2


        public void Parse(string[] input) {
            if (input.Length != 2)
                throw new ArgumentException("Invalid number of inputs.");

            switch (input[0]) {
                case "A":
                    Opponent = RockPaperScissors.Rock;
                    break;
                case "B":
                    Opponent = RockPaperScissors.Paper;
                    break;
                case "C":
                    Opponent = RockPaperScissors.Scissors;
                    break;
                default:
                    throw new ArgumentException("Invalid first argument.");
            }

            switch (input[1]) {
                case "X":
                    Choice = RockPaperScissors.Rock;
                    Result = 0; // Lose
                    break;
                case "Y":
                    Choice = RockPaperScissors.Paper;
                    Result = 1; // Draw
                    break;
                case "Z":
                    Choice = RockPaperScissors.Scissors;
                    Result = 2; // Win
                    break;
                default:
                    throw new ArgumentException("Invalid second argument.");
            }
        }

        public int Score() {
            return Choice.ScoreValue() + (Choice.Beats(Opponent) * 3);
        }

        // Part 2
        public StrategyGuideEntry ChangeChoiceToGetResult() {
            if (Result == 1) {
                // Draw
                Choice = Opponent;
            } else if (Result == 0) {
                // Lose
                switch (Opponent) {
                    case RockPaperScissors.Rock:
                        Choice = RockPaperScissors.Scissors;
                        break;
                    case RockPaperScissors.Paper:
                        Choice = RockPaperScissors.Rock;
                        break;
                    case RockPaperScissors.Scissors:
                        Choice = RockPaperScissors.Paper;
                        break;
                }
            } else if (Result == 2) {
                switch (Opponent) {
                    case RockPaperScissors.Rock:
                        Choice = RockPaperScissors.Paper;
                        break;
                    case RockPaperScissors.Paper:
                        Choice = RockPaperScissors.Scissors;
                        break;
                    case RockPaperScissors.Scissors:
                        Choice = RockPaperScissors.Rock;
                        break;
                }
            }

            return this;
        }
    }
}
