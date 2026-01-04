using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_07 {
    public interface IFileSystem {

        public long Size { get; }
        public IEnumerable<IFileSystem> Children { get; }
        public Directory Parent { get; set; }

    }
}
