using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_21 {
    internal class Program : ProgramStructure<int[]> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .Filter(x => x.Substring("Player 1 starting position: ".Length))
            .Filter(int.Parse)
            .Filter(x => x - 1) // Internally is 0-9 while the actual board is 1-10
            .ToArray()
        ) { }

        static void Main(string[] args) {
            //new Program().Run(args);
            new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(int[] input) {
            int pos1 = input[0];
            int score1 = 0;

            int pos2 = input[1];
            int score2 = 0;

            int dice = -1;
            int rolls = 0;

            while (true) {
                // On each player's turn, the player rolls the die three times and adds up the results.
                int move1 = Roll(ref dice) + Roll(ref dice) + Roll(ref dice);
                rolls += 3;
                // Then, the player moves their pawn that many times forward around the track
                pos1 = (pos1 + move1) % 10;
                // After each player moves, they increase their score by the value of the space their pawn stopped on.
                // NOTE: our board is 0-9, so we add one to get the actual board's 1-10
                score1 += (pos1 + 1);
                if (score1 >= 1000) {
                    return score2 * rolls;
                }

                // Do the same for player 2
                int move2 = Roll(ref dice) + Roll(ref dice) + Roll(ref dice);
                rolls += 3;
                pos2 = (pos2 + move2) % 10;
                score2 += (pos2 + 1);
                if (score2 >= 1000) {
                    return score1 * rolls;
                }
            }
        }

        private static int Roll(ref int dice) {
            dice = (dice + 1) % 100;
            return dice + 1; // The variable goes from 0-99, but the dice rolls from 1-100
        }

        protected override object SolvePart2(int[] input) {
            State start = new(input[0], 0, input[1], 0, false);
            Dictionary<State, Tuple<long, long>> states = new();
            Tuple<long, long> wins = GetWins(states, start);
            return Math.Max(wins.Item1, wins.Item2);
        }

        private static Tuple<long, long> GetWins(Dictionary<State, Tuple<long, long>> states, State current) {
            Tuple<long, long> totalWins;

            // Try to find the stored result for this state
            if (states.TryGetValue(current, out totalWins)) {
                return totalWins;
            }

            // Check if this is a winning state
            if(current.Score1 >= 21) {
                return new Tuple<long, long>(1, 0);
            }else if(current.Score2 >= 21) {
                return new Tuple<long, long>(0, 1);
            }

            // Since this state hasn't been discoverd yet, find the result
            long player1Wins = 0;
            long player2Wins = 0;
            bool turn = current.Turn;
            int pos = (turn ? current.Position2 : current.Position1);
            int score = (turn ? current.Score2 : current.Score1);
            
            for(int i = 1; i <= 3; i++) {
                int newPos = (pos + i) % 10;
                int newScore = score + (newPos + 1); // NOTE: our board is 0-9, so we add one to get the actual board's 1-10
                State state = new(
                    turn ? current.Position1 : newPos,
                    turn ? current.Score1 : newScore,
                    turn ? newPos : current.Position2,
                    turn ? newScore : current.Score2,
                    !turn
                );
                Tuple<long, long> wins = GetWins(states, state);
                player1Wins += wins.Item1;
                player2Wins += wins.Item2;
            }           
            
            return new Tuple<long, long>(player1Wins, player2Wins);
        }
        

    }
}
