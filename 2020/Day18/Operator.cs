using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Day18 {
	enum Operator {
	
		Add,
		Multiply

	}

	static class OperatorExtentions {
		
		public static BigInteger Calculate(this Operator op, BigInteger left, BigInteger right) {
			switch (op) {
				case Operator.Add: return left + right;
				case Operator.Multiply: return left * right;
				default: throw new ArgumentException("Invalid operation", "op");
			}
		}

	}
}
