using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWork {
    /// <summary>
    /// IVGResultsHandler(处理网络返回值接口)
    /// </summary>
    public interface IResultsHandler
    {
        /// <summary>
        /// 网络请求成功返回，SUC 代码
        /// </summary>
        /// <param name="httptag">请求类型</param>
        /// <param name="results">服务器返回的结果</param>
        void RequestSuccessed(int httptag, string results);

        /// <summary>
        /// 网络请求成功返回，ERR 代码
        /// </summary>
        /// <param name="httptag">请求类型</param>
        /// <param name="type">失败类型</param>
        void RequestError(int httptag, string results);

        /// <summary>
        /// 网络请求失败,0 未连接 1 网络异常 2 连接超时
        /// </summary>
        /// <param name="httptag">请求类型</param>
        /// <param name="type">失败类型</param>
        void RequestFailed(int httptag, int type);
    }
}
