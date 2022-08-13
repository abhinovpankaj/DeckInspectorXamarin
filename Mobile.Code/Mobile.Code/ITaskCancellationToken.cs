using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Mobile.Code
{
    public interface ITaskCancellationToken
    { 
        CancellationToken CancelToken { get; set; }
        void SetCancellationToken(CancellationToken token);
    }
}
