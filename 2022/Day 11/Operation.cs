using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_11 {
    internal class Operation {

        // Only add or muliply
        bool IsAdd;
        bool ArgIsOld; // true means second arg is "old" value, otherwise a const int arg
        int Arg;

        private Operation() { }

        public long Calculate(long old) {
            long arg2 = old;
            if (!ArgIsOld)
                arg2 = this.Arg;

            if (IsAdd) {
                return old + arg2;
            } else {
                return old * arg2;
            }
        }

        internal static Operation Parse(string line) {
            if (!line.StartsWith("new = old "))
                throw new ArgumentException("Bad input.");

            Operation result = new Operation();

            char op = line["new = old ".Length];
            result.IsAdd = (op == '+');

            string arg2 = line.Substring("new = old + ".Length);
            if (arg2 == "old") {
                result.ArgIsOld = true;
            } else {
                result.ArgIsOld = false;
                result.Arg = int.Parse(arg2);
            }

            return result;
        }

    }
}
