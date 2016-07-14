using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net.Config;

namespace Driver
{
    public class MvcApplication : System.Web.HttpApplication
    {
        System.Timers.Timer _timScheduledTask;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);

            DeleteOldPositions();

            _timScheduledTask = new System.Timers.Timer
            {
                Interval = 86400000,
                Enabled = true
            };

            _timScheduledTask.Elapsed += (o, e) =>
           {
               DeleteOldPositions();
           };

            //            ConvertVoiceTest();
        }

        private void ConvertVoiceTest()
        {
            var voice = "";
            using (
                 var fs = new FileStream("d:\\777.mp3", FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);
                byte[] bytes1 = br.ReadBytes((int)fs.Length);
                voice = Convert.ToBase64String(bytes1);
                br.Close();
            }


            var bytes = Convert.FromBase64String(voice);
            var voiceName = "user_now" + ".mp3";
            var voicePath = Server.MapPath("~/Voice/");
            if (!Directory.Exists(voicePath))
            {
                Directory.CreateDirectory(voicePath);
            }
            using (
                var fs = new FileStream(voicePath + voiceName, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
        }

        private async Task DeleteOldPositions()
        {
            await Task.Run(() =>
            {
                using (var db = new DriverDBContext())
                {
                    var begin = DateTime.Now.AddHours(-3);
                    db.Positions.Where(x => x.UploadTime < begin).ToList().ForEach(x => db.Positions.Remove(x));
                }
            });
        }
    }
}
