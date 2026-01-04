using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_18 {
    public class SnailfishNumber : IObjectParser<string> {

        public int Value { get; private set; }
        private SnailfishNumber[] Elements = null;
        public SnailfishNumber Parent { get; private set; }

        public bool IsPair { get => this.Elements != null; }
        public bool IsValue { get => this.Elements == null; }

        public void Parse(string input) {
            IEnumerator<char> enumerator = input.GetEnumerator();
            Parse(enumerator);
        }

        private void Parse(IEnumerator<char> enumerator) {
            if (!enumerator.MoveNext()) throw new OverflowException();
            if (enumerator.Current == '[') {
                // Array type
                Elements = new SnailfishNumber[2];

                this[0] = new SnailfishNumber();
                this[0].Parse(enumerator);

                if (!enumerator.MoveNext()) throw new OverflowException();
                if (enumerator.Current != ',') throw new Exception("Bad parse");

                this[1] = new SnailfishNumber();
                this[1].Parse(enumerator);

                if (!enumerator.MoveNext()) throw new Exception();
                if (enumerator.Current != ']') throw new Exception("Bad parse");
            } else {
                // Value type
                Value = (enumerator.Current - '0');
            }
        }

        public SnailfishNumber this[int index] {
            get => Elements[index];
            set {
                if (IsPair && Elements[index] != null) {
                    Elements[index].Parent = null;
                }

                Elements[index] = value;

                if (value != null) {
                    value.Parent = this;
                }
            }
        }

        public static SnailfishNumber operator +(SnailfishNumber left, SnailfishNumber right) {
            SnailfishNumber result = new SnailfishNumber();
            result.Elements = new SnailfishNumber[2];
            result[0] = left;
            result[1] = right;
            left.Parent = result;
            right.Parent = result;
            result.Reduce();
            return result;
        }

        public SnailfishNumber GetRoot() {
            if (this.Parent != null) {
                return this.Parent.GetRoot();
            } else {
                return this;
            }
        }

        public void Reduce() {
            bool reduced = true;
            while (reduced) {
                reduced = false;

                while (true) {
                    bool exploded = ExplodeAll(0);
                    reduced |= exploded;
                    if (!exploded) {
                        break;
                    }
                }

                while (true) {
                    bool split = SplitAll();
                    reduced |= split;
                    if (!split) {
                        break;
                    }
                }
            }
        }

        public bool ExplodeAll(int depth) {
            if(this.IsPair) {
                if (depth >= 4 && this[0].IsValue && this[1].IsValue) {
                    Explode();
                    return true;
                } else {
                    bool exploded = false;
                    exploded |= this[0].ExplodeAll(depth + 1);
                    exploded |= this[1].ExplodeAll(depth + 1);
                    return exploded;
                }
            } else {
                return false;
            }
        }

        private void Explode() {
            // Only applies to pairs
            if (this.IsValue) throw new Exception("Pairs only, please");
            if (this[0].IsPair || this[1].IsPair) throw new Exception("Should only contain values!");

            SnailfishNumber leftNumber = SearchNumbersLeft().FirstOrDefault((SnailfishNumber)null);
            SnailfishNumber rightNumber = SearchNumbersRight().FirstOrDefault((SnailfishNumber)null);
            if(leftNumber != null) {
                leftNumber.Value += this[0].Value;
            }
            if(rightNumber != null) {
                rightNumber.Value += this[1].Value;
            }

            // EXPLODE!
            this[0] = null; // Sets parents to null
            this[1] = null; // Sets parents to null
            this.Elements = null;
            this.Value = 0;
        } 

        public bool SplitAll() {
            if (this.IsPair) {
                bool split = false;
                split |= this[0].SplitAll();
                split |= this[1].SplitAll();
                return split;
            } else {
                if (this.Value >= 10) {
                    Split();
                    return true;
                } else {
                    return false;
                }
            }
        }

        private void Split() {
            // Only applies to values
            if (this.IsPair) throw new Exception("Values only, please.");

            // Change this value into a pair
            this.Elements = new SnailfishNumber[2];

            this[0] = new SnailfishNumber();
            this[0].Value = this.Value / 2;

            this[1] = new SnailfishNumber();
            this[1].Value = this.Value - this[0].Value;

            this.Value = 0; // Reset just to be safe
        }

        public IEnumerable<SnailfishNumber> SearchNumbersLeft() {
            return SearchNumbers(true);
        }

        public IEnumerable<SnailfishNumber> SearchNumbersRight() {
            return SearchNumbers(false);
        }

        public IEnumerable<SnailfishNumber> SearchNumbers(bool SearchLeft) {
            SnailfishNumber root = this.GetRoot();
            bool foundTarget = false;
            return Search(SearchLeft, root, this, ref foundTarget);
        }

        private static IEnumerable<SnailfishNumber> Search(bool reverse, SnailfishNumber search, SnailfishNumber target, ref bool foundTarget) {
            if (search == target) {
                foundTarget = true;
                return Enumerable.Empty<SnailfishNumber>();
            }

            IEnumerable<SnailfishNumber> result = Enumerable.Empty<SnailfishNumber>();
            IEnumerable<SnailfishNumber> searchOrder = (reverse ? search.Reverse() : search.Elements);
            foreach (SnailfishNumber element in searchOrder) {
                if (element.IsValue) {
                    if (foundTarget) {
                        result = result.Concat(element);
                    }
                } else {
                    result = result.Concat(Search(reverse, element, target, ref foundTarget));
                }
            }

            return result;
        }

        private IEnumerable<SnailfishNumber> Reverse() {
            yield return this[1];
            yield return this[0];
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            if (this.IsValue) {
                sb.Append(this.Value);
            } else {
                sb.Append('[');
                sb.Append(this.Elements[0].ToString());
                sb.Append(',');
                sb.Append(this.Elements[1].ToString());
                sb.Append(']');
            }
            return sb.ToString();
        }
    }
}
