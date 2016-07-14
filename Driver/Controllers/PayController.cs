using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Driver.Models;
using log4net;
using Newtonsoft.Json;

namespace Driver.Controllers
{
    public class PayController : ControllerBase
    {
        [HttpGet, Route("api/PostageTypes")]
        public ActionResult GetPostageTypes()
        {
            try
            {
                var token = Request.Headers["Token"];
                if (!CheckToken(token)) return ApiResponse.NotSignIn;
                var postageTypes = new List<dynamic>()
                {
                    new {Name="三个月",Price=60,Code=0},
                    new {Name="半年",Price=90,Code=1},
                    new {Name="一年",Price=150,Code=2}
                };

                return ApiResponse.OK(JsonConvert.SerializeObject(postageTypes));
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetLogger(typeof(HttpRequest));
                logger.Error("------------------------api/PostageTypes error-------------------------------\r\n" + ex.Message);
                return ApiResponse.UnknownError;
            }
        }

        [HttpPost, Route("api/Pay")]
        public ActionResult Pay([ModelBinder(typeof(JsonBinder<PayRequest>))]PayRequest payRequest)
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
                    user.ExpirationTime = UpdateExpirationTime(user.ExpirationTime, payRequest.PostageType);
                    user.Amount += payRequest.Amount + ",";
                    user.PostageTypes += payRequest.PostageType + ",";
                    user.Paychannel += payRequest.Paychannel + ",";
                    context.SaveChanges();
                    return ApiResponse.OK();
                }
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetLogger(typeof(HttpRequest));
                logger.Error("------------------------api/Pay error-------------------------------\r\n" + ex.Message);
                return ApiResponse.UnknownError;
            }
        }

        private DateTime UpdateExpirationTime(DateTime dateTime, int postageType)
        {
            switch ((PostageType)postageType)
            {
                case PostageType.HalfYear:
                    return dateTime.AddMonths(6);
                    break;
                case PostageType.ThreeMonths:
                    return dateTime.AddMonths(3);
                    break;
                case PostageType.OneYear:
                    return dateTime.AddYears(1);
                    break;
            }
            return dateTime;
        }
    }
}