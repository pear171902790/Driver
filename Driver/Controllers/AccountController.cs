using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Driver.Models;
using Newtonsoft.Json;

namespace Driver.Controllers
{
    public class AccountController : Controller
    {
        [HttpPut, Route("api/SignUp")]
        public ActionResult SignUp([ModelBinder(typeof(JsonBinder<SignUpRequest>))]SignUpRequest signUpRequest)
        {
            try
            {
                var isPhoneNumber = System.Text.RegularExpressions.Regex.IsMatch(signUpRequest.PhoneNumber, @"^[1]+[3,5]+\d{9}");
                if (!isPhoneNumber)
                {
                    return ApiResponse.NotPhoneNumber;
                }
                var count = DriverDBContext.Instance.Datas.Count(x => x.PhoneNumber == signUpRequest.PhoneNumber);
                if (count > 0)
                {
                    return ApiResponse.PhoneNumberAlreadySignUp;
                }
                var guid = Guid.NewGuid();
                var data = new Data()
                {
                    Key = guid,
                    CreateTime = DateTime.Now,
                    PhoneNumber = signUpRequest.PhoneNumber,
                    Type = (int)DataType.User,
                    Valid = true,
                    Value = JsonConvert.SerializeObject(signUpRequest.ToUser(guid))
                };
                DriverDBContext.Instance.Datas.Add(data);
                DriverDBContext.Instance.SaveChanges();
                return ApiResponse.OK(guid.ToString());
            }
            catch (Exception ex)
            {
                return ApiResponse.UnknownError;
            }
        }

        [HttpGet, Route("api/SignIn/{base64}")]
        public ActionResult SignIn(string base64)
        {
            try
            {
                var json = base64.ToStr();
                var signInRequest = JsonConvert.DeserializeObject<SignInRequest>(json);
                var userData = DriverDBContext.Instance.Datas.SingleOrDefault(x => x.PhoneNumber == signInRequest.PhoneNumber);
                if (userData == null)
                {
                    return ApiResponse.UserNotExist;
                }
                var user = JsonConvert.DeserializeObject<User>(userData.Value);
                if (user.Password != signInRequest.Password)
                {
                    return ApiResponse.PasswordError;
                }
                var token = user.Id.ToString();

                HttpRuntime.Cache.Add(token, token, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration, CacheItemPriority.High, null);

                Response.Headers.Add("X-Token", token);
                Response.Headers.Add("Access-Control-Expose-Headers", "X-Token");

                return ApiResponse.OK(JsonConvert.SerializeObject(user.ToSignInResponse()));
            }
            catch (Exception ex)
            {
                return ApiResponse.UnknownError;
            }
        }

        [HttpGet,Route("api/test")]
        public ActionResult Test()
        {
            return Content("test");
        }
    }
}