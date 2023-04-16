using System;
using System.Collections.Generic;

namespace AutoGenInterfaces
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string path = System.Environment.CurrentDirectory;
            List<IFModel> ifs = MiddleCode.parseIFDoc(path + "/apilist.txt");

            MoyaHandyJsonOutput moya = new MoyaHandyJsonOutput(ifs, "", "", "", path);
            moya.genIOSOutput();

            CSharpOutput cs = new CSharpOutput("myNamespace", ifs, "", "", "", path);
            cs.genCSharpOutput();
        }
    }
}
