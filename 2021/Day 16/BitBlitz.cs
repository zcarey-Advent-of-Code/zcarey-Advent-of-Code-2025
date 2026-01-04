using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_16 {
    // The name has no meaning other than I thought it sounded cool.
    public class BitBlitz : IObjectParser<IEnumerable<byte>>{

        private IEnumerator<byte> stream;
        private byte data;
        private int bitsLeft = 0;

        public void Parse(IEnumerable<byte> input) {
            stream = input.GetEnumerator();
        }

        public int Take(int bits) {
            if (bits > 31) throw new OverflowException("Oops");

            int value = 0;
            while(bits > 0) {
                if (bitsLeft == 0) {
                    // Load more bits
                    if (!stream.MoveNext()) throw new EndOfStreamException("Oops");
                    data = stream.Current;
                    bitsLeft = 8;
                }

                if (bits >= bitsLeft) {
                    // Just take all the bits we got
                    value <<= bitsLeft;
                    int bitShift = 8 - bitsLeft;
                    value |= (data >> bitShift);

                    bits -= bitsLeft;
                    // Dont need to change data, bitsLeft = 0 so variable is in an invalid/unusable state
                    bitsLeft = 0;
                } else {
                    // We only need a few bits, so bits is always at least <= 8
                    value <<= bits;
                    int bitShift = 8 - bits;
                    value |= (data >> bitShift);

                    bitsLeft -= bits;
                    data <<= bits;
                    break;
                }
            }

            return value;
        }

        public bool GetBool() {
            return Take(1) == 1;
        }

    }
}
