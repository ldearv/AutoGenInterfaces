using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NetWork {
    public static class NativeMethods {
        /// <summary>
        /// 检测网络是否连接
        /// </summary>
        /// <param name="Description"></param>
        /// <param name="ReservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet.dll")]
        public extern static bool InternetGetConnectedState(int Description, int ReservedValue);
    }
}
