using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutoGenInterfaces
{
    public class CSharpOutput
    {
        private StringBuilder sbModelString; // 数据模型文件内容
        private List<string> classNameList; // 类名列表，避免类名重复
        private StringBuilder sbNetApiString; // NetApi文件内容

        private string strNameSpace;
        private List<IFModel> ifs;
        private string debugURL;
        private string releaseURL;
        private string md5Key;
        private string savePath;
        private string StartupPath;

        private string EndLine = "\r\n";
        private string Tab = "\t";
        // 构造函数
        public CSharpOutput(string strNameSpace, List<IFModel> ifs, string debugURL, string releaseURL, string md5Key, string StartupPath)
        {
            this.strNameSpace = strNameSpace;
            this.ifs = ifs;
            this.debugURL = debugURL;
            this.releaseURL = releaseURL;
            this.md5Key = md5Key;
            this.StartupPath = StartupPath;
            string date = DateTime.Now.ToString();
            date = date.Replace("/", "-");
            date = date.Replace(":", "");
            this.savePath = StartupPath + "/Result/" + date + "/CSharp";
        }

        public void genCSharpOutput()
        {
            // 初始化全局变量
            initGlobalData();
            Directory.CreateDirectory(savePath);
            // 生成Config文件
            genFileConfig();

            // 生成Constant文件
            genFileConstant();

            genFileRequest();

            for (int i = 0; i < ifs.Count; i++)
            {
                string postClassName = genCodePostModel(ifs[i]);
                genCodeNetApi(ifs[i], postClassName);
                genCodeReturnModel(ifs[i]);
            }

            // 生成Model.cs文件
            genFileModel();

            genFileNetApi();

            genFileNetDemo();
        }

        // 初始化全局变量
        private void initGlobalData()
        {
            classNameList = new List<string>();
            sbModelString = new StringBuilder();
            sbNetApiString = new StringBuilder();
        }

        #region HttpRequest文件
        private void genFileRequest()
        {
            
            StringBuilder sbRequestString = new StringBuilder();
            for (int i = 0; i < ifs.Count; i++)
            {
                string requestStr = EndLine
                    + Tab + Tab + Tab + Tab + "case NetTag.Tag_" + ifs[i].IF_module + "_" + ifs[i].IF_method + ": //" + ifs[i].IF_num + " " + ifs[i].IF_name + EndLine
                    + Tab + Tab + Tab + Tab + Tab + "urlString = string.Format(urlString, \"" + ifs[i].IF_module + "/" + ifs[i].IF_method + "\");" + EndLine
                    + Tab + Tab + Tab + Tab + Tab + "break;" + EndLine;
                sbRequestString.Append(requestStr);
            }

            List<string> requestTempls = readRequestTemplates();
            string requestFileContext = requestTempls[0] + strNameSpace + requestTempls[1]
                + sbRequestString.ToString() + requestTempls[2];
            CommonFuncs.writeStringToFile(requestFileContext, savePath, "NetRequest.cs.txt");
        }

        // 读取模板文件
        private List<string> readRequestTemplates()
        {
            List<string> result = new List<string>();

            string path = StartupPath + "/Templates/csTemplates/NetRequest_begin.txt";
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string content_1 = sr.ReadToEnd();
            sr.Close();
            result.Add(content_1);

            path = StartupPath + "/Templates/csTemplates/NetRequest_middle.txt";
            sr = new StreamReader(path, Encoding.Default);
            string content_2 = sr.ReadToEnd();
            sr.Close();
            result.Add(content_2);

            path = StartupPath + "/Templates/csTemplates/NetRequest_end.txt";
            sr = new StreamReader(path, Encoding.Default);
            string content_3 = sr.ReadToEnd();
            sr.Close();
            result.Add(content_3);

            return result;
        }
        #endregion HttpRequest文件

        #region NetApis文件
        private void genFileNetApi()
        {
            string netApiFileContext = Constants.nomal_using
                + "using System.Net;" + EndLine
                + "using System.Net.NetworkInformation;" + EndLine
                + "using NetWork;" + EndLine
                + "using Newtonsoft.Json;" + EndLine
                + EndLine
                + "namespace " + strNameSpace + " {" + EndLine
                + Tab + "/// <summary>" + EndLine
                + Tab + "/// 网络接口类" + EndLine
                + Tab + "/// </summary>" + EndLine
                + Tab + "public class NetAPIs" + EndLine
                + Tab + "{" + EndLine
                + sbNetApiString.ToString() + EndLine
                + Tab + "}" + EndLine
                + "}";
            CommonFuncs.writeStringToFile(netApiFileContext, savePath, "NetApis.cs.txt");
        }
        private void genCodeNetApi(IFModel IF, string postClassName)
        {
            StringBuilder sbApiCode = new StringBuilder();
            sbApiCode.Append(Tab + Tab + "/// <summary>" + EndLine);
            sbApiCode.Append(Tab + Tab + "/// " + IF.IF_name + " " + IF.IF_num + EndLine);
            if (IF.IF_remarks != null && IF.IF_remarks.Count != 0)
            {
                for (int i = 0; i < IF.IF_remarks.Count; i++)
                {
                    sbApiCode.Append(Tab + Tab + "/// " + IF.IF_remarks[i] + EndLine);
                }
            }
            if (IF.IF_returnCode != null && IF.IF_returnCode.Count != 0)
            {
                sbApiCode.Append(Tab + Tab + "/// 返回状态码" + EndLine);
                for (int i = 0; i < IF.IF_returnCode.Count; i++)
                {
                    sbApiCode.Append(Tab + Tab + "/// " + IF.IF_returnCode[i] + EndLine);
                }
            }
            sbApiCode.Append(Tab + Tab + "/// </summary>" + EndLine);
            if (postClassName == null)
            {
                sbApiCode.Append(Tab + Tab + "public static void " + IF.IF_module + "_" + IF.IF_method + "(IResultsHandler client)" + EndLine);
                sbApiCode.Append(Tab + Tab + "{" + EndLine);
                sbApiCode.Append(Tab + Tab + Tab + "NetRequest request = new NetRequest();" + EndLine);
                sbApiCode.Append(Tab + Tab + Tab + "string postDataStr = \"\";" + EndLine);
            }
            else
            {
                sbApiCode.Append(Tab + Tab + "public static void " + IF.IF_module + "_" + IF.IF_method + "(" + postClassName + " postData, IResultsHandler client)" + EndLine);
                sbApiCode.Append(Tab + Tab + "{" + EndLine);
                sbApiCode.Append(Tab + Tab + Tab + "NetRequest request = new NetRequest();" + EndLine);
                sbApiCode.Append(Tab + Tab + Tab + "string postDataStr = JsonConvert.SerializeObject(postData);" + EndLine);
            }
            sbApiCode.Append(Tab + Tab + Tab + "request.StartRequestWithType(postDataStr, NetTag.Tag_" + IF.IF_module + "_" + IF.IF_method + ", client);" + EndLine);
            sbApiCode.Append(Tab + Tab + "}" + EndLine);
            sbApiCode.Append(EndLine);
            sbNetApiString.Append(sbApiCode.ToString());
        }
        #endregion NetApis文件

        #region NetModels文件
        private void genFileModel()
        {
            string modelFileContext = "using System;" + EndLine
                + "using System.Collections.Generic;" + EndLine
                + "using System.Linq;" + EndLine
                + "using System.Text;" + EndLine
                + EndLine
                + "namespace " + strNameSpace + EndLine
                + "{" + EndLine
                + Tab + "public class OnlyResult_Return_Model" + EndLine
                + Tab + "{" + EndLine
                + Tab + Tab + "public List<string> Result { get; set; }" + EndLine
                + Tab + "}" + EndLine
                + EndLine
                + sbModelString.ToString() + EndLine
                + "}";

            CommonFuncs.writeStringToFile(modelFileContext, savePath, "NetModels.cs.txt");
        }
        /// <summary>
        /// 生成数据模型文件
        /// </summary>
        private string genCodePostModel(IFModel IF)
        {
            if (IF.IF_post.Count == 0)
            {
                // 无发送数据
                string className_0 = IF.IF_module + "_" + IF.IF_method + "_Post_Model_" + IF.IF_num;
                if (classNameList.Contains(className_0))
                {
                    className_0 = className_0 + "_1";
                }
                classNameList.Add(className_0);   
                string classContext_0 = string.Format(Tab + "public class {0}" + EndLine + Tab + "{1}" + EndLine + Tab + "{2}" + EndLine + EndLine, className_0, "{", "}");
                sbModelString.Append(classContext_0);
                return className_0;
            }
            JObject jo = CommonFuncs.enJobject(IF.IF_post);
            string str = JObjectToModel(jo, IF);

            string className = IF.IF_module + "_" + IF.IF_method + "_Post_Model_" + IF.IF_num;
            if (classNameList.Contains(className))
            {
                className = className + "_1";
            }
            classNameList.Add(className);
            string classContext = string.Format(Tab + "public class {0}" + EndLine + Tab + "{1}" + EndLine + "{2}" + Tab + "{3}" + EndLine + EndLine, className, "{", str, "}");
            sbModelString.Append(classContext);
            return className;
        }

        private string genCodeReturnModel(IFModel IF)
        {
            if (IF.IF_returnData.Count == 0)
            {
                return null;
            }
            JObject jo = CommonFuncs.enJobject(IF.IF_returnData);
            string str = JObjectToModel(jo, IF);

            string className = IF.IF_module + "_" + IF.IF_method + "_Return_Model_" + IF.IF_num;
            if (classNameList.Contains(className))
            {
                className = className + "_1";
            }
            classNameList.Add(className);
            string classContext = string.Format(Tab + "public class {0}" + EndLine + Tab + "{1}" + EndLine + "{2}" + EndLine + Tab + Tab + "public string code {3} get; set; {4}" + EndLine + Tab + Tab + "public string msg {3} get; set; {4}" + EndLine + Tab + "{5}" + EndLine + EndLine, className, "{", str, "{", "}", "}");
            sbModelString.Append(classContext);
            return className;
        }

        private string JObjectToModel(JObject jo, IFModel IF)
        {
            if (jo != null && jo.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                IEnumerable<JProperty> properties = jo.Properties();
                foreach (JProperty item in properties)
                {
                    string field = item.Name;
                    string ann = item.Annotation<string>();
                    JToken value = item.Value;
                    if (value.HasValues)
                    {
                        // 枝干
                        JObject nextJo = (JObject)value;
                        JToken t;

                        if (nextJo.Count == 1 && nextJo.TryGetValue("Array", out t))
                        {
                            // 数组
                            JObject arrayItemJo = (JObject)t;
                            string arrayItemStr = JObjectToModel(arrayItemJo, IF);
                            string className = field + "_Model_" + IF.IF_num;
                            if (classNameList.Contains(className))
                            {
                                className = className + "_1";
                            }
                            classNameList.Add(className);
                            string classContext = string.Format(Tab + "public class {0}" + EndLine + Tab + "{1}" + EndLine + "{2}" + Tab + "{3}" + EndLine + EndLine, className, "{", arrayItemStr, "}");
                            sbModelString.Append(classContext);
                            sb.AppendFormat(Tab + Tab + "public List<{0}> {1} {2} get; set; {3}" + EndLine, className, field, "{", "}");
                        }
                        else
                        {
                            string nextStr = JObjectToModel(nextJo, IF);
                            string className = field + "_Model_" + IF.IF_num;
                            if (classNameList.Contains(className))
                            {
                                className = className + "_1";
                            }
                            classNameList.Add(className);
                            string classContext = string.Format(Tab + "public class {0}" + EndLine + Tab + "{1}" + EndLine + "{2}" + Tab + "{3}" + EndLine + EndLine, className, "{", nextStr, "}");
                            sbModelString.Append(classContext);
                            sb.AppendFormat(Tab + Tab + "public {0} {1} {2} get; set; {3}" + EndLine, className, field, "{", "}");
                        }
                    }
                    else
                    {
                        // 叶子节点
                        if (string.IsNullOrEmpty(ann))
                        {
                            sb.AppendFormat(Tab + Tab + "public string {0} {1} get; set; {2}" + EndLine, field, "{", "}");
                        }
                        else
                        {
                            sb.AppendFormat(Tab + Tab + "public string {0} {1} get; set; {2} // {3}" + EndLine, field, "{", "}", ann);
                        }
                    }
                }
                return sb.ToString();
            }
            return null;
        }
        #endregion NetModels文件

        #region NetDemo文件
        private void genFileNetDemo()
        {
            StringBuilder sbPostString = new StringBuilder();
            StringBuilder sbSucString = new StringBuilder();
            StringBuilder sbErrString = new StringBuilder();
            StringBuilder sbFaildString = new StringBuilder();
            // 发起联网
            for (int i = 0; i < ifs.Count; i++)
            {
                IFModel IF = ifs[i];
                sbPostString.Append(Tab + Tab + Tab + "// " + IF.IF_name + EndLine);
                sbPostString.Append(Tab + Tab + Tab + IF.IF_module + "_" + IF.IF_method + "_Post_Model_" + IF.IF_num + " pd_" + IF.IF_num + " = new " + IF.IF_module + "_" + IF.IF_method + "_Post_Model_" + IF.IF_num + "();" + EndLine);
                //sbPostString.Append(Tab + Tab + Tab + "// TODO:封装发送数据" + EndLine);
                sbPostString.Append(Tab + Tab + Tab + "NetAPIs." + IF.IF_module + "_" + IF.IF_method + "(pd_" + IF.IF_num + ", this);" + EndLine + EndLine);

                sbSucString.Append(Tab + Tab + Tab + Tab + "#region " + IF.IF_name + EndLine);
                sbSucString.Append(Tab + Tab + Tab + Tab + "case NetTag.Tag_" + IF.IF_module + "_" + IF.IF_method + ":" + EndLine);
                sbSucString.Append(Tab + Tab + Tab + Tab + "{" + EndLine);
                if (IF.IF_returnData.Count == 0)
                {
                    sbSucString.Append(Tab + Tab + Tab + Tab + Tab + "OnlyResult_Return_Model returnData = JsonConvert.DeserializeObject<OnlyResult_Return_Model>(results);" + EndLine);
                }
                else
                {
                    sbSucString.Append(Tab + Tab + Tab + Tab + Tab + IF.IF_module + "_" + IF.IF_method + "_Return_Model_" + IF.IF_num + " returnData = JsonConvert.DeserializeObject<" + IF.IF_module + "_" + IF.IF_method + "_Return_Model_" + IF.IF_num + ">(results);" + EndLine);
                }
                sbSucString.Append(Tab + Tab + Tab + Tab + "}" + EndLine);
                sbSucString.Append(Tab + Tab + Tab + Tab + Tab + "break;" + EndLine);
                sbSucString.Append(Tab + Tab + Tab + Tab + "#endregion " + IF.IF_name + EndLine + EndLine);


                sbErrString.Append(Tab + Tab + Tab + Tab + "#region " + IF.IF_name + EndLine);
                sbErrString.Append(Tab + Tab + Tab + Tab + "case NetTag.Tag_" + IF.IF_module + "_" + IF.IF_method + ":" + EndLine);
                sbErrString.Append(Tab + Tab + Tab + Tab + "{" + EndLine);
                List<string> comStrList = CommonFuncs.componentsSeparateString(IF.IF_returnCode[0], " ");
                //List<string> comDotStrList = CommonFuncs.componentsSeparateString(comStrList[0], ".");
                //sbErrString.Append(Tab + Tab + Tab + Tab + Tab + "if (results.Contains(\"" + comStrList[0] + "\"))" + EndLine);
                //sbErrString.Append(Tab + Tab + Tab + Tab + Tab + "{" + EndLine);
                //sbErrString.Append(Tab + Tab + Tab + Tab + Tab + Tab + "System.Console.WriteLine(\"" + IF.IF_returnCode[0] + "\");" + EndLine);
                //sbErrString.Append(Tab + Tab + Tab + Tab + Tab + "}" + EndLine);
                // for (int j = 1; j < IF.IF_returnCode.Count; j++)
                // {
                //     comStrList = CommonFuncs.componentsSeparateString(IF.IF_returnCode[j], " ");
                //     //comDotStrList = CommonFuncs.componentsSeparateString(comStrList[0], ".");
                //     sbErrString.Append(Tab + Tab + Tab + Tab + Tab + "else if (results.Contains(\"" + comStrList[0] + "\"))" + EndLine);
                //     sbErrString.Append(Tab + Tab + Tab + Tab + Tab + "{" + EndLine);
                //     sbErrString.Append(Tab + Tab + Tab + Tab + Tab + Tab + "System.Console.WriteLine(\"" + IF.IF_returnCode[j] + "\");" + EndLine);
                //     sbErrString.Append(Tab + Tab + Tab + Tab + Tab + "}" + EndLine);
                // }
                sbErrString.Append(Tab + Tab + Tab + Tab + "}" + EndLine);
                sbErrString.Append(Tab + Tab + Tab + Tab + Tab + "break;" + EndLine);
                sbErrString.Append(Tab + Tab + Tab + Tab + "#endregion " + IF.IF_name + EndLine + EndLine);

                sbFaildString.Append(Tab + Tab + Tab + Tab + "#region " + IF.IF_name + EndLine);
                sbFaildString.Append(Tab + Tab + Tab + Tab + "case NetTag.Tag_" + IF.IF_module + "_" + IF.IF_method + ":" + EndLine);
                sbFaildString.Append(Tab + Tab + Tab + Tab + "{" + EndLine);
                sbFaildString.Append(Tab + Tab + Tab + Tab + Tab + "System.Console.WriteLine(\"联网失败:\" + \"" + IF.IF_num + "\");" + EndLine);
                sbFaildString.Append(Tab + Tab + Tab + Tab + "}" + EndLine);
                sbFaildString.Append(Tab + Tab + Tab + Tab + Tab + "break;" + EndLine);
                sbFaildString.Append(Tab + Tab + Tab + Tab + "#endregion " + IF.IF_name + EndLine + EndLine);
            }

            string strDemo = Constants.nomal_using
                + "using NetWork;" + EndLine
                + "using Newtonsoft.Json;" + EndLine
                + "using System.Windows.Forms;" + EndLine
                + EndLine
                + "namespace " + strNameSpace + EndLine
                + "{" + EndLine
                + Tab + "public class NetDemo:Form,IResultsHandler" + EndLine
                + Tab + "{" + EndLine
                + Tab + Tab + "public void TestDemo()" + EndLine
                + Tab + Tab + "{" + EndLine
                + sbPostString.ToString()
                + Tab + Tab + "}" + EndLine
                + Tab + Tab + "#region 网络" + EndLine
                + Tab + Tab + "public void RequestSuccessed(string httptag, string results)" + EndLine
                + Tab + Tab + "{" + EndLine
                + Tab + Tab + Tab + "//if (!this.IsHandleCreated)" + EndLine
                + Tab + Tab + Tab + "//{" + EndLine
                + Tab + Tab + Tab + "//" + Tab + "return;" + EndLine
                + Tab + Tab + Tab + "//}" + EndLine
                + Tab + Tab + Tab + "Console.WriteLine(\"成功接口编号为:\" + httptag + \"    内容为:\" + results);" + EndLine
                + EndLine
                + Tab + Tab + Tab + "switch (httptag)" + EndLine
                + Tab + Tab + Tab + "{" + EndLine
                + sbSucString.ToString()
                + Tab + Tab + Tab + "}" + EndLine
                + Tab + Tab + "}" + EndLine
                + EndLine
                + EndLine
                + Tab + Tab + "public void RequestError(string httptag, string results)" + EndLine
                + Tab + Tab + "{" + EndLine
                + Tab + Tab + Tab + "//if (!this.IsHandleCreated)" + EndLine
                + Tab + Tab + Tab + "//{" + EndLine
                + Tab + Tab + Tab + "//" + Tab + "return;" + EndLine
                + Tab + Tab + Tab + "//}" + EndLine
                + Tab + Tab + Tab + "Console.WriteLine(\"Error接口编号为:\" + httptag+\"    内容为:\"+results);" + EndLine
                + EndLine
                + Tab + Tab + Tab + "switch (httptag)" + EndLine
                + Tab + Tab + Tab + "{" + EndLine
                + sbErrString.ToString()
                + Tab + Tab + Tab + "}" + EndLine
                + Tab + Tab + "}" + EndLine
                + EndLine
                + EndLine
                + Tab + Tab + "public void RequestFailed(string httptag, int type)" + EndLine
                + Tab + Tab + "{" + EndLine
                + Tab + Tab + Tab + "//if (!this.IsHandleCreated)" + EndLine
                + Tab + Tab + Tab + "//{" + EndLine
                + Tab + Tab + Tab + "//" + Tab + "return;" + EndLine
                + Tab + Tab + Tab + "//}" + EndLine
                + Tab + Tab + Tab + "Console.WriteLine(\"Failed接口编号为:\" + httptag);" + EndLine
                + EndLine
                + Tab + Tab + Tab + "switch (httptag)" + EndLine
                + Tab + Tab + Tab + "{" + EndLine
                + sbFaildString.ToString()
                + Tab + Tab + Tab + "}" + EndLine
                + Tab + Tab + "}" + EndLine
                + EndLine
                + Tab + Tab + "#endregion 网络" + EndLine
                + Tab + "}" + EndLine
                + "}";
            CommonFuncs.writeStringToFile(strDemo, savePath, "NetDemo.cs.txt");
        }
        #endregion NetDemo文件

        #region NetConfig文件
        // 生成config文件
        private void genFileConfig()
        {
            string path = StartupPath + "/Templates/csTemplates/NetConfig.txt";
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string content = sr.ReadToEnd();
            sr.Close();

            string result = content.Replace("{0}", strNameSpace);
            result = result.Replace("{1}", md5Key);
            result = result.Replace("{2}", debugURL);
            result = result.Replace("{3}", releaseURL);

            CommonFuncs.writeStringToFile(result, savePath, "NetConfig.cs.txt");
        }
        #endregion NetConfig文件

        #region NetTags文件
        private void genFileConstant()
        {
            
            StringBuilder sbTagsString = new StringBuilder();
            for (int i = 0; i < ifs.Count; i++)
            {
                sbTagsString.AppendFormat(
                    Tab + Tab + "/// <summary>" + EndLine
                    + Tab + Tab + "/// " + ifs[i].IF_name + EndLine
                    + Tab + Tab + "/// </summary>" + EndLine
                    + Tab + Tab + "public const string Tag_" + ifs[i].IF_module + "_" + ifs[i].IF_method + " = \"" + ifs[i].IF_num + "\";" + EndLine);
            }

            string result = Constants.nomal_using
                + "namespace " + strNameSpace + EndLine
                + "{" + EndLine
                + Tab + "/// <summary>" + EndLine
                + Tab + "/// 常量" + EndLine
                + Tab + "/// </summary>" + EndLine
                + Tab + "public class NetTag" + EndLine
                + Tab + "{" + EndLine
                + EndLine
                + sbTagsString.ToString()
                + Tab + "}" + EndLine
                +"}" + EndLine;
            CommonFuncs.writeStringToFile(result, savePath, "NetTags.cs.txt");
        }
        #endregion NetTags文件

    }
}
