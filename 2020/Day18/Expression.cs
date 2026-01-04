using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Day18 {
	//Definitely not the best way to do this, but it works
	class Expression {

		private BigInteger result = 0;
		private List<Expression> parameters = new List<Expression>();
		private List<Operator> operators = new List<Operator>();

		private Expression() { }
		private Expression(int constant) {
			this.result = constant;
		}

		public BigInteger Calculate(bool additionFirst) {
			if (parameters.Count == 0 && operators.Count == 0) return result;

			while(operators.Count > 0) {
				Reduce(additionFirst);
			}
			if (parameters.Count != 1) throw new Exception("Invalid state: there is no result.");
			result = parameters[0].Calculate(additionFirst);
			parameters.Clear();
			return result;
		}

		private void Reduce(bool additionFirst) {
			int operatorIndex = 0;
			if (additionFirst) {
				operatorIndex = operators.IndexOf(Operator.Add);
				if (operatorIndex < 0) {
					operatorIndex = 0;
				}
			}
			int param1Index = operatorIndex;
			int param2Index = param1Index + 1;
			BigInteger result = operators[operatorIndex].Calculate(parameters[param1Index].Calculate(additionFirst), parameters[param2Index].Calculate(additionFirst));
			parameters.RemoveAt(param2Index);
			operators.RemoveAt(operatorIndex);
			parameters[param1Index].result = result;
		}

		public static Expression Parse(string input) {
			int index = 0;
			Expression exp = parseExpression(input, ref index);
			if (index < input.Length) throw new Exception("Parse ended too early.");
			return exp;
		}

		private static Expression parseExpression(string input, ref int index) {
			Expression exp = new Expression();
			exp.parameters.Add(parseParameter(input, ref index));
			while ((index < input.Length) && (input[index] != ')')) {
				exp.operators.Add(parseOperator(input, ref index));
				exp.parameters.Add(parseParameter(input, ref index));
			}
			return exp;
		}

		private static Expression parseParameter(string expression, ref int index) {
			skipWhitespace(expression, ref index);
			if (expression[index] == '(') {
				index++;
				Expression exp = parseExpression(expression, ref index);
				index++; //Move past the closing bracket ')'
				return exp;
			} else {
				int indexStart = index;
				while ((index < expression.Length) && char.IsNumber(expression[index])) {
					index++;
				}
				return new Expression(int.Parse(expression.Substring(indexStart, index - indexStart)));
			}
		}
		private static Operator parseOperator(string input, ref int index) {
			skipWhitespace(input, ref index);
			switch (input[index++]) {
				case '+': return Operator.Add;
				case '*': return Operator.Multiply;
				default: throw new Exception("Invalid operator.");
			}
		}

		private static void skipWhitespace(string expression, ref int index) {
			while (char.IsWhiteSpace(expression[index])) {
				index++;
			}
		}

	}
}
