using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Driver.Models;
using log4net;
using Newtonsoft.Json;

namespace Driver.Controllers
{
    public class AccountController : ControllerBase
    {
        [HttpPost, Route("api/SignUp")]
        public ActionResult SignUp([ModelBinder(typeof(JsonBinder<SignUpRequest>))]SignUpRequest signUpRequest)
        {
            try
            {
                if (signUpRequest == null) return ApiResponse.ParameterError;
                Regex dReg = new Regex("[0-9]{11,11}");
                var isPhoneNumber = dReg.IsMatch(signUpRequest.PhoneNumber.Trim());
                if (!isPhoneNumber)
                {
                    return ApiResponse.NotPhoneNumber;
                }
                using (var context = new DriverDBContext())
                {
                    var count = context.Users.Count(x => x.PhoneNumber == signUpRequest.PhoneNumber);
                    if (count > 0)
                    {
                        return ApiResponse.PhoneNumberAlreadySignUp;
                    }
                    count = context.Users.Count(x => x.CarNumber == signUpRequest.CarNumber);
                    if (count > 0)
                    {
                        return ApiResponse.CarNumberAlreadySignUp;
                    }
                    var guid = Guid.NewGuid();
                    var now = DateTime.Now;
                    var data = new User()
                    {
                        Id = guid,
                        RegsiterTime = now,
                        PhoneNumber = signUpRequest.PhoneNumber,
                        CarNumber = signUpRequest.CarNumber,
                        CarType = signUpRequest.CarType,
                        Password = signUpRequest.Password,
                        Valid = true,
                        ExpirationTime = now.AddMonths(1)
                    };
                    context.Users.Add(data);
                    context.SaveChanges();
                    return ApiResponse.OK(guid.ToString());
                }
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetLogger(typeof(HttpRequest));
                logger.Error("------------------------api/SignUp error-------------------------------\r\n" + ex.Message);
                return ApiResponse.UnknownError;
            }
        }


        [HttpPost, Route("api/SignIn")]
        public ActionResult SignIn([ModelBinder(typeof(JsonBinder<SignInRequest>))]SignInRequest signInRequest)
        {
            try
            {
                using (var context = new DriverDBContext())
                {
                    var user = context.Users.SingleOrDefault(x => x.PhoneNumber == signInRequest.PhoneNumber);
                    if (user == null)
                    {
                        return ApiResponse.UserNotExist;
                    }
                    if (user.Password != signInRequest.Password)
                    {
                        return ApiResponse.PasswordError;
                    }
                    var token = user.Id.ToString();

                    HttpRuntime.Cache.Add(token, token, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration,
                        CacheItemPriority.High, null);

                    Response.Headers.Add("X-Token", token);
                    Response.Headers.Add("Access-Control-Expose-Headers", "X-Token");

                    return ApiResponse.OK(JsonConvert.SerializeObject(user.ToSignInResponse()));
                }
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetLogger(typeof(HttpRequest));
                logger.Error("------------------------api/SignIn error-------------------------------\r\n" + ex.Message);
                return ApiResponse.UnknownError;
            }
        }

        [HttpGet, Route("api/SignOut")]
        public ActionResult SignOut()
        {
            var token = Request.Headers["Token"];
            if (!string.IsNullOrEmpty(token)) HttpRuntime.Cache.Remove(token);
            return ApiResponse.OK("你需要重新登录");
        }
    }
}