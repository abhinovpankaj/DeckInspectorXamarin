﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mobile.Code
{
    public interface IScreenshotManager
    {
        Task<byte[]> CaptureAsync();
    }
}
