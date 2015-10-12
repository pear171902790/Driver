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

        public ActionResult Test()
        {
            return View();
        }
    }
}