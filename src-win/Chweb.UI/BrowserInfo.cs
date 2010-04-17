using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Chweb.UI
{
    public class BrowserInfo
    {
        public string Path { get; set; }

        public string FileName
        {
            get { return new FileInfo(Path).Name; }
        }

        public BrowserInfo(string path)
        {
            this.Path = path;
        }
    }
}
