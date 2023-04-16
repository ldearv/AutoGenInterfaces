using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWork;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
	public class NetDemo:Form,IResultsHandler
	{
		public void TestDemo()
		{
			// 【C1】无参数测试接口1 返回data是字符串
			Common_noparam1_Post_Model_C1 pd_C1 = new Common_noparam1_Post_Model_C1();
			NetAPIs.Common_noparam1(pd_C1, this);

			// 【C2】无参数测试接口2 返回data是字符串数组
			Common_noparam2_Post_Model_C2 pd_C2 = new Common_noparam2_Post_Model_C2();
			NetAPIs.Common_noparam2(pd_C2, this);

			// 【C3】无参数测试接口3 返回data是对象
			Common_noparam3_Post_Model_C3 pd_C3 = new Common_noparam3_Post_Model_C3();
			NetAPIs.Common_noparam3(pd_C3, this);

			// 【C4】无参数测试接口4 返回data是对象数组
			Common_noparam4_Post_Model_C4 pd_C4 = new Common_noparam4_Post_Model_C4();
			NetAPIs.Common_noparam4(pd_C4, this);

			// 【C5】有参数测试接1 参数是字符串
			Common_hasparam1_Post_Model_C5 pd_C5 = new Common_hasparam1_Post_Model_C5();
			NetAPIs.Common_hasparam1(pd_C5, this);

			// 【C6】有参数测试接2 参数是字符串数组
			Common_hasparam2_Post_Model_C6 pd_C6 = new Common_hasparam2_Post_Model_C6();
			NetAPIs.Common_hasparam2(pd_C6, this);

			// 【C7】有参数测试接3 参数是字典
			Common_hasparam3_Post_Model_C7 pd_C7 = new Common_hasparam3_Post_Model_C7();
			NetAPIs.Common_hasparam3(pd_C7, this);

			// 【C8】有参数测试接4 参数是多个字符串
			Common_hasparam4_Post_Model_C8 pd_C8 = new Common_hasparam4_Post_Model_C8();
			NetAPIs.Common_hasparam4(pd_C8, this);

		}
		#region 网络
		public void RequestSuccessed(string httptag, string results)
		{
			//if (!this.IsHandleCreated)
			//{
			//	return;
			//}
			Console.WriteLine("成功接口编号为:" + httptag + "    内容为:" + results);

			switch (httptag)
			{
				#region 【C1】无参数测试接口1 返回data是字符串
				case NetTag.Tag_Common_noparam1:
				{
					Common_noparam1_Return_Model_C1 returnData = JsonConvert.DeserializeObject<Common_noparam1_Return_Model_C1>(results);
				}
					break;
				#endregion 【C1】无参数测试接口1 返回data是字符串

				#region 【C2】无参数测试接口2 返回data是字符串数组
				case NetTag.Tag_Common_noparam2:
				{
					Common_noparam2_Return_Model_C2 returnData = JsonConvert.DeserializeObject<Common_noparam2_Return_Model_C2>(results);
				}
					break;
				#endregion 【C2】无参数测试接口2 返回data是字符串数组

				#region 【C3】无参数测试接口3 返回data是对象
				case NetTag.Tag_Common_noparam3:
				{
					Common_noparam3_Return_Model_C3 returnData = JsonConvert.DeserializeObject<Common_noparam3_Return_Model_C3>(results);
				}
					break;
				#endregion 【C3】无参数测试接口3 返回data是对象

				#region 【C4】无参数测试接口4 返回data是对象数组
				case NetTag.Tag_Common_noparam4:
				{
					Common_noparam4_Return_Model_C4 returnData = JsonConvert.DeserializeObject<Common_noparam4_Return_Model_C4>(results);
				}
					break;
				#endregion 【C4】无参数测试接口4 返回data是对象数组

				#region 【C5】有参数测试接1 参数是字符串
				case NetTag.Tag_Common_hasparam1:
				{
					Common_hasparam1_Return_Model_C5 returnData = JsonConvert.DeserializeObject<Common_hasparam1_Return_Model_C5>(results);
				}
					break;
				#endregion 【C5】有参数测试接1 参数是字符串

				#region 【C6】有参数测试接2 参数是字符串数组
				case NetTag.Tag_Common_hasparam2:
				{
					Common_hasparam2_Return_Model_C6 returnData = JsonConvert.DeserializeObject<Common_hasparam2_Return_Model_C6>(results);
				}
					break;
				#endregion 【C6】有参数测试接2 参数是字符串数组

				#region 【C7】有参数测试接3 参数是字典
				case NetTag.Tag_Common_hasparam3:
				{
					Common_hasparam3_Return_Model_C7 returnData = JsonConvert.DeserializeObject<Common_hasparam3_Return_Model_C7>(results);
				}
					break;
				#endregion 【C7】有参数测试接3 参数是字典

				#region 【C8】有参数测试接4 参数是多个字符串
				case NetTag.Tag_Common_hasparam4:
				{
					Common_hasparam4_Return_Model_C8 returnData = JsonConvert.DeserializeObject<Common_hasparam4_Return_Model_C8>(results);
				}
					break;
				#endregion 【C8】有参数测试接4 参数是多个字符串

			}
		}


		public void RequestError(string httptag, string results)
		{
			//if (!this.IsHandleCreated)
			//{
			//	return;
			//}
			Console.WriteLine("Error接口编号为:" + httptag+"    内容为:"+results);

			switch (httptag)
			{
				#region 【C1】无参数测试接口1 返回data是字符串
				case NetTag.Tag_Common_noparam1:
				{
				}
					break;
				#endregion 【C1】无参数测试接口1 返回data是字符串

				#region 【C2】无参数测试接口2 返回data是字符串数组
				case NetTag.Tag_Common_noparam2:
				{
				}
					break;
				#endregion 【C2】无参数测试接口2 返回data是字符串数组

				#region 【C3】无参数测试接口3 返回data是对象
				case NetTag.Tag_Common_noparam3:
				{
				}
					break;
				#endregion 【C3】无参数测试接口3 返回data是对象

				#region 【C4】无参数测试接口4 返回data是对象数组
				case NetTag.Tag_Common_noparam4:
				{
				}
					break;
				#endregion 【C4】无参数测试接口4 返回data是对象数组

				#region 【C5】有参数测试接1 参数是字符串
				case NetTag.Tag_Common_hasparam1:
				{
				}
					break;
				#endregion 【C5】有参数测试接1 参数是字符串

				#region 【C6】有参数测试接2 参数是字符串数组
				case NetTag.Tag_Common_hasparam2:
				{
				}
					break;
				#endregion 【C6】有参数测试接2 参数是字符串数组

				#region 【C7】有参数测试接3 参数是字典
				case NetTag.Tag_Common_hasparam3:
				{
				}
					break;
				#endregion 【C7】有参数测试接3 参数是字典

				#region 【C8】有参数测试接4 参数是多个字符串
				case NetTag.Tag_Common_hasparam4:
				{
				}
					break;
				#endregion 【C8】有参数测试接4 参数是多个字符串

			}
		}


		public void RequestFailed(string httptag, int type)
		{
			//if (!this.IsHandleCreated)
			//{
			//	return;
			//}
			Console.WriteLine("Failed接口编号为:" + httptag);

			switch (httptag)
			{
				#region 【C1】无参数测试接口1 返回data是字符串
				case NetTag.Tag_Common_noparam1:
				{
					System.Console.WriteLine("联网失败:" + "C1");
				}
					break;
				#endregion 【C1】无参数测试接口1 返回data是字符串

				#region 【C2】无参数测试接口2 返回data是字符串数组
				case NetTag.Tag_Common_noparam2:
				{
					System.Console.WriteLine("联网失败:" + "C2");
				}
					break;
				#endregion 【C2】无参数测试接口2 返回data是字符串数组

				#region 【C3】无参数测试接口3 返回data是对象
				case NetTag.Tag_Common_noparam3:
				{
					System.Console.WriteLine("联网失败:" + "C3");
				}
					break;
				#endregion 【C3】无参数测试接口3 返回data是对象

				#region 【C4】无参数测试接口4 返回data是对象数组
				case NetTag.Tag_Common_noparam4:
				{
					System.Console.WriteLine("联网失败:" + "C4");
				}
					break;
				#endregion 【C4】无参数测试接口4 返回data是对象数组

				#region 【C5】有参数测试接1 参数是字符串
				case NetTag.Tag_Common_hasparam1:
				{
					System.Console.WriteLine("联网失败:" + "C5");
				}
					break;
				#endregion 【C5】有参数测试接1 参数是字符串

				#region 【C6】有参数测试接2 参数是字符串数组
				case NetTag.Tag_Common_hasparam2:
				{
					System.Console.WriteLine("联网失败:" + "C6");
				}
					break;
				#endregion 【C6】有参数测试接2 参数是字符串数组

				#region 【C7】有参数测试接3 参数是字典
				case NetTag.Tag_Common_hasparam3:
				{
					System.Console.WriteLine("联网失败:" + "C7");
				}
					break;
				#endregion 【C7】有参数测试接3 参数是字典

				#region 【C8】有参数测试接4 参数是多个字符串
				case NetTag.Tag_Common_hasparam4:
				{
					System.Console.WriteLine("联网失败:" + "C8");
				}
					break;
				#endregion 【C8】有参数测试接4 参数是多个字符串

			}
		}

		#endregion 网络
	}
}