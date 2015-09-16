using System;
using System.Linq;
using System.Web.Mvc;
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
                return null;
            }
            catch (Exception ex)
            {
                return ApiResponse.UnknownError;
            }
        }
    }
}