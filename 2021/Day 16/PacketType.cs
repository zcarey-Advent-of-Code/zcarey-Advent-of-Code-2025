using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_16 {
    public enum PacketType {

        // their value is the sum of the values of their sub-packets.
        Sum = 0,

        // their value is the result of multiplying together the values of their sub-packets.
        Product = 1,

        // their value is the minimum of the values of their sub-packets.
        Minimum = 2,

        // their value is the maximum of the values of their sub-packets.
        Maximum = 3,

        // Literal value packets encode a single binary number.
        // To do this, the binary number is padded with leading zeroes until its length is a multiple of four bits,
        // and then it is broken into groups of four bits.
        // Each group is prefixed by a 1 bit except the last group, which is prefixed by a 0 bit.
        // These groups of five bits immediately follow the packet header.
        LiteralValue = 4,

        // their value is 1 if the value of the first sub-packet is greater than the value of the second sub-packet;
        // otherwise, their value is 0.
        // These packets always have exactly two sub-packets.
        GreaterThan = 5,

        // their value is 1 if the value of the first sub-packet is less than the value of the second sub-packet;
        // otherwise, their value is 0.
        // These packets always have exactly two sub-packets.
        LessThan = 6,

        // their value is 1 if the value of the first sub-packet is equal to the value of the second sub-packet;
        // otherwise, their value is 0.
        // These packets always have exactly two sub-packets.
        EqualTo = 7
    }
}
