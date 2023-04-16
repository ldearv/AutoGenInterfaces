namespace AutoGenInterfaces
{
    public class Constants
    {
        public const string IF_start = ">>"; // 接口文档中，接口开始的标志

        public const string nt = "\r\n\t";
        public const string n2t = "\r\n\t\t";
        public const string n3t = "\r\n\t\t\t";
        public const string n4t = "\r\n\t\t\t\t";

        public const string nomal_using = "using System;" + "\r\n" +
            "using System.Collections.Generic;" + "\r\n" +
            "using System.Linq;" + "\r\n" +
            "using System.Text;" + "\r\n";
        public const string nomal_namespace = "namespace {0}" + "\r\n" + "{" + "\r\n" + "{1}" + "\r\n" + "}";
        public const string nomal_class = "\t" + "public class {0}" + nt + "{" + "\r\n" + "{1}" +nt + "}";

        // NetConstant 文件
        //public const string NC_using = nomal_using;
        //public const string NC_class = "{/// <summary>\r\n\t/// 常量\r\n\t/// </summary>\r\n\tpublic class NetConstant\r\n\t{\r\n\t\tpublic const string DEBUG_PLACE = @\"&DEBUG=0\";\r\n\t\tpublic const string MD5_PLACE = @\"&M=\";\r\n\t\tpublic const string MD5_KEY = @\"";
        //public const string NC_result = "\";\r\n\r\n\t\t// 基本每个接口的返回中都有Result字段.\r\n\t\tpublic const string Result = \"Result\";";
        //public const string NC_end = "\r\n\t}\r\n}";

        // NetConfig 文件，读取模板文件NetConfig.txt

        // NetModel 文件
        public const string result_class =
            "\t" + "/// <summary>"
            + nt + "/// 只有返回result的数据模型"
            + nt + "/// </summary>"
            + nt + "public class ResultModel"
            + nt + "{"
            + n2t + "public List<string> Result;"
            + nt + "}" + "\r\n";
            
    }
}
