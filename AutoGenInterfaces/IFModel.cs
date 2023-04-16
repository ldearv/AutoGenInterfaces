using System.Collections.Generic;

namespace AutoGenInterfaces
{
    /*
>>
1、登录接口
   Interface No. 2001
   user/login
   POST:
    ['User']['UserName'] 用户账号
    ['User']['Password'] 用户密码
    返回状态码：
    R.2001.ERR.1 参数为空
    R.2001.ERR.2 账号、密码不匹配
    R.2001.ERR.9 数据库异常
    R.2001.SUC.1 成功
	返回数据：
	['UserId'] 用户ID
	['UserName'] 用户账号
	['Mobile'] 手机号
	['Email'] 邮箱地址
>>
     */
    /// <summary>
    /// 接口文档模型
    /// </summary>
    public class IFModel
    {
        public string IF_name { get; set; } // 接口名字 <1、登录接口>
        public string IF_num { get; set; } // 接口编号 <2001>
        public string IF_module { get; set; } // 接口模块 <user>
        public string IF_method { get; set; } // 接口方法 <login>
        public List<string> IF_post { get; set; } // 发送数据 <["['User']['UserName'] 用户账号"，"['User']['Password'] 用户密码"]>
        public List<string> IF_returnCode { get; set; } // 返回状态码，空格隔开编码和说明
        public List<string> IF_returnData { get; set; } // 返回数据，空格隔开字段和说明
        public List<string> IF_remarks { get; set; } // 备注。

        public List<string> err { get; set; } // 解析过程中报错内容

        // 构造函数
        public IFModel()
        {
            IF_post = new List<string>();
            IF_returnCode = new List<string>();
            IF_returnData = new List<string>();
            IF_remarks = new List<string>();
            err = new List<string>();
        }
    }

    public class InfoModel
    {
        public string strMd5key { get; set; }
        public string strNamespace { get; set; }
        public string strDebugUrl { get; set; }
        public string strReleaseUrl { get; set; }
        public string strDocumentPath { get; set; }
        public bool isPHPChecked { get; set; }
        public bool isCSharpChecked { get; set; }
        public bool isASIChecked { get; set; }
        public bool isAFChecked { get; set; }
        public bool isNSChecked { get; set; }
        public bool isSwiftChecked { get; set; }
    }
}
