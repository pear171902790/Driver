using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Driver
{
    public static class Convertor
    {
        public static string ToStr(this string base64)
        {
            var bpath = Convert.FromBase64String(base64);
            return System.Text.Encoding.UTF8.GetString(bpath);
        }
    }
}