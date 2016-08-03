using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Driver.Models;
using log4net;
using Newtonsoft.Json;
using NReco.VideoConverter;

namespace Driver.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            return new ContentResult() { Content = "hello driver" };
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
                        Task.Run(() =>
                        {
                            try
                            {
                                var bytes = Convert.FromBase64String(uploadPositionRequest.Voice);
                                var voiceName = user.Id + "_" + CurrentTime;
                                var voicePath = Server.MapPath("~/Voice/");
                                var sourceFileFullName = voicePath + voiceName + ".3gp";
                                var finalFileFullName = voicePath + voiceName + ".mp4";
                                if (!Directory.Exists(voicePath))
                                {
                                    Directory.CreateDirectory(voicePath);
                                }
                                using (var fs = new FileStream(sourceFileFullName, FileMode.Create))
                                {
                                    fs.Write(bytes, 0, bytes.Length);
                                    fs.Close();
                                }
                                var ffMpeg = new FFMpegConverter();
                                ffMpeg.ConvertMedia(sourceFileFullName, finalFileFullName, Format.mp4);
                                System.IO.File.Delete(sourceFileFullName);
                                postion.Voice = finalFileFullName;
                            }
                            catch (Exception ex)
                            {
                                var logger = LogManager.GetLogger(typeof (HttpRequest));
                                logger.Error(
                                    "------------------------api/Voice error-------------------------------\r\n" +
                                    ex.Message);
                            }
                        });
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
                    var begin = DateTime.Now.AddHours(-3);
                    var data =
                        context.Positions.Where(
                            x => x.UploadTime >= begin).ToList();
                    var positions =
                        data.Select(
                            x =>
                                new GetPositionsResponse.Position()
                                {
                                    Latitude = x.Latitude,
                                    Longitude = x.Longitude,
                                    Address = x.Address,
                                    UploadTime = x.UploadTime
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

        [HttpGet, Route("api/AllPositions")]
        public ActionResult GetAllPositions()
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
                    var begin = DateTime.Now.AddDays(-1);
                    var data =
                        context.Positions.Where(
                            x => x.UploadTime >= begin).ToList();
                    var positions =
                        data.Select(
                            x =>
                                new GetPositionsResponse.Position()
                                {
                                    Latitude = x.Latitude,
                                    Longitude = x.Longitude,
                                    Address = x.Address,
                                    UploadTime = x.UploadTime
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
                logger.Error("------------------------api/AllPositions error-------------------------------\r\n" + ex.Message);
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

        

        

        [HttpGet, Route("yuyin")]
        public ActionResult Voice()
        {
            List<VoiceViewModel> list;
            using (var context = new DriverDBContext())
            {
                var positions = context.Positions;
                var users = context.Users;
                list = (from p in positions
                        join u in users on p.UploadBy equals u.Id
                        where !string.IsNullOrEmpty(p.Voice)
                        orderby p.UploadTime descending 
                        select new VoiceViewModel()
                        {
                            CarNumber = u.CarNumber,
                            Source = p.Voice,
                            UploadTime = p.UploadTime
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