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
using NReco.VideoConverter;

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
            //            TestConvert3gpToMp4();
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

        private void TestConvert3gpToMp4()
        {
            var voicePath = Server.MapPath("~/Voice/");
            var voiceName = "87de70cc-2bdf-4ad1-b4f7-7bedd68c5975_2016718211939";
            var ffMpeg = new FFMpegConverter();
            ffMpeg.ConvertMedia(voicePath + voiceName + ".3gp", voicePath + voiceName + ".mp4", Format.mp4);
            File.Delete(voicePath + voiceName + ".3gp");
        }

        private async Task DeleteOldPositions()
        {
            await Task.Run(() =>
            {
                ClearOldPositions();

                ClearOldVoices();
            });
        }

        private void ClearOldPositions()
        {
            using (var db = new DriverDBContext())
            {
                db.Positions.Where(x => x.UploadTime < DateTime.Today).ToList().ForEach(x => db.Positions.Remove(x));
                db.SaveChanges();
            }
        }

        private void ClearOldVoices()
        {
            var voicePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Voice/");
            if (!string.IsNullOrEmpty(voicePath)&&Directory.Exists(voicePath))
            {
                var files = Directory.GetFiles(voicePath);
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastAccessTime < DateTime.Today)
                        fi.Delete();
                }
            }
        }
    }
}
