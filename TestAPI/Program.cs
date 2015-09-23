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
            var str = "eyJQaG9uZU51bWJlciI6IjEzODE2NzY5NzY0IiwiUGFzc3dvcmQiOiJlMTBhZGMzOTQ5YmE1OWFiYmU1NmUwNTdmMjBmODgzZSJ9";
            var a = str.ToStr();
            Console.WriteLine(a);
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
