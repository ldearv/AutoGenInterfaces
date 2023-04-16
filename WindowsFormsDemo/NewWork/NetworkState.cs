using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWork {
    /// <summary>
    /// 网络状态
    /// </summary>
    public enum NetworkState {
        /// <summary>
        /// 网络未连接，检查网络设置
        /// </summary>
        NotConnected = 0,
        /// <summary>
        /// 网络异常，未能接入互联网
        /// </summary>
        Anomaly = 1,
        /// <summary>
        /// 网络请求超时，请稍后重试
        /// </summary>
        TimeOut = 2
    }
}
