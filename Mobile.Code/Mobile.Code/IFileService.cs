using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mobile.Code
{
    public interface IFileService
    {
        string SavePicture(string name, Stream data, string location = "temp");
    }
}
