using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    public static class DataEncryption
    {
        public static string ToMD5(this string source)
        {
            return GetMD5(source);
        }
        public static string GetMD5(string info)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(info, "MD5");
        }
    }
}
