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

        [HttpPut, Route("api/UploadPosition")]
        public ActionResult UploadPosition([ModelBinder(typeof(JsonBinder<UploadPositionRequest>))]UploadPositionRequest uploadPositionRequest)
        {
            try
            {
                var token = Request.Headers["Token"];
                var guid = new Guid(token);
                var userData = DriverDBContext.Instance.Datas.SingleOrDefault(x => x.Key == guid);
                if (userData == null)
                {
                    return ApiResponse.UserNotExist;
                }
                uploadPositionRequest.CarNumber = JsonConvert.DeserializeObject<User>(userData.Value).CarNumber;
                uploadPositionRequest.Address = HttpUtility.UrlDecode(uploadPositionRequest.Address, Encoding.UTF8);
                if (!string.IsNullOrEmpty(uploadPositionRequest.Voice))
                {
                    var bytes = Convert.FromBase64String(uploadPositionRequest.Voice);
                    var voiceName = userData.Key + "_" + CurrentTime + ".mp3";
                    using (
                        var fs = new FileStream(Server.MapPath("~/Voice/") + voiceName,FileMode.Create))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Close();
                    }
                    uploadPositionRequest.Voice = voiceName;
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
            catch (Exception ex)
            {
                var logger = LogManager.GetLogger(typeof(HttpRequest));
                logger.Error("------------------------api/Positions error-------------------------------\r\n"+ex.Message);
                return ApiResponse.UnknownError;
            }
        }

        [HttpPost, Route("api/ChangePassword")]
        public ActionResult ChangePassword([ModelBinder(typeof(JsonBinder<ChangePasswordRequest>))]ChangePasswordRequest changePasswordRequest)
        {
            try
            {
                var token = Request.Headers["Token"];
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
                var result= new GetVersionResponse()
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
                var guid = new Guid(token);
                var userData = DriverDBContext.Instance.Datas.SingleOrDefault(x => x.Key == guid);
                if (userData == null)
                {
                    return ApiResponse.UserNotExist;
                }

                var user = JsonConvert.DeserializeObject<User>(userData.Value);
                user.Integral += 10;
                userData.Value = JsonConvert.SerializeObject(user);
                DriverDBContext.Instance.Datas.AddOrUpdate(userData);
                DriverDBContext.Instance.SaveChanges();
                return ApiResponse.OK();
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
            var datas =
                     DriverDBContext.Instance.Datas.Where(
                         x => x.CreateTime >= DateTime.Now.AddDays(-2) && x.Type == (int)DataType.Position).ToList();
            var list = (from data in datas
                        let position = JsonConvert.DeserializeObject<UploadPositionRequest>(data.Value)
                        where !string.IsNullOrEmpty(position.Voice)
                        select new VoiceViewModel()
                            {
                                CarNumber = position.CarNumber, Source = position.Voice, UploadTime = data.CreateTime.ToLongTimeString()
                            }).ToList();
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