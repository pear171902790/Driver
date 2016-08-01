using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Driver;
using System.Net;
using System.IO;
using System.Threading;

namespace TestAPI
{
    class Program
    {
        static async Task Do1()
        {
            await Task.Run(()=>Console.WriteLine("Do1"));
        }

        static async Task Do2()
        {
            await Task.Delay(1500);
            Console.WriteLine("Do2");
        }

        static void Post(string authorization = null)
        {
            var url = "http://iems.shinetechchina.com/MyiEMS/taskes/mytaskes.aspx";
            var request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.Headers["Cookie"] = "ASP.NET_SessionId=t3ihj4t1ylffhadj3t4rblxe";
            request.Headers["Upgrade-Insecure-Requests"] = "1";
            request.Headers["Origin"] = "http://iems.shinetechchina.com";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers["Accept-Encoding"] = "gzip, deflate";
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.94 Safari/537.36";
            request.Referer = "http://iems.shinetechchina.com/MyiEMS/taskes/mytaskes.aspx";
            request.Host = "iems.shinetechchina.com";
            request.Headers["Cache-Control"] = "max-age=0";
            //request.Headers[""] = "";
            //request.Headers[""] = "";
            if (!string.IsNullOrEmpty(authorization))
            {
                request.Headers["Authorization"] = authorization;
            }
            var postData = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTE0MjM0Njc4NTIPFgIeC1ZFbXBsb3llZUlkKClYU3lzdGVtLkd1aWQsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OSQ3MTdkMGE2OC1iZWFhLTQ1YTUtOWQ4Yy1mNmE5MjBjYWQyNGQWAmYPZBYCAgMPZBYCAgEPZBYEAgEPDxYCHgRUZXh0BQoyMDE2LTA3LTEzZGQCAw8WAh4LXyFJdGVtQ291bnQCARYCZg9kFiACAQ8PFgIfAQURRnJlZHJpayBHP3JhbnNzb25kZAICDw8WAh8BBRRQUkoyMDExMDMtMTg2MyAgICAgIGRkAgMPDxYCHwEFGU9EQyBzZXJ2aWNlIGZvciBGb3JlZnJvbnRkZAIEDxUBCOato%2BW4uFBPZAIFDw8WAh8BBRBPUkQtMjAxNjAxLTEzODE0ZGQCBg8PFgIfAQUKMjAxNi0wMS0wNWRkAgcPDxYCHwEFCjIwMTYtMDEtMDFkZAIIDw8WAh8BBQoyMDE2LTEyLTMxZGQCCQ8PFgIfAQUHMjAwMC4wMGRkAgoPDxYCHwEFBzEwMzIuMDBkZAILDw8WAh8BBQczMzYwLjAwZGQCDA8VAQNVU0RkAg0PDxYCHwEFCDQwMzIwLjAwZGQCDg8VAQNVU0RkAhgPDxYEHwEFDuaPkOS6pChTdWJtaXQpHg9Db21tYW5kQXJndW1lbnQFJDg3NmMyOGYyLTJmNjAtNDJkOC1hZTA1LTA5MmZlYjBmYTcwOGRkAhkPDxYCHwEFZ%2BWQjOS4gFBP5LiL5ZCM5LiA5bKX57qn5q%2BP5pel5re75Yqg55qE5bel5L2c6YePLOS7mOi0ueW5tOWBh%2BaAu%2BWSjOS4jeiDvei2hei%2FhzjlsI%2Fml7bvvIzor7fph43mlrDloavlhplkZGQka4FVdA6Fw%2BdUHYJkI%2F93AQY%2F6aKz2tbJlioUuR0NdQ%3D%3D&__VIEWSTATEGENERATOR=F480A8BE&__EVENTVALIDATION=%2FwEdAApdoN1Nobxjikqp3%2FLlMTlUDhvtX1R1fw0bQOO0dEZlgMFC%2F8pH6fuIrYGYTJ%2FWHeXPh%2F1NmbcP5QLKLGF%2Bria0Y1M5sIjAF%2BrEIhYF%2Bvckm0np1rYrYgbli87OUv2dBXjaYbBOi0r3dRpZfOA9bkfHYr46%2F3Pznb5%2BGlA5Iv3JGkwQXII8iseNfOeoib2qwnlpzWFaHenW%2BzTIuBDUx1VEEGu2%2FZzlH%2FhhveiGDqZS4aQCmBwEBiQRt3Lb%2FG23Bqk%3D&ctl00%24ContentPlaceHolderMain%24rtPOs%24ctl00%24hidIsOverTime=False&ctl00%24ContentPlaceHolderMain%24rtPOs%24ctl00%24txtTask1=&ctl00%24ContentPlaceHolderMain%24rtPOs%24ctl00%24txtHours1=8&ctl00%24ContentPlaceHolderMain%24rtPOs%24ctl00%24txtDescription1=&ctl00%24ContentPlaceHolderMain%24rtPOs%24ctl00%24hfHoursAmount=968.00&ctl00%24ContentPlaceHolderMain%24rtPOs%24ctl00%24hfPOId=3457a523-7564-45d1-b66c-010abdbba7b0&ctl00%24ContentPlaceHolderMain%24rtPOs%24ctl00%24hdnVisibleNo=1&ctl00%24ContentPlaceHolderMain%24rtPOs%24ctl00%24btnSaveTaskes=%E6%8F%90%E4%BA%A4%28Submit%29";
            var data = Encoding.ASCII.GetBytes(postData);
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            using (var myHttpWebResponse = (HttpWebResponse)request.GetResponse())
            {
                using (var responseStream = myHttpWebResponse.GetResponseStream())
                {
                    using (var myStreamReader = new StreamReader(responseStream, Encoding.Default))
                    {
                        var pageContent = myStreamReader.ReadToEnd();
                        var a = pageContent;
                    }
                }
            }
        }

        

        static void Main(string[] args)
        {

            Console.WriteLine("11");
            Task.Run(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("2222");
            });
            Console.WriteLine("333");



//            try
//            {
//                throw new Exception();
//            }
//            catch (Exception ex)
//            {
//
//                Console.WriteLine("catch");
//            }
//            finally
//            {
//                Console.WriteLine("catch waimiande");
//
//            }
//
//
//            Console.WriteLine("catch waimiande");
//            








            //            Console.WriteLine("start...");
            //            var timScheduledTask = new System.Timers.Timer
            //            {
            //                Interval = 3000,
            //                Enabled = true
            //            };
            //
            //            timScheduledTask.Elapsed += (o, e) =>
            //            {
            //                Do1();
            //            };
            //
            //            Do1();


            //            var base64EncodedData = "TlRMTVNTUAADAAAAGAAYAKIAAACKAYoBugAAAAAAAABYAAAALAAsAFgAAAAeAB4AhAAAABAAEABEAgAAFYKI4goAWikAAAAPlCBK7Fi/dR9nU1MsRoVLQmwAaQBjAEAAcwBoAGkAbgBlAHQAZQBjAGgAYwBoAGkAbgBhAC4AYwBvAG0ARABFAFMASwBUAE8AUAAtAE8ANABTAEYAMgAzADcAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAhLvpiq/dhsn14iH7rpd3vwEBAAAAAAAAEohnNtzc0QGNHSvtjYh2ggAAAAACABwAUwBIAEkATgBFAFQARQBDAEgAQwBIAEkATgBBAAEADgBDAFIATQAtAFMAVgBSAAQAJABzAGgAaQBuAGUAdABlAGMAaABjAGgAaQBuAGEALgBjAG8AbQADADQAYwByAG0ALQBzAHYAcgAuAHMAaABpAG4AZQB0AGUAYwBoAGMAaABpAG4AYQAuAGMAbwBtAAUAJABzAGgAaQBuAGUAdABlAGMAaABjAGgAaQBuAGEALgBjAG8AbQAHAAgAEohnNtzc0QEGAAQAAgAAAAgAMAAwAAAAAAAAAAAAAAAAMAAAnzvwAyR8Sj9acSBdCZaxbPfumDHUVdUzXh91mDeg/zMKABAAAAAAAAAAAAAAAAAAAAAAAAkAOABIAFQAVABQAC8AaQBlAG0AcwAuAHMAaABpAG4AZQB0AGUAYwBoAGMAaABpAG4AYQAuAGMAbwBtAAAAAAAAAAAAAAAAAK11Uh2SBW74gcSPTzKqK8k=";
            //            var str = System.Text.Encoding.ASCII.GetString(System.Convert.FromBase64String(base64EncodedData));
            //            Console.WriteLine(str);


            //try
            //{
            //    Post();
            //}
            //catch {
            //    try
            //    {
            //        Post("Negotiate TlRMTVNTUAABAAAAl4II4gAAAAAAAAAAAAAAAAAAAAAKAFopAAAADw==");
            //    }
            //    catch {
            //        Post("Negotiate TlRMTVNTUAADAAAAGAAYAKIAAACKAYoBugAAAAAAAABYAAAALAAsAFgAAAAeAB4AhAAAABAAEABEAgAAFYKI4goAWikAAAAPJ6yf/EX5rY4WcijO017rpmwAaQBjAEAAcwBoAGkAbgBlAHQAZQBjAGgAYwBoAGkAbgBhAC4AYwBvAG0ARABFAFMASwBUAE8AUAAtAE8ANABTAEYAMgAzADcAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAZidPZIg6Tqkdhxwnv+v2YwEBAAAAAAAADZ+0RNrc0QELCErQs9LH8wAAAAACABwAUwBIAEkATgBFAFQARQBDAEgAQwBIAEkATgBBAAEADgBDAFIATQAtAFMAVgBSAAQAJABzAGgAaQBuAGUAdABlAGMAaABjAGgAaQBuAGEALgBjAG8AbQADADQAYwByAG0ALQBzAHYAcgAuAHMAaABpAG4AZQB0AGUAYwBoAGMAaABpAG4AYQAuAGMAbwBtAAUAJABzAGgAaQBuAGUAdABlAGMAaABjAGgAaQBuAGEALgBjAG8AbQAHAAgADZ+0RNrc0QEGAAQAAgAAAAgAMAAwAAAAAAAAAAAAAAAAMAAAnzvwAyR8Sj9acSBdCZaxbPfumDHUVdUzXh91mDeg/zMKABAAAAAAAAAAAAAAAAAAAAAAAAkAOABIAFQAVABQAC8AaQBlAG0AcwAuAHMAaABpAG4AZQB0AGUAYwBoAGMAaABpAG4AYQAuAGMAbwBtAAAAAAAAAAAAAAAAAHLi3PLhpb3T75WdXyct+yM=");
            //    }
            //}











            //Task t=Do1();
            //Console.WriteLine("Main");


            //           TestPut().Wait();
            //            var str = "eyJQaG9uZU51bWJlciI6IjEzODE2NzY5NzY0IiwiUGFzc3dvcmQiOiJlMTBhZGMzOTQ5YmE1OWFiYmU1NmUwNTdmMjBmODgzZSJ9";
            //            var a = str.ToStr();
            //var task = TestPut();
            //            var str =
            //                "%E9%99%95%E8%A5%BF%E7%9C%81%E8%A5%BF%E5%AE%89%E5%B8%82%E9%9B%81%E5%A1%94%E5%8C%BA%E5%A4%A7%E5%AF%A8%E8%B7%AF";
            //            Console.WriteLine(a);
            Console.ReadLine();
        }
        //localhost:31437
        //114.215.157.116
        public async static Task TestPut()
        {
            var client = new HttpClient();
            var message = await client.PostAsync("http://localhost:31437/api/SignUp",
                new StringContent("{\"PhoneNumber\":\"18609154433\",\"CarType\":\"标致4008\",\"CarNumber\":\"陕AS0915\",\"Password\":\"mockpassword\"}"));
            var a = message;
            var b = await message.Content.ReadAsStringAsync();

            var c = b;
        }
    }
}
