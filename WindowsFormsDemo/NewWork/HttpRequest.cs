using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWork
{
    /// <summary>
    /// Http请求的抽象基类
    /// </summary>
    public abstract class HttpRequest
    {
        public abstract void StartRequestWithType(string postData, int httpTag, IResultsHandler client);
    }
}
