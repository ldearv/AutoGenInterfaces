using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Helper;
using System.Windows.Forms;
using NetWork;

namespace WindowsFormsApp1
{
    /// <summary>
    /// 
    /// </summary>
    class NetRequest : HttpRequest
    {
        /// <summary>
        /// 
        /// </summary>
        private NetOperation NetOperation;

        /// <summary>
        /// 
        /// </summary>
        public NetRequest()
        {
            NetOperation = new NetOperation();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="httpTag"></param>
        public override void StartRequestWithType(string postData, string httpTag, IResultsHandler client) {
            //url
            string urlString = string.Format("{0}{1}{2}{3}",
                                             NetConfig.V_BASEURL, "{0}",
                                             NetConfig.DEBUG_PLACE,
                                             NetConfig.MD5_PLACE);

            switch (httpTag) {
				case NetTag.Tag_Common_noparam1: //C1 【C1】无参数测试接口1 返回data是字符串
					urlString = string.Format(urlString, "Common/noparam1");
					break;

				case NetTag.Tag_Common_noparam2: //C2 【C2】无参数测试接口2 返回data是字符串数组
					urlString = string.Format(urlString, "Common/noparam2");
					break;

				case NetTag.Tag_Common_noparam3: //C3 【C3】无参数测试接口3 返回data是对象
					urlString = string.Format(urlString, "Common/noparam3");
					break;

				case NetTag.Tag_Common_noparam4: //C4 【C4】无参数测试接口4 返回data是对象数组
					urlString = string.Format(urlString, "Common/noparam4");
					break;

				case NetTag.Tag_Common_hasparam1: //C5 【C5】有参数测试接1 参数是字符串
					urlString = string.Format(urlString, "Common/hasparam1");
					break;

				case NetTag.Tag_Common_hasparam2: //C6 【C6】有参数测试接2 参数是字符串数组
					urlString = string.Format(urlString, "Common/hasparam2");
					break;

				case NetTag.Tag_Common_hasparam3: //C7 【C7】有参数测试接3 参数是字典
					urlString = string.Format(urlString, "Common/hasparam3");
					break;

				case NetTag.Tag_Common_hasparam4: //C8 【C8】有参数测试接4 参数是多个字符串
					urlString = string.Format(urlString, "Common/hasparam4");
					break;
			}
            //md5
            string md5 = MD5Helper.ToMD5(string.Format("{0}{1}", postData, NetConfig.MD5_KEY));

            NetOperation.Excutor = client;
            Action<string, string, int> fun = NetOperation.GetWebContent;
            fun.BeginInvoke(urlString + md5, postData, httpTag, FunCallback, fun);
          
        }

        public void FunCallback(IAsyncResult ar) {
            //(ar.AsyncState as Action<string, string, string>).EndInvoke(ar);
        }
    }
}