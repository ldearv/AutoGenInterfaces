using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoGenInterfaces
{
    class MiddleCode
    {
        /// <summary>
        /// 解析接口文件
        /// </summary>
        /// <param name="interfaceText">接口文件地址</param>
        /// <returns>解析成功，结果存到ifs中; 解析失败，失败原因存到ifs_err中</returns>
        public static List<IFModel> parseIFDoc(string interfaceDocPath)
        {
            // 初始化全局变量
            List<IFModel> ifs = new List<IFModel>();// 解析好的接口数据
            List<IFModel> ifs_err = new List<IFModel>();// 解析失败的接口
            IFModel current_IF = null; // 当前处理的接口
            int current_step = 0; // 当前步骤 0：未开始或已结束； 1：开始处理接口{接口名，接口编号，模块名，方法名}；2 Post数据；3 返回状态码；4 返回数据和备注；

            Stack<string> postDataStack = new Stack<string>(); // 发送数据栈
            Stack<string> returnDataStack = new Stack<string>(); // 返回数据栈

            StreamReader sr = new StreamReader(interfaceDocPath, Encoding.Default);
            String line;
            try
            {
                while ((line = sr.ReadLine()) != null)
                {
                    //Console.WriteLine(line.ToString());
                    string trimLine = line.Trim();
                    if (trimLine == Constants.IF_start)
                    {
                        // 遇到 ">>"的行，认为是开始一个新的接口,保存上一个接口，
                        if (current_IF != null)
                        {
                            if (current_IF.IF_name == null || current_IF.IF_module == null || current_IF.IF_method == null || current_IF.IF_returnCode == null)
                            {
                                current_IF.err.Add("关键数据缺失");
                            }
                            if (postDataStack != null && postDataStack.Count != 0)
                            {
                                current_IF.err.Add("Post数据中\"{\" 与\"}\"数量不一致");
                            }
                            if (returnDataStack != null && returnDataStack.Count != 0)
                            {
                                current_IF.err.Add("返回数据中\"{\" 与\"}\"数量不一致");
                            }

                            if (current_IF.err.Count == 0)
                            {
                                ifs.Add(current_IF);
                            }
                            else
                            {
                                ifs_err.Add(current_IF);
                            }
                        }
                        // 开始解析接口名、接口编号、接口模块、接口方法
                        IFModel IF = new IFModel();
                        current_IF = IF;
                        current_step = 1;
                    }
                    else if (trimLine.ToUpper().StartsWith("POST") || trimLine.StartsWith("发送数据"))
                    {
                        // 开始解析发送字段
                        current_step = 2;
                        postDataStack = new Stack<string>();
                    }
                    else if (trimLine.StartsWith("返回状态码") || trimLine.StartsWith("ReturnCode"))
                    {
                        // 开始解析返回状态码
                        current_step = 3;
                    }
                    else if (trimLine.StartsWith("返回数据") || trimLine.StartsWith("ReturnData"))
                    {
                        // 开始解析返回数据和备注
                        current_step = 4;
                        returnDataStack = new Stack<string>();
                    }
                    else if (current_step == 1)
                    {
                        // 解析接口名、接口编号、接口模块、接口方法
                        if (trimLine.StartsWith("Interface No."))
                        {
                            // 接口编号
                            current_IF.IF_num = trimLine.Substring("Interface No.".Length).Trim();
                        }
                        else if (trimLine.StartsWith("接口编号"))
                        {
                            // 接口编号
                            current_IF.IF_num = trimLine.Substring("接口编号".Length).Trim();
                        }
                        else if (trimLine.Contains("/"))
                        {
                            // 接口模块/接口方法
                            int compoIndex = trimLine.IndexOf("/");
                            current_IF.IF_module = trimLine.Substring(0, compoIndex);
                            current_IF.IF_method = trimLine.Substring(compoIndex + 1);
                        }
                        else
                        {
                            // 接口名
                            current_IF.IF_name = trimLine;
                        }
                    }
                    else if (current_step == 2)
                    {
                        // 解析发送字段
                        if (trimLine.StartsWith("['"))
                        {
                            if (postDataStack.Count == 0)
                            {
                                current_IF.IF_post.Add(fieldDocToMid(trimLine));
                            }
                            else
                            {
                                string preLevelStr = postDataStack.Peek();
                                current_IF.IF_post.Add(preLevelStr + "*" + fieldDocToMid(trimLine));
                            }
                        }
                        else if (trimLine.StartsWith("{"))
                        {
                            // "{"约定为数组的开始
                            if (current_IF.IF_post.Count == 0)
                            {
                                current_IF.err.Add("无法解析" + trimLine);
                            }
                            else
                            {
                                string preLevel = current_IF.IF_post.Last();
                                if (preLevel.Contains(" "))
                                {
                                    // 去掉注释
                                    string preLevelField = preLevel.Substring(0, preLevel.IndexOf(" "));
                                    postDataStack.Push(preLevelField + "*");
                                }
                                else
                                {
                                    postDataStack.Push(preLevel + "*");
                                }
                            }
                        }
                        else if (trimLine.StartsWith("}"))
                        {
                            // "}"约定为数组的结束
                            postDataStack.Pop();
                        }
                    }
                    else if (current_step == 3)
                    {
                        // 解析返回状态码
                        if (trimLine.StartsWith("R."))
                        {
                            if (trimLine.Contains("\t"))
                            {
                                trimLine = trimLine.Replace("\t", " ");
                            }
                            current_IF.IF_returnCode.Add(trimLine);
                        }
                        else
                        {
                            current_IF.IF_remarks.Add(trimLine);
                        }
                    }
                    else if (current_step == 4)
                    {
                        // 解析返回数据和备注
                        if (trimLine.StartsWith("['"))
                        {
                            if (returnDataStack.Count == 0)
                            {
                                current_IF.IF_returnData.Add(fieldDocToMid(trimLine));
                            }
                            else
                            {
                                string preLevelStr = returnDataStack.Peek();
                                current_IF.IF_returnData.Add(preLevelStr + "*" + fieldDocToMid(trimLine));
                            }
                        }
                        else if (trimLine.StartsWith("{"))
                        {
                            // "{"约定为数组的开始
                            if (current_IF.IF_returnData.Count == 0)
                            {
                                current_IF.err.Add("无法解析" + trimLine);
                            }
                            else
                            {
                                string preLevel = current_IF.IF_returnData.Last();
                                if (preLevel.Contains(" "))
                                {
                                    // 去掉注释
                                    string preLevelField = preLevel.Substring(0, preLevel.IndexOf(" "));
                                    returnDataStack.Push(preLevelField + "*");
                                }
                                else
                                {
                                    returnDataStack.Push(preLevel + "*");
                                }
                            }
                        }
                        else if (trimLine.StartsWith("}"))
                        {
                            // "}"约定为数组的结束
                            returnDataStack.Pop();
                        }
                        else
                        {
                            current_IF.IF_remarks.Add(trimLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("堆栈为空"))
                {
                    current_IF.err.Add("请检查本接口中\"{\" 和 \"}\"是否数量一致, 并且要注意,代表数组的\"{\"和\"}\"都是要另起一行写的");
                }
                else
                {
                    current_IF.err.Add(ex.Message + ex.StackTrace);
                }
                ifs_err.Add(current_IF);
            }
            sr.Close();

            if (ifs_err.Count > 0)
            {
                StringBuilder sbError = new StringBuilder();
                for (int i = 0; i < ifs_err.Count; i++)
                {
                    IFModel ife = ifs_err[i];
                    System.Console.WriteLine(ife.IF_num + " : " + ife.IF_module + "/" + ife.IF_method);
                    sbError.Append(ife.IF_num + " : " + ife.IF_module + "/" + ife.IF_method + "\r\n");
                    for (int j = 0; j < ife.err.Count; j++)
                    {
                        System.Console.WriteLine(ife.err[j]);
                        sbError.Append(ife.err[j] + "\r\n");
                    }
                    sbError.Append("\r\n");
                }
                Console.WriteLine("某些接口有错,具体查看Console输出");
                Console.WriteLine(sbError.ToString());
            }

            return ifs;
        }


        /// <summary>
        /// 将文档中字段的表示方式，改成中间存储方式。即去除中括号和单引号，字典类型上下级用"*"隔开。
        /// </summary>
        /// <param name="field">文档中的字段表示方法</param>
        /// <returns></returns>
        private static string fieldDocToMid(string field)
        {
            string tempField = field;
            string Annotations = "";//注释
            if (field.Contains("\t"))
            {
                tempField = field.Replace("\t", " ");
            }

            if (tempField.Contains(" "))
            {
                // 去掉注释
                Annotations = tempField.Substring(tempField.IndexOf(" ")).Trim();
                tempField = tempField.Substring(0, tempField.IndexOf(" "));
            }

            string tempStr = tempField.Replace("']['", "*");
            tempStr = tempStr.Replace("['", "");
            tempStr = tempStr.Replace("']", "");
            if (Annotations.Equals(""))
            {
                return tempStr;
            }
            else
            {
                return tempStr + " " + Annotations;
            }
        }
    }
}
