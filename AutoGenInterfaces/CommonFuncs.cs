using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoGenInterfaces
{
    public class CommonFuncs
    {
        public static void writeStringToFile(string Str, string FilePath, string FileName)
        {
            string saveFileName = FilePath + "/" + FileName;
            FileStream fs = new FileStream(saveFileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(Str);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        public static List<string> componentsSeparateString(string str, string separator)
        {
            List<string> result = new List<string>();
            string tempStr = str;
            while (tempStr.Contains(separator))
            {
                int index = tempStr.IndexOf(separator);
                string frontStr = tempStr.Substring(0, index);
                tempStr = tempStr.Substring(index + 1);
                result.Add(frontStr);
            }
            result.Add(tempStr);
            return result;
        }

        public static JObject enJobject(List<string> fieldList)
        {
            JObject jo = new JObject();
            if (fieldList == null || fieldList.Count == 0)
            {
                return jo;
            }

            for (int i = 0; i < fieldList.Count; i++)
            {
                string fieldStr = fieldList[i];
                string Annotations = "";//注释 
                if (fieldStr.Contains(" "))
                {
                    // 去掉注释
                    Annotations = fieldStr.Substring(fieldStr.IndexOf(" ")).Trim();
                    fieldStr = fieldStr.Substring(0, fieldStr.IndexOf(" "));
                }

                if (!fieldStr.Contains("*"))
                {
                    // 不包含下划线，顶级字段
                    JProperty jp = new JProperty(fieldStr, null);
                    jp.AddAnnotation(Annotations);
                    jo.Add(jp);
                }
                else
                {
                    string tempStr = fieldStr;
                    if (fieldStr.Contains("**"))
                    {
                        // 双下划线，代表是数组
                        tempStr = fieldStr.Replace("**", "*Array*");
                    }
                    List<string> list = componentsSeparateString(tempStr, "*");
                    if (list != null)
                    {
                        JObject tempJo = jo;
                        for (int j = 0; j < list.Count; j++)
                        {
                            JToken jt;
                            if (tempJo.TryGetValue(list[j], out jt))
                            {
                                if (jt.HasValues)
                                {
                                    // 找到，继续查找下一项
                                    tempJo = (JObject)jt;
                                }
                                else
                                {
                                    // 找到并且Value为null，说明在加入时是作为叶子节点加入的。
                                    // 但是后面又会检查,有两种情况：1 字段重复，是文档编写错误引起的。2 本节点是数组，文档中在下一行是“{}”包含的内容。
                                    // 在IF_post中，下一项是包含“Array”关键字的字符串，即本次检查的内容。
                                    if (list.Count > j + 1 && list[j + 1].Equals("Array"))
                                    {
                                        // 数组
                                        JObject branchJo = enJObjectFromList(list, j + 1, Annotations);
                                        string key = list[j];
                                        if (key != null)
                                        {
                                            if (branchJo == null)
                                            {
                                                JProperty branchJp = new JProperty(key, null);
                                                branchJp.AddAnnotation(Annotations);
                                                tempJo.Add(branchJp);
                                            }
                                            else
                                            {
                                                JProperty jp = tempJo.Property(key);
                                                string ann = jp.Annotation<string>();
                                                branchJo.AddAnnotation(ann);
                                                tempJo.Remove(key);
                                                tempJo.Add(key, branchJo);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // 文档出错
                                        System.Console.WriteLine("文档有错：" + tempStr);
                                    }
                                }
                            }
                            else
                            {
                                // 没找到，添加对应分支
                                JObject branchJo = enJObjectFromList(list, j + 1, Annotations);
                                string key = list[j];
                                if (key != null)
                                {
                                    if (branchJo == null)
                                    {
                                        JProperty branchJp = new JProperty(key, null);
                                        branchJp.AddAnnotation(Annotations);
                                        tempJo.Add(branchJp);
                                    }
                                    else
                                    {
                                        tempJo.Add(key, branchJo);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return jo;
        }

        private static JObject enJObjectFromList(List<string> list, int fromIndex, string Annotations)
        {
            if (fromIndex >= list.Count)
            {
                return null;
            }
            JObject jo = new JObject();
            JProperty jp = new JProperty(list.Last(), null);
            jp.AddAnnotation(Annotations);
            jo.Add(jp);
            JObject tempJo = jo;
            for (int i = list.Count - 2; i >= fromIndex; i--)
            {
                tempJo = new JObject(new JProperty(list[i], tempJo));
            }
            return tempJo;
        }

    }
}
