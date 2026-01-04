using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_16 {
    internal class Program : ProgramStructure<Packet> {

        Program() : base(new Parser()
            .Parse(new StringReader())
            .Filter(s => Enumerable.Range(0, s.Length).Where(x => x % 2 == 0).Select(x => s.Substring(x, 2))) // Return groups of 2 hex characters
            .Filter(x => Convert.ToByte(x, 16)) // Convert 2 hex characters into a byte
            .Create<BitBlitz>()
            .Create<Packet>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
            //new Program().Run(args, "Example2.txt");
        }

        protected override object SolvePart1(Packet input) {
            return SumVersionNumbers(input);
        }

        protected override object SolvePart2(Packet input) {
/*            List<string> Examples = new() {
                "C200B40A82",
                "04005AC33890",
                "880086C3E88112",
                "CE00C43D881120",
                "D8005AC2A8F0",
                "F600BC2D8F",
                "9C005AC2F8F0",
                "9C0141080250320F1802104A08"
            };

            foreach(string example in Examples) {
                BitBlitz bits = new();
                bits.Parse(
                    Enumerable.Range(0, example.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => example.Substring(x, 2))
                    .Select(x => Convert.ToByte(x, 16))
                );

                Packet packet = new Packet();
                packet.Parse(bits);
                Console.WriteLine(packet.CalculateValue());
            }
            Console.WriteLine();
*/
            return input.CalculateValue();
        }

        private static long SumVersionNumbers(Packet packet) {
            return packet.Version + packet.SubPackets.Select(x => SumVersionNumbers(x)).Sum();
        }

    }
}
