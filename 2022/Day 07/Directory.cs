using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_07 {
    public class Directory : IFileSystem, IObjectParser<IEnumerable<string[]>> {
        public long Size => totalSize;
        public IEnumerable<IFileSystem> Children => files.Values;
        public Directory Parent { get; set; } = null;
        

        private Dictionary<string, IFileSystem> files = new();
        private long totalSize;

        private void AddSize(long size) {
            this.totalSize += size;
            this.Parent?.AddSize(size);
        }

        public IFileSystem this[string name] {
            get => files[name];
            set {
                if (files.ContainsKey(name))
                    throw new Exception("Name collision.");

                files[name] = value;
                if (value != null) {
                    value.Parent = this;
                    if (value is File file) {
                        this.AddSize(file.Size);
                    }
                }
            }
        }

        public void Parse(IEnumerable<string[]> input) {
            Directory currentDir = this;
            bool listingFiles = false;

            foreach (string[] line in input) {
                if (line[0] == "$") {
                    // new command
                    listingFiles = false;

                    if (line[1] == "cd") {
                        if (line[2] == "/") {
                            currentDir = this;
                        } else if (line[2] == "..") {
                            currentDir = currentDir.Parent;
                        } else {
                            IFileSystem subfile = currentDir[line[2]];
                            if (subfile != null && subfile is Directory subdir) {
                                currentDir = subdir;
                            } else {
                                throw new Exception("Could not find subdirectory.");
                            }
                        }
                    } else if (line[1] == "ls") {
                        listingFiles = true;
                    } else {
                        throw new Exception("Invalid command.");
                    }
                } else {
                    if (listingFiles) {
                        if (line[0] == "dir") {
                            currentDir[line[1]] = new Directory();
                        } else {
                            currentDir[line[1]] = new File(int.Parse(line[0]));
                        }
                    } else {
                        throw new Exception("File name not expected.");
                    }
                }
            }
        }
    }
}
