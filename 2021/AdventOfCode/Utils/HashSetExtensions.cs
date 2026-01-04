using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils {
    public static class HashSetExtensions {

        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> items) {
            foreach(T item in items) {
                set.Add(item);
            }
        }

    }
}
