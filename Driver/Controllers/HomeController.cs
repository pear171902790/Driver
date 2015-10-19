using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Driver.Models;
using Newtonsoft.Json;

namespace Driver.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return new ContentResult() { Content = "hello driver" };
        }

        [HttpPut, Route("api/UploadPosition")]
        public ActionResult UploadPosition([ModelBinder(typeof(JsonBinder<UploadPositionRequest>))]UploadPositionRequest uploadPositionRequest)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var guid = new Guid(token);
                var userData = DriverDBContext.Instance.Datas.SingleOrDefault(x => x.Key == guid);
                if (userData == null)
                {
                    return ApiResponse.UserNotExist;
                }
                var positionData = new Data()
                {
                    Key = Guid.NewGuid(),
                    PhoneNumber = userData.PhoneNumber,
                    CreateTime = DateTime.Now,
                    Type = (int)DataType.Position,
                    Valid = true,
                    Value = JsonConvert.SerializeObject(uploadPositionRequest)
                };
                DriverDBContext.Instance.Datas.Add(positionData);
                DriverDBContext.Instance.SaveChanges();
                return ApiResponse.OK();
            }
            catch (Exception)
            {
                return ApiResponse.UnknownError;
            }
        }


        [HttpGet, Route("api/Positions")]
        public ActionResult GetPositions()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var guid = new Guid(token);
                var userData = DriverDBContext.Instance.Datas.SingleOrDefault(x => x.Key == guid);
                if (userData == null)
                {
                    return ApiResponse.UserNotExist;
                }
                var now = DateTime.Now;
                var begin = new DateTime(now.Year, now.Month, now.Day, 3, 0, 0);
                var end = new DateTime(now.Year, now.Month, now.AddDays(1).Day, 2, 59, 59);
                var data =
                    DriverDBContext.Instance.Datas.Where(
                        x => x.CreateTime >= begin && x.CreateTime <= end && x.Type == (int)DataType.Position).ToList();
                var positions = data.Select(x =>
                {

                    var position =
                        JsonConvert.DeserializeObject<UploadPositionRequest>(x.Value);
                    return new GetPositionsResponse.Position() { Latitude = position.Latitude, Longitude = position.Longitude, Address = position.Address };
                }).ToList();

                var result = new GetPositionsResponse()
                {
                    Positions = positions
                };

                return ApiResponse.OK(JsonConvert.SerializeObject(result));
            }
            catch (Exception)
            {
                return ApiResponse.UnknownError;
            }
        }

        [HttpPost, Route("api/ChangePassword")]
        public ActionResult ChangePassword([ModelBinder(typeof(JsonBinder<ChangePasswordRequest>))]ChangePasswordRequest changePasswordRequest)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var guid = new Guid(token);
                var userData = DriverDBContext.Instance.Datas.SingleOrDefault(x => x.Key == guid);
                if (userData == null)
                {
                    return ApiResponse.UserNotExist;
                }

                var user = JsonConvert.DeserializeObject<User>(userData.Value);
                if (user.Password != changePasswordRequest.OldPassword)
                {
                    return ApiResponse.OldPasswordError;
                }
                user.Password = changePasswordRequest.NewPassword;
                userData.Value = JsonConvert.SerializeObject(user);
                DriverDBContext.Instance.Datas.AddOrUpdate(userData);
                DriverDBContext.Instance.SaveChanges();
                return ApiResponse.OK();
            }
            catch (Exception)
            {
                return ApiResponse.UnknownError;
            }
        }

        [HttpPost, Route("api/UpdateUserInfo")]
        public ActionResult UpdateUserInfo([ModelBinder(typeof(JsonBinder<UpdateUserInfoRequest>))]UpdateUserInfoRequest updateUserInfoRequest)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var guid = new Guid(token);
                var userData = DriverDBContext.Instance.Datas.SingleOrDefault(x => x.Key == guid);
                if (userData == null)
                {
                    return ApiResponse.UserNotExist;
                }

                var user = JsonConvert.DeserializeObject<User>(userData.Value);
                user.CarNumber = updateUserInfoRequest.CarNumber;
                user.CarType = updateUserInfoRequest.CarType;
                user.PhoneNumber = updateUserInfoRequest.PhoneNumber;
                userData.Value = JsonConvert.SerializeObject(user);
                DriverDBContext.Instance.Datas.AddOrUpdate(userData);
                DriverDBContext.Instance.SaveChanges();
                return ApiResponse.OK();
            }
            catch (Exception)
            {
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
                var latestApk = files[0];
                var version = latestApk.Name.Replace("SpecialTA", String.Empty);
                var result= new GetVersionResponse()
                {
                    VersionNumber = version,
                    DownloadUrl = "http://114.215.157.116/api/apk/" + version
                };
                return ApiResponse.OK(JsonConvert.SerializeObject(result));
            }
            catch (Exception)
            {
                return ApiResponse.UnknownError;
            }
        }

        [HttpGet, Route("api/apk/{version}")]
        public ActionResult GetApk(string version)
        {
            try
            {
                var path = GetApkFolderPath() + @"\SpecialTA\" + version;
                return !System.IO.File.Exists(path) ? ApiResponse.FileNotExists : File(path, "application/x-zip-compressed");
            }
            catch (Exception)
            {
                return ApiResponse.UnknownError;
            }
        }

        public ActionResult Test()
        {
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
    }
}