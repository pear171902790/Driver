using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

                return ApiResponse.OK();
            }
            catch (Exception)
            {
                return ApiResponse.UnknownError;
                throw;
            }
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}