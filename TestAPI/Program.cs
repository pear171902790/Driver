using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Driver;

namespace TestAPI
{
    class Program
    {
        static  void Main(string[] args)
        {
//           TestPut().Wait();
//            var str = "eyJQaG9uZU51bWJlciI6IjEzODE2NzY5NzY0IiwiUGFzc3dvcmQiOiJlMTBhZGMzOTQ5YmE1OWFiYmU1NmUwNTdmMjBmODgzZSJ9";
//            var a = str.ToStr();

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
            var message = await client.PutAsync("http://114.215.157.116/api/SignUp", 
                new StringContent("{\"PhoneNumber\":\"15972198620\",\"CarType\":\"aa\",\"CarNumber\":\"bb\",\"Password\":\"dsfadfsaf\"}"));
            var a = message;
            var b = await message.Content.ReadAsStringAsync();

            var c = b;
        }
    }
}
