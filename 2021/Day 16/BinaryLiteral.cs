using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_16 {
    public class BinaryLiteral {

        public int Bits { get; private set; } = 0;
        public int Value { get; private set; } = 0;

        public BinaryLiteral() { }

        public void Append(bool bit) {
            Value = (Value << 1) | (bit ? 1 : 0);
            Bits++;
        }

        // Used for parsing LiteralValue packets
        public static BinaryLiteral Parse(BitBlitz bits, out int bitsRead) {
            BinaryLiteral value = new();
            bitsRead = 0;
            bool morePackets = true;
            do {
                // Each group is prefixed by a 1 bit except the last group, which is prefixed by a 0 bit.
                morePackets = bits.GetBool();

                // and then it is broken into groups of four bits.
                for (int i = 0; i < 4; i++) {
                    value.Append(bits.GetBool());
                }

                bitsRead += 5;
            } while (morePackets);

            return value;
        }

    }
}
