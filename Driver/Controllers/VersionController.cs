using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Driver.Models;
using log4net;
using Newtonsoft.Json;

namespace Driver.Controllers
{
    public class VersionController : ControllerBase
    {
        [HttpGet, Route("api/version")]
        public ActionResult GetVersion()
        {
            try
            {
                AppVersion appVersion;
                using (var ctx=new DriverDBContext())
                {
                    appVersion = ctx.AppVersions.OrderByDescending(x => x.VersionCode).FirstOrDefault();
                }
                var result = new GetVersionResponse()
                {
                    VersionName = appVersion.VersionName,
                    VersionCode = appVersion.VersionCode,
                    DownloadUrl = Request.Url.Authority+"/apk/" + appVersion.FileName.Substring(0,appVersion.FileName.IndexOf('.'))
                };
                return ApiResponse.OK(JsonConvert.SerializeObject(result));
            }
            catch (Exception)
            {
                return ApiResponse.UnknownError;
            }
        }

        [HttpGet, Route("apk/{fileName}")]
        public ActionResult DownloadApk(string fileName)
        {
            fileName += ".apk";
            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename="+fileName);
            string filename = Server.MapPath("~/APK/"+fileName);
            Response.TransmitFile(filename);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        [HttpGet, Route("version")]
        public ActionResult Version()
        {
            return View();
        }

        [HttpPost, Route("version")]
        public ActionResult PostVersion()
        {
            using (var ctx = new DriverDBContext())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var httpPostedFile = Request.Files["APK"];
                        var folderPath = Server.MapPath("~/APK");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        var fullPath = Server.MapPath("~/APK/" + httpPostedFile.FileName);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        var versionCode = Request.Form["VersionCode"];
                        var versionName = Request.Form["VersionName"];
                        var appVersion = new AppVersion() { VersionCode = Convert.ToInt32(versionCode), VersionName = versionName, FileName =  httpPostedFile.FileName };
                        ctx.AppVersions.Add(appVersion);
                        ctx.SaveChanges();
                        httpPostedFile.SaveAs(fullPath);
                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        var logger = LogManager.GetLogger(typeof(HttpRequest));
                        logger.Error("------------------------api/PostVersion error-------------------------------\r\n" + ex.Message);
                        dbTran.Rollback();
                        return new RedirectResult("~/version?msg=上传出错");
                    }
                }
            }

            return new RedirectResult("~/version?msg=上传成功");
        }
    }
}