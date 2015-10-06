using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Driver.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return new ContentResult() {Content = "hello driver"};
        }

        [HttpPut, Route("api/UploadPosition")]
        public ActionResult UploadPosition([ModelBinder(typeof(JsonBinder<UploadPositionRequest>))]UploadPositionRequest uploadPositionRequest)
        {
            try
            {
                var token=Request.Headers["Authorization"];
                var guid=new Guid(token);
                var userData = DriverDBContext.Instance.Datas.SingleOrDefault(x => x.Key == guid);
                if (userData == null)
                {
                    return ApiResponse.UserNotExist;
                }
                var positionData=new Data()
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

        public ActionResult Test()
        {
            return View();
        }
    }
}