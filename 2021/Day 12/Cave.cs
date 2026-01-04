using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_12 {
    public class Cave {

        public List<Cave> Connections = new();

        public readonly bool IsBigCave;

        // Only set to true for small caves
        private bool traversed = false;
        public bool Traversed { 
            get => traversed; 
            set {
                if (!IsBigCave) {
                    traversed = value;
                } else {
                    if(value == false) {
                        traversed = false;
                    }
                }
            }
        }

        public Cave(bool IsBig, string? debug = null) {
            IsBigCave = IsBig;
            Debug = debug;
        }

        public readonly string? Debug;

        public override string ToString() {
            return Debug;
        }

    }
}
