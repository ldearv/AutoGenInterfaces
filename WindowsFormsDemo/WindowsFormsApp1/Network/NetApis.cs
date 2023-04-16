using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using NetWork;
using Newtonsoft.Json;

namespace WindowsFormsApp1
{
	/// <summary>
	/// 网络接口类
	/// </summary>
	public class NetAPIs
	{
		/// <summary>
		/// 【C1】无参数测试接口1 返回data是字符串 C1
		/// 
		/// 返回状态码
		/// R.200 OK
		/// </summary>
		public static void Common_noparam1(Common_noparam1_Post_Model_C1 postData, IResultsHandler client)
		{
			NetRequest request = new NetRequest();
			string postDataStr = JsonConvert.SerializeObject(postData);
			request.StartRequestWithType(postDataStr, NetTag.Tag_Common_noparam1, client);
		}

		/// <summary>
		/// 【C2】无参数测试接口2 返回data是字符串数组 C2
		/// 
		/// 返回状态码
		/// R.200 OK
		/// </summary>
		public static void Common_noparam2(Common_noparam2_Post_Model_C2 postData, IResultsHandler client)
		{
			NetRequest request = new NetRequest();
			string postDataStr = JsonConvert.SerializeObject(postData);
			request.StartRequestWithType(postDataStr, NetTag.Tag_Common_noparam2, client);
		}

		/// <summary>
		/// 【C3】无参数测试接口3 返回data是对象 C3
		/// 
		/// 返回状态码
		/// R.200 OK
		/// </summary>
		public static void Common_noparam3(Common_noparam3_Post_Model_C3 postData, IResultsHandler client)
		{
			NetRequest request = new NetRequest();
			string postDataStr = JsonConvert.SerializeObject(postData);
			request.StartRequestWithType(postDataStr, NetTag.Tag_Common_noparam3, client);
		}

		/// <summary>
		/// 【C4】无参数测试接口4 返回data是对象数组 C4
		/// 
		/// 返回状态码
		/// R.200 OK
		/// </summary>
		public static void Common_noparam4(Common_noparam4_Post_Model_C4 postData, IResultsHandler client)
		{
			NetRequest request = new NetRequest();
			string postDataStr = JsonConvert.SerializeObject(postData);
			request.StartRequestWithType(postDataStr, NetTag.Tag_Common_noparam4, client);
		}

		/// <summary>
		/// 【C5】有参数测试接1 参数是字符串 C5
		/// 
		/// 返回状态码
		/// R.200 OK
		/// </summary>
		public static void Common_hasparam1(Common_hasparam1_Post_Model_C5 postData, IResultsHandler client)
		{
			NetRequest request = new NetRequest();
			string postDataStr = JsonConvert.SerializeObject(postData);
			request.StartRequestWithType(postDataStr, NetTag.Tag_Common_hasparam1, client);
		}

		/// <summary>
		/// 【C6】有参数测试接2 参数是字符串数组 C6
		/// 
		/// 返回状态码
		/// R.200 OK
		/// </summary>
		public static void Common_hasparam2(Common_hasparam2_Post_Model_C6 postData, IResultsHandler client)
		{
			NetRequest request = new NetRequest();
			string postDataStr = JsonConvert.SerializeObject(postData);
			request.StartRequestWithType(postDataStr, NetTag.Tag_Common_hasparam2, client);
		}

		/// <summary>
		/// 【C7】有参数测试接3 参数是字典 C7
		/// 
		/// 返回状态码
		/// R.200 OK
		/// </summary>
		public static void Common_hasparam3(Common_hasparam3_Post_Model_C7 postData, IResultsHandler client)
		{
			NetRequest request = new NetRequest();
			string postDataStr = JsonConvert.SerializeObject(postData);
			request.StartRequestWithType(postDataStr, NetTag.Tag_Common_hasparam3, client);
		}

		/// <summary>
		/// 【C8】有参数测试接4 参数是多个字符串 C8
		/// 
		/// 返回状态码
		/// R.200 OK
		/// </summary>
		public static void Common_hasparam4(Common_hasparam4_Post_Model_C8 postData, IResultsHandler client)
		{
			NetRequest request = new NetRequest();
			string postDataStr = JsonConvert.SerializeObject(postData);
			request.StartRequestWithType(postDataStr, NetTag.Tag_Common_hasparam4, client);
		}


	}
}