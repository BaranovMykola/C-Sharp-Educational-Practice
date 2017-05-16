using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOSystem
{
    [Serializable]
    public class FileInfoWrapper
    {
        #region Fields
        FileInfo information;
        private string fileName;
        #endregion

        #region Propeties
        public string FileName
        {
            get { return fileName; }
            set
            {
                information = new FileInfo(value);
                fileName = value;
            }
        }
        public long Size => information.Length;
        public FileAttributes Atributes => information.Attributes;
        public DateTime CreationTime => information.CreationTime;
        public string Extension => information.Extension;
        public string FullName => information.FullName;
        public string Directory => information.Directory.Name;
        #endregion

        #region Constructors
        public FileInfoWrapper()
        {

        }
        public FileInfoWrapper(string file)
        {
            FileName = file;
        }
        #endregion

        #region Methods
        public override string ToString() => string.Format("{0,-20}{1,40}\n" +
            "{2,-20}{3,40}\n" +
            "{4,-20}{5,40}\n" +
            "{6,-20}{7,40}\n" +
            "{8,-20}{9,40}\n",
            "Size", Size, "Creation Time", CreationTime, "Extension", Extension, "Atributes", Atributes, "Directory", Directory);
        #endregion

    }
}
