using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApp1
{
	public class OnlyResult_Return_Model
	{
		public List<string> Result { get; set; }
	}

	public class Common_noparam1_Post_Model_C1
	{
	}

	public class Common_noparam1_Return_Model_C1
	{
		public string data { get; set; }

		public string code { get; set; }
		public string msg { get; set; }
	}

	public class Common_noparam2_Post_Model_C2
	{
	}

	public class data_Model_C2
	{
		public string  { get; set; }
	}

	public class Common_noparam2_Return_Model_C2
	{
		public List<data_Model_C2> data { get; set; }

		public string code { get; set; }
		public string msg { get; set; }
	}

	public class Common_noparam3_Post_Model_C3
	{
	}

	public class data_Model_C3
	{
		public string name { get; set; } // 名字
		public string age { get; set; } // 年龄
	}

	public class Common_noparam3_Return_Model_C3
	{
		public data_Model_C3 data { get; set; }

		public string code { get; set; }
		public string msg { get; set; }
	}

	public class Common_noparam4_Post_Model_C4
	{
	}

	public class data_Model_C4
	{
		public string name { get; set; } // 名字
		public string age { get; set; } // 年龄
	}

	public class Common_noparam4_Return_Model_C4
	{
		public List<data_Model_C4> data { get; set; }

		public string code { get; set; }
		public string msg { get; set; }
	}

	public class Common_hasparam1_Post_Model_C5
	{
		public string param { get; set; }
	}

	public class data_Model_C5
	{
		public string  { get; set; }
	}

	public class Common_hasparam1_Return_Model_C5
	{
		public List<data_Model_C5> data { get; set; }

		public string code { get; set; }
		public string msg { get; set; }
	}

	public class params_Model_C6
	{
		public string  { get; set; }
	}

	public class Common_hasparam2_Post_Model_C6
	{
		public List<params_Model_C6> params { get; set; }
	}

	public class Common_hasparam2_Return_Model_C6
	{
		public string data { get; set; }

		public string code { get; set; }
		public string msg { get; set; }
	}

	public class keyword_Model_C7
	{
		public string param1 { get; set; }
		public string param2 { get; set; }
	}

	public class Common_hasparam3_Post_Model_C7
	{
		public keyword_Model_C7 keyword { get; set; }
	}

	public class Common_hasparam3_Return_Model_C7
	{
		public string data { get; set; }

		public string code { get; set; }
		public string msg { get; set; }
	}

	public class Common_hasparam4_Post_Model_C8
	{
		public string name { get; set; }
		public string age { get; set; }
		public string city { get; set; }
	}

	public class Common_hasparam4_Return_Model_C8
	{
		public string data { get; set; }

		public string code { get; set; }
		public string msg { get; set; }
	}


}