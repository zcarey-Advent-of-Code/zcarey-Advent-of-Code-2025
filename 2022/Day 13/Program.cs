using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_13 {
    internal class Program : ProgramStructure<IEnumerable<(PacketList Packet1, PacketList Packet2)>> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .Filter(new TextBlockFilter())
            .ForEach(
                // For each pair of packet strings
                new Parser<string[]>()
                .Parse( x => {
                    return (PacketList.Parse(x[0]), PacketList.Parse(x[1]));
                })
            )
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(IEnumerable<(PacketList Packet1, PacketList Packet2)> input) {
            int correctOrderSum = 0;
            foreach(var packetPair in input.WithIndex()) {
                int packetIndex = packetPair.Index + 1;
                PacketList packet1 = packetPair.Element.Packet1;
                PacketList packet2 = packetPair.Element.Packet2;
                if (PacketsInOrder(packet1, packet2)) {
                    correctOrderSum += packetIndex;
                }
            }
            
            return correctOrderSum;
        }

        private static bool PacketsInOrder(PacketList left, PacketList right) {
            return left.Compare(right) > 0;
        }

        protected override object SolvePart2(IEnumerable<(PacketList Packet1, PacketList Packet2)> input) {
            List<PacketList> packets = input.SelectMany(GetPacketEnumerable).ToList();
            packets.Add(new PacketList(new PacketList(new PacketValue(2))));
            packets.Add(new PacketList(new PacketList(new PacketValue(6))));
            packets.Sort((PacketList left, PacketList right) => -left.Compare(right));

            int dividerIndex2 = -1;
            int dividerIndex6 = -1;
            for(int i = 0; i < packets.Count; i++) {
                PacketList packet = packets[i];
                if (packet.Elements.Length == 1 && packet.Elements[0] is PacketList list) {
                    if (list.Elements.Length == 1 && list.Elements[0] is PacketValue value) {
                        if (value.Value == 2) {
                            dividerIndex2 = i;
                        } else if (value.Value == 6) {
                            dividerIndex6 = i;
                        }
                    }
                }
            }

            if (dividerIndex2 < 0 || dividerIndex6 < 0)
                throw new Exception("Could not find divider packets");

            return (dividerIndex2 + 1) * (dividerIndex6 + 1);
        }

        private static IEnumerable<PacketList> GetPacketEnumerable((PacketList Packet1, PacketList Packet2) input) {
            yield return input.Packet1;
            yield return input.Packet2;
        }

    }

    internal interface IPacketData {

        protected static IPacketData Parse(string input, ref int index) {
            if (input[index] == '[') {
                return PacketList.Parse(input, ref index);
            } else {
                return PacketValue.Parse(input, ref index);
            }
        }

        // <0 means wrong order, >0 right order, 0 means not sure
        public int Compare(IPacketData right);

    }

    internal struct PacketValue : IPacketData {

        public int Value;

        public PacketValue(int v) {
            this.Value = v;
        }

        internal static PacketValue Parse(string input, ref int index) {
            int startIndex = index;
            while (char.IsDigit(input[index])) {
                index++;
            }
            return new PacketValue(int.Parse(input.Substring(startIndex, index - startIndex)));
        }

        // <0 means wrong order, >0 right order, 0 means not sure
        public int Compare(IPacketData right) {
            if (right is PacketValue value) {
                return this.Compare(value);
            } else if (right is PacketList list) {
                return this.Compare(list);
            } else {
                throw new ArgumentException();
            }
        }

        // <0 means wrong order, >0 right order, 0 means not sure
        public int Compare(PacketValue right) {
            return right.Value - this.Value;
        }

        // <0 means wrong order, >0 right order, 0 means not sure
        public int Compare(PacketList right) {
            // TODO optimize
            return new PacketList(new IPacketData[] { this }).Compare(right);
        }

    }

    internal struct PacketList : IPacketData {

        public IPacketData[] Elements;

        public PacketList(params IPacketData[] elements) {
            Elements = elements;
        }

        public static PacketList Parse(string input) {
            int index = 0;
            return PacketList.Parse(input, ref index);
        }

        internal static PacketList Parse(string input, ref int index) {
            if (input[index] != '[') {
                throw new ArgumentException("Expected an open bracket.");
            }

            // Check for an empty array
            if (input[index + 1] == ']') {
                // Early exit
                index += 2;
                return new PacketList(new IPacketData[0]);
            }

            List<IPacketData> data = new();

            do {
                index++;
                data.Add(IPacketData.Parse(input, ref index));
            } while (input[index] == ',');

            if (input[index++] != ']') {
                throw new ArgumentException("Expected a close bracket.");
            }

            return new PacketList(data.ToArray());
        }

        // <0 means wrong order, >0 right order, 0 means not sure
        public int Compare(IPacketData right) {
            if (right is PacketValue value) {
                return this.Compare(value);
            } else if (right is PacketList list) {
                return this.Compare(list);
            } else {
                throw new ArgumentException();
            }
        }

        // <0 means wrong order, >0 right order, 0 means not sure
        public int Compare(PacketList right) {
            int len = Math.Min(this.Elements.Length, right.Elements.Length);
            for (int i = 0; i < len; i++) {
                int result = this.Elements[i].Compare(right.Elements[i]);
                if (result != 0) {
                    return result;
                }
            }

            if (this.Elements.Length < right.Elements.Length) {
                return 1;
            } else if (this.Elements.Length > right.Elements.Length) {
                return -1;
            } else {
                return 0;
            }
        }

        // <0 means wrong order, >0 right order, 0 means not sure
        public int Compare(PacketValue right) {
            // TODO optimize
            return this.Compare(new PacketList(new IPacketData[] { right }));
        }
    }

}
