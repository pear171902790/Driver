using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Driver.Models;
using log4net;
using Newtonsoft.Json;

namespace Driver.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return new ContentResult() { Content = "hello driver" };
        }

        private bool CheckToken(string token)
        {
            return (!string.IsNullOrEmpty(token)) && (HttpRuntime.Cache.Get(token) != null);
        }

        [HttpPost, Route("api/UploadPosition")]
        public ActionResult UploadPosition([ModelBinder(typeof(JsonBinder<UploadPositionRequest>))]UploadPositionRequest uploadPositionRequest)
        {
            try
            {
                var token = Request.Headers["Token"];
                if (!CheckToken(token)) return ApiResponse.NotSignIn;
                var guid = new Guid(token);
                using (var context = new DriverDBContext())
                {
                    var user = context.Users.SingleOrDefault(x => x.Id == guid);
                    if (user == null)
                    {
                        return ApiResponse.UserNotExist;
                    }
                    var postion = new Position() { Id = Guid.NewGuid() };
                    postion.UploadBy = user.Id;
                    postion.Address = HttpUtility.UrlDecode(uploadPositionRequest.Address, Encoding.UTF8);
                    postion.Latitude = uploadPositionRequest.Latitude;
                    postion.Longitude = uploadPositionRequest.Longitude;
                    postion.UploadTime = DateTime.Now;
                    if (!string.IsNullOrEmpty(uploadPositionRequest.Voice))
                    {
                        var bytes = Convert.FromBase64String(uploadPositionRequest.Voice);
                        var voiceName = user.Id + "_" + CurrentTime + ".mp3";
                        using (
                            var fs = new FileStream(Server.MapPath("~/Voice/") + voiceName, FileMode.Create))
                        {
                            fs.Write(bytes, 0, bytes.Length);
                            fs.Close();
                        }
                        postion.Voice = voiceName;
                    }
                    context.Positions.Add(postion);
                    context.SaveChanges();
                    return ApiResponse.OK();
                }
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetLogger(typeof(HttpRequest));
                logger.Error("------------------------api/UploadPosition error-------------------------------\r\n" + ex.Message);
                return ApiResponse.UnknownError;
            }
        }




        [HttpGet, Route("api/Positions")]
        public ActionResult GetPositions()
        {
            try
            {
                var token = Request.Headers["Token"];
                if (!CheckToken(token)) return ApiResponse.NotSignIn;
                var guid = new Guid(token);
                using (var context = new DriverDBContext())
                {
                    var user = context.Users.SingleOrDefault(x => x.Id == guid);
                    if (user == null)
                    {
                        return ApiResponse.UserNotExist;
                    }
                    var now = DateTime.Now;
                    var begin = new DateTime(now.Year, now.Month, now.Day, 3, 0, 0);
                    //                var end = new DateTime(now.Year, now.Month, now.AddDays(1).Day, 2, 59, 59);
                    var data =
                        context.Positions.Where(
                            x => x.UploadTime >= begin && x.UploadTime <= now).ToList();
                    var positions =
                        data.Select(
                            x =>
                                new GetPositionsResponse.Position()
                                {
                                    Latitude = x.Latitude,
                                    Longitude = x.Longitude,
                                    Address = x.Address
                                }).ToList();

                    var result = new GetPositionsResponse()
                    {
                        Positions = positions
                    };

                    return ApiResponse.OK(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetLogger(typeof(HttpRequest));
                logger.Error("------------------------api/Positions error-------------------------------\r\n" + ex.Message);
                return ApiResponse.UnknownError;
            }
        }

        [HttpPost, Route("api/ChangePassword")]
        public ActionResult ChangePassword([ModelBinder(typeof(JsonBinder<ChangePasswordRequest>))]ChangePasswordRequest changePasswordRequest)
        {
            try
            {
                var token = Request.Headers["Token"];
                if (!CheckToken(token)) return ApiResponse.NotSignIn;
                var guid = new Guid(token);
                using (var context = new DriverDBContext())
                {
                    var user = context.Users.SingleOrDefault(x => x.Id == guid);
                    if (user == null)
                    {
                        return ApiResponse.UserNotExist;
                    }

                    if (user.Password != changePasswordRequest.OldPassword)
                    {
                        return ApiResponse.OldPasswordError;
                    }
                    user.Password = changePasswordRequest.NewPassword;
                    context.Users.AddOrUpdate(user);
                    context.SaveChanges();
                    HttpRuntime.Cache.Remove(token);
                    return ApiResponse.OK("你需要重新登录");
                }
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetLogger(typeof(HttpRequest));
                logger.Error("------------------------api/ChangePassword error-------------------------------\r\n" + ex.Message);
                return ApiResponse.UnknownError;
            }
        }

        [HttpPost, Route("api/UpdateUserInfo")]
        public ActionResult UpdateUserInfo([ModelBinder(typeof(JsonBinder<UpdateUserInfoRequest>))]UpdateUserInfoRequest updateUserInfoRequest)
        {
            try
            {
                var token = Request.Headers["Token"];
                if (!CheckToken(token)) return ApiResponse.NotSignIn;
                var guid = new Guid(token);
                using (var context = new DriverDBContext())
                {
                    var user = context.Users.SingleOrDefault(x => x.Id == guid);
                    if (user == null)
                    {
                        return ApiResponse.UserNotExist;
                    }
                    user.CarNumber = updateUserInfoRequest.CarNumber;
                    user.CarType = updateUserInfoRequest.CarType;
                    user.PhoneNumber = updateUserInfoRequest.PhoneNumber;
                    context.Users.AddOrUpdate(user);
                    context.SaveChanges();
                    return ApiResponse.OK();
                }
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetLogger(typeof(HttpRequest));
                logger.Error("------------------------api/UpdateUserInfo error-------------------------------\r\n" + ex.Message);
                return ApiResponse.UnknownError;
            }
        }


        [HttpGet, Route("api/version")]
        public ActionResult GetVersion()
        {
            try
            {
                var dirinfo = new DirectoryInfo(GetApkFolderPath());
                var files = dirinfo.GetFiles();
                Array.Sort<FileInfo>(files, new FIleLastTimeComparer());
                var fileName = files[0].Name;
                var result = new GetVersionResponse()
                {
                    LatestVersion = fileName,
                    DownloadUrl = "http://114.215.157.116/APK/" + fileName
                };
                return ApiResponse.OK(JsonConvert.SerializeObject(result));
            }
            catch (Exception)
            {
                return ApiResponse.UnknownError;
            }
        }

        [HttpPost, Route("api/share")]
        public ActionResult Share([ModelBinder(typeof(JsonBinder<ShareRequest>))]ShareRequest shareRequest)
        {
            try
            {
                var token = Request.Headers["Token"];
                if (!CheckToken(token)) return ApiResponse.NotSignIn;
                var guid = new Guid(token);
                using (var context = new DriverDBContext())
                {
                    var user = context.Users.SingleOrDefault(x => x.Id == guid);
                    if (user == null)
                    {
                        return ApiResponse.UserNotExist;
                    }
                    user.Integral += 10;
                    context.Users.AddOrUpdate(user);
                    context.SaveChanges();
                    return ApiResponse.OK();
                }
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetLogger(typeof(HttpRequest));
                logger.Error("------------------------api/share error-------------------------------\r\n" + ex.Message);
                return ApiResponse.UnknownError;
            }
        }



        public ActionResult Test()
        {
            var str =
                "%E9%99%95%E8%A5%BF%E7%9C%81%E8%A5%BF%E5%AE%89%E5%B8%82%E9%9B%81%E5%A1%94%E5%8C%BA%E5%A4%A7%E5%AF%A8%E8%B7%AF";

            var a = HttpUtility.UrlDecode(str, Encoding.UTF8);
            return View();
        }

        private string GetRootPath()
        {
            string AppPath = "";
            HttpContext HttpCurrent = System.Web.HttpContext.Current;
            if (HttpCurrent != null)
            {
                AppPath = HttpCurrent.Server.MapPath("~");
            }
            else
            {
                AppPath = AppDomain.CurrentDomain.BaseDirectory;
                if (Regex.Match(AppPath, @"\\$", RegexOptions.Compiled).Success)
                    AppPath = AppPath.Substring(0, AppPath.Length - 1);
            }
            return AppPath;
        }

        private string GetApkFolderPath()
        {
            return GetRootPath() + @"\APK";
        }
        public class FIleLastTimeComparer : IComparer<FileInfo>
        {
            public int Compare(FileInfo x, FileInfo y)
            {
                return y.LastWriteTime.CompareTo(x.LastWriteTime);//递减
                //return x.LastWriteTime.CompareTo(y.LastWriteTime);//递增
            }
        }


        public ActionResult Voice()
        {
            List<VoiceViewModel> list;
            using (var context = new DriverDBContext())
            {
                var positions = context.Positions;
                var users = context.Users;
                list = (from p in positions
                        join u in users on p.UploadBy equals u.Id
                        where (!string.IsNullOrEmpty(p.Voice)) && (p.UploadTime >= DateTime.Now.AddDays(-2))
                        select new VoiceViewModel()
                        {
                            CarNumber = u.CarNumber,
                            Source = p.Voice,
                            UploadTime = p.UploadTime.ToLongTimeString()
                        }).ToList();
            }
            return View(list);
        }

        public string CurrentTime
        {
            get
            {
                var now = DateTime.Now;
                return now.Year.ToString() + now.Month.ToString() + now.Day.ToString() + now.Hour.ToString() +
                       now.Minute.ToString() + now.Second.ToString();
            }
        }
    }
}