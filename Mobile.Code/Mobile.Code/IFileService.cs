﻿using System.IO;

namespace Mobile.Code
{
    public interface IFileService
    {
        string SavePicture(string name, Stream data, string location = "temp");
    }
}
