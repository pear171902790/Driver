using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Driver.Models;

namespace Driver
{
    public static class Convertor
    {
        public static string ToStr(this string base64)
        {
            var bpath = Convert.FromBase64String(base64);
            return System.Text.Encoding.UTF8.GetString(bpath);
        }


        

//        public static string ToStr(this PostageType postageType)
//        {
//            switch (postageType)
//            {
//                case PostageType.HalfYear:
//                    return "半年";
//                case PostageType.ThreeMonths:
//                    return "三个月";
//                case PostageType.OneYear:
//                    return "一年";
//            }
//            return null;
//        }
    }
}