using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_07 {
    public class File : IFileSystem {
        public long Size { get; private set; }

        public IEnumerable<IFileSystem> Children => new List<IFileSystem>();

        public Directory Parent { get; set; }

        public File(long size) {
            this.Size = size;
        }

    }
}
