using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOSystem
{
    class FileInfoWrapper
    {
        FileInfo information;
        public long Size => information.Length;
        public FileAttributes Atributes => information.Attributes;
        public DateTime CreationTime => information.CreationTime;
        public string Extension => information.Extension;
        public string FullName => information.FullName;
        public string Directory => information.Directory.Name;

        public FileInfoWrapper(string fileName)
        {
            information = new FileInfo(fileName);
        }

        public override string ToString() => string.Format("{0,-20}{1,40}\n" +
            "{2,-20}{3,40}\n" +
            "{4,-20}{5,40}\n" +
            "{6,-20}{7,40}\n" +
            "{8,-20}{9,40}\n",
            "Size", Size, "Creation Time", CreationTime, "Extension", Extension, "Atributes", Atributes, "Directory", Directory);

    }
}
