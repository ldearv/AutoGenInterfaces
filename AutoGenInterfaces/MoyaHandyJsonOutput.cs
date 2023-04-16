using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutoGenInterfaces
{
    class MoyaHandyJsonOutput
    {
        private StringBuilder sbModelString = new StringBuilder(); // 数据模型文件内容
        private List<string> classNameList = new List<string>(); // 类名列表，避免类名重复

        private List<IFModel> ifs;
        private string debugURL;
        private string releaseURL;
        private string md5Key;
        private string savePath;

        private string EndLine = "\n";
        private string Tab = "    ";
        //private string StartupPath;

        // 构造函数
        public MoyaHandyJsonOutput(List<IFModel> ifs, string debugURL, string releaseURL, string md5Key, string StartupPath)
        {
            this.ifs = ifs;
            this.debugURL = debugURL;
            this.releaseURL = releaseURL;
            this.md5Key = md5Key;
            //this.StartupPath = StartupPath;
            string date = DateTime.Now.ToString();
            date = date.Replace("/", "-");
            date = date.Replace(":", "");
            this.savePath = StartupPath + "/Result/" + date + "/MoyaHandyJSON";
            Console.WriteLine(this.savePath);
        }

        public void genIOSOutput()
        {
            // 初始化全局变量
            initGlobalData();
            Directory.CreateDirectory(savePath);
            genEnum();
            genTargetType();
            for (int i = 0; i < ifs.Count; i++)
            {
                //string postClassName = genCodePostModel(ifs[i]);
                genCodeReturnModel(ifs[i]);
            }

            // 生成Model文件
            genFileModel();
            genFileNetDemo();
        }

        // 初始化全局变量
        private void initGlobalData()
        {
            classNameList = new List<string>();
            sbModelString = new StringBuilder();
        }


        #region Enum
        private void genEnum()
        {
            StringBuilder sbEnumString = new StringBuilder();
            for (int i = 0; i < ifs.Count; i++)
            {
                if (ifs[i].IF_post.Count > 0) {
                    string requestStr = ""
                        + Tab + "case " + ifs[i].IF_module + "_" + ifs[i].IF_method + "(parameters: Dictionary<String, Any>)" + Tab + Tab + "// MARK: " + ifs[i].IF_name + EndLine;
                    sbEnumString.Append(requestStr);
                } else {
                    string requestStr = ""
                        + Tab +"case " + ifs[i].IF_module + "_" + ifs[i].IF_method + Tab + Tab + "// MARK: " + ifs[i].IF_name + EndLine;
                    sbEnumString.Append(requestStr);
                }

            }

            string requestFileContext = "//" + EndLine
                + "// Created by AutoIF" + EndLine
                + "//" + EndLine
                + EndLine
                + "public enum HJApi {" + EndLine
                + sbEnumString.ToString()
                + "}" + EndLine;
            CommonFuncs.writeStringToFile(requestFileContext, savePath, "HJApiEnum.swift");
        }

        #endregion

        #region TargetType
        private void genTargetType()
        {
            StringBuilder sbPathString = new StringBuilder();
            for (int i = 0; i < ifs.Count; i++)
            {
                string requestStr = ""
                    + Tab + Tab + "case ." + ifs[i].IF_module + "_" + ifs[i].IF_method + ":" + Tab + Tab + "//" + ifs[i].IF_num + " " + ifs[i].IF_name + EndLine
                    + Tab + Tab + Tab + "return \"" + ifs[i].IF_module + "/" + ifs[i].IF_method + "\"" + EndLine;
                sbPathString.Append(requestStr);

            }

            StringBuilder sbTaskString = new StringBuilder();
            for (int i = 0; i < ifs.Count - 1; i++)
            {
                if (ifs[i].IF_post.Count > 0)
                {
                    string requestStr = ""
                        + Tab + Tab + "case ." + ifs[i].IF_module + "_" + ifs[i].IF_method + "(let parameters):" + Tab + Tab + "//" + ifs[i].IF_num + " " + ifs[i].IF_name + EndLine
                        + Tab + Tab + Tab + "fallthrough" + EndLine;
                    sbTaskString.Append(requestStr);
                }
            }
            sbTaskString.Append(""
                + Tab + Tab + "case ." + ifs[ifs.Count - 1].IF_module + "_" + ifs[ifs.Count - 1].IF_method + "(let parameters):" + Tab + Tab + "//" + ifs[ifs.Count - 1].IF_num + " " + ifs[ifs.Count - 1].IF_name + EndLine
                + Tab + Tab + Tab + "let signPostDic = HJNet.getSignPostDataWithDictionary(dicParameters: parameters)" + EndLine
                + Tab + Tab + Tab + "return .requestParameters(parameters: signPostDic, encoding: JSONEncoding.default)" + EndLine
                + EndLine);

            string requestFileContext = "//" + EndLine
                + "//  HJApiTargetType.swift" + EndLine
                + "//  Created by AutoIF" + EndLine
                + "//" + EndLine
                + EndLine
                + "import Moya" + EndLine
                + "import Foundation" + EndLine
                + EndLine
                + "extension HJApi {" + EndLine
                + Tab + "// MARK: - 请求地址" + EndLine
                + Tab + "public var path: String {" + EndLine
                + Tab + Tab + "switch self {" + EndLine
                + sbPathString.ToString()
                + Tab + Tab + "}" + EndLine
                + Tab + "}" + EndLine
                + EndLine
                + Tab + "// MARK: - 请求的参数在这里处理" + EndLine
                + Tab + "public var task: Task {" + EndLine
                + Tab + Tab + "switch self {" + EndLine
                + sbTaskString.ToString()
                + Tab + Tab + "default: // 无参数走default" + EndLine
                + Tab + Tab + Tab + "let signPostDic = HJNet.getSignPostDataWithDictionary(dicParameters: Dictionary.init())" + EndLine
                + Tab + Tab + Tab + "return .requestParameters(parameters: signPostDic, encoding: JSONEncoding.default)" + EndLine
                + Tab + Tab + Tab + "//return .requestPlain" + EndLine
                + Tab + Tab + "}" + EndLine
                + Tab + "}" + EndLine
                + EndLine
                + "}" + EndLine;
            CommonFuncs.writeStringToFile(requestFileContext, savePath, "HJApiTaskType.swift");
        }

        #endregion

        #region NetModels文件
        private void genFileModel()
        {
            string modelFileContext = "//" + EndLine
                + "//  NetModels.swift" + EndLine
                + "//  Created by AutoIF" + EndLine
                + "//" + EndLine
                + EndLine
                + "import Foundation" + EndLine
                + "import HandyJSON" + EndLine
                + EndLine
                + sbModelString.ToString() + EndLine
                + EndLine;

            CommonFuncs.writeStringToFile(modelFileContext, savePath, "NetModel.swift");
        }
        /// <summary>
        /// 生成数据模型文件
        /// </summary>
        private string genCodePostModel(IFModel IF)
        {
            if (IF.IF_post.Count == 0)
            {
                return null;
            }
            sbModelString.Append(EndLine
                + "#MARK: " + IF.IF_name + EndLine);
            JObject jo = CommonFuncs.enJobject(IF.IF_post);
            string str = JObjectToModel(jo, IF);

            string className = IF.IF_module + "_" + IF.IF_method + "_Post_Model_" + IF.IF_num;
            while (classNameList.Contains(className))
            {
                className = className + "_1";
            }
            classNameList.Add(className);
            string classContext = "struct " + className + ": HandyJSON {" + EndLine
                + str + EndLine
                + "}"+ EndLine;
            sbModelString.Append(classContext);
            return className;
        }

        private string genCodePostDic(IFModel IF)
        {
            if (IF.IF_post.Count == 0)
            {
                // 无发送数据，即GET方式调用
                return null;
            }

            StringBuilder sb = new StringBuilder();
            JObject jo = CommonFuncs.enJobject(IF.IF_post);
            string str = JObjectToDic(jo, IF);
            sb.AppendFormat("[{0}]", str);
            return sb.ToString();
        }

        private string genCodeReturnModel(IFModel IF)
        {


            sbModelString.Append("// MARK: " + IF.IF_name + EndLine);
            if (IF.IF_returnData.Count == 0)
            {
                // 未设置返回值的接口
                string noReturnDataClassName = IF.IF_module + "_" + IF.IF_method + "_Return_Model_" + IF.IF_num;
                string noReturnDataClassContext = "struct " + noReturnDataClassName + ": HandyJSON {" + EndLine
                    + Tab + "var code: Int!" + EndLine
                    + Tab + "var data: String?" + EndLine
                    + Tab + "var msg: String?" + EndLine
                    + "}" + EndLine
                    + EndLine;
                sbModelString.Append(noReturnDataClassContext);
                return null;
            }
            JObject jo = CommonFuncs.enJobject(IF.IF_returnData);
            string str = JObjectToModel(jo, IF);

            string className = IF.IF_module + "_" + IF.IF_method + "_Return_Model_" + IF.IF_num;
            while (classNameList.Contains(className))
            {
                className = className + "_1";
            }
            classNameList.Add(className);
            string classContext = "struct " + className + ": HandyJSON {" + EndLine
                + Tab + "var code: Int!" + EndLine
                + str
                + Tab + "var msg: String?" + EndLine
                + "}" + EndLine
                + EndLine;
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
                            if (arrayItemStr == null)
                            {
                                sb.AppendFormat(Tab + "var {0}: Array<String>?" + EndLine, field);
                            }
                            else
                            {
                                string className = field + "_Model_" + IF.IF_num;
                                while (classNameList.Contains(className))
                                {
                                    className = className + "_1";
                                }
                                classNameList.Add(className);
                                string classContext = "struct " + className + ": HandyJSON {" + EndLine + arrayItemStr + "}" + EndLine + EndLine;
                                sbModelString.Append(classContext);
                                sb.AppendFormat(Tab + "var {1}: Array<{0}>?" + EndLine, className, field);
                            }
                        }
                        else
                        {
                            string nextStr = JObjectToModel(nextJo, IF);
                            string className = field + "_Model_" + IF.IF_num;
                            while (classNameList.Contains(className))
                            {
                                className = className + "_1";
                            }
                            classNameList.Add(className);
                            string classContext = "struct " + className + ": HandyJSON {" + EndLine + nextStr + "}" + EndLine + EndLine;
                            sbModelString.Append(classContext);
                            sb.AppendFormat(Tab + "var {1}: {0}?" + EndLine, className, field);
                        }
                    }
                    else
                    {
                        // 叶子节点
                        if (string.IsNullOrEmpty(ann))
                        {
                            if (string.IsNullOrEmpty(field.Trim()))
                            {
                                return null;
                            }
                            sb.AppendFormat(Tab + "var {0}: String?" + EndLine, field);
                        }
                        else
                        {
                            sb.AppendFormat(Tab + "var {0}: String? // {1}" + EndLine, field, ann);
                        }
                    }
                }
                return sb.ToString();
            }
            return null;
        }

        private string JObjectToDic(JObject jo, IFModel IF)
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
                            string arrayItemStr = JObjectToDic(arrayItemJo, IF);
                            if (arrayItemStr == "\"\"")
                            {
                                sb.AppendFormat("\"{0}\": [{1}], ", field, arrayItemStr);
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\": [[{1}]], ", field, arrayItemStr);
                            }
                        }
                        else
                        {
                            string nextStr = JObjectToDic(nextJo, IF);
                            sb.AppendFormat("\"{0}\": [{1}], ", field, nextStr);
                            
                        }
                    }
                    else
                    {
                        // 叶子节点
                        if (string.IsNullOrEmpty(field))
                        {
                            sb.AppendFormat("\"\", ");
                        }
                        else
                        {
                            sb.AppendFormat("\"{0}\": \"{0}\", ", field);
                        }
                        
                    }
                }
                sb.Remove(sb.Length - 2, 2);
                return sb.ToString();
            }
            return null;
        }
        #endregion NetModels文件

        #region NetDemo文件

        private void genFileNetDemo()
        {
            StringBuilder sbString = new StringBuilder();
            StringBuilder sbCall = new StringBuilder();

            for (int i = 0; i < ifs.Count; i++)
            {
                IFModel IF = ifs[i];
                sbString.Append(Tab + "// MARK: " + IF.IF_name + EndLine);
                sbString.Append(Tab + "func request" + IF.IF_module + IF.IF_method + "() {" + EndLine);
                if (IF.IF_post.Count > 0)
                {
                    sbString.Append(Tab + Tab + "let param = " + genCodePostDic(IF) + " as [String : Any]" + EndLine);
                    sbString.Append(Tab + Tab + "HJNet.request(target: HJApi." + IF.IF_module + "_" + IF.IF_method + "(parameters: param)) { code, result in" + EndLine);
                }
                else
                {
                    sbString.Append(Tab + Tab + "HJNet.request(target: HJApi." + IF.IF_module + "_" + IF.IF_method + ") { code, result in" + EndLine);
                }
                sbString.Append("#if APIDebug1" + EndLine);
                sbString.Append(Tab + Tab + Tab + "print(#function)" + EndLine);
                sbString.Append(Tab + Tab + Tab + "print(\"code:\\(code)\")" + EndLine);
                sbString.Append(Tab + Tab + Tab + "print(\"result:\\(result)\")" + EndLine);
                sbString.Append("#endif" + EndLine);
                sbString.Append(Tab + Tab + Tab + "if let tempModel = " + IF.IF_module + "_" + IF.IF_method + "_Return_Model_" + IF.IF_num + ".deserialize(from: result) {" + EndLine);
                sbString.Append("#if APIDebug1" + EndLine);
                sbString.Append(Tab + Tab + Tab + Tab + "print(#function)" + EndLine);
                sbString.Append(Tab + Tab + Tab + Tab + "HJNet.compareJson(preJson: result, nextJson: (tempModel.toJSONString() ?? \"{}\"))" + EndLine);
                sbString.Append("#endif" + EndLine);
                sbString.Append(Tab + Tab + Tab + Tab + "if tempModel.code == 200 {" + EndLine);
                sbString.Append(Tab + Tab + Tab + Tab + Tab + EndLine);
                sbString.Append(Tab + Tab + Tab + Tab + "} else {" + EndLine);
                sbString.Append(Tab + Tab + Tab + Tab + Tab + "print(\"code 不等于 200\")" + EndLine);
                sbString.Append(Tab + Tab + Tab + Tab + "}" + EndLine);
                sbString.Append(Tab + Tab + Tab + "}" + EndLine);
                sbString.Append(Tab + Tab + "} failureBlock: { code, msg in" + EndLine);
                sbString.Append(Tab + Tab + Tab + "print(#function)" + EndLine);
                sbString.Append(Tab + Tab + Tab + "print(\"联网失败：code->\\(code), msg->\\(msg)\")" + EndLine);
                sbString.Append(Tab + Tab + "}" + EndLine);
                sbString.Append(Tab + "}" + EndLine);
                sbString.Append(EndLine);

                sbCall.Append(Tab + Tab + "request" + IF.IF_module + IF.IF_method + "()" + Tab  + Tab + "// " + IF.IF_num + " " + IF.IF_name + EndLine);
            }

            string strDemo = "//" + EndLine
                + "// NetDemo.swift" + EndLine
                + "// Created by AutoIF" + EndLine
                + "//" + EndLine
                + EndLine
                + "class NetDemo {" + EndLine
                + Tab + "static let sharedInstance = NetDemo()" + EndLine
                + EndLine
                + Tab + "private init() {" + EndLine
                + Tab + Tab + "test()" + EndLine
                + Tab + "}" + EndLine
                + EndLine
                + Tab + "func test() {" + EndLine
                + sbCall.ToString()
                + Tab + "}" + EndLine
                + EndLine
                + sbString.ToString()
                + "}" + EndLine;
            CommonFuncs.writeStringToFile(strDemo, savePath, "NetDemo.swift");
        }        
        #endregion NetDemo文件
    }
}
