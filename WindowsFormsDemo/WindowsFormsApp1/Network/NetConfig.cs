using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WindowsFormsApp1
{
    public class NetConfig {
        public const string DEBUG_PLACE = @"&DEBUG=0";
        public const string MD5_PLACE = @"&M=";
        public const string MD5_KEY = @"";

        public const string V_BASEURL_Test = "";
        public const string V_BASEURL_Release = "";

        public static string V_BASEURL {
            get {
                return V_BASEURL_Test;
            }
        }

        static NetConfig() {

        }

    }
}
