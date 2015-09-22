using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using log4net;
using Newtonsoft.Json;

namespace Driver
{
    public class JsonBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                var request = controllerContext.HttpContext.Request;
                var logger = LogManager.GetLogger(typeof(HttpRequest));
                var sb = new StringBuilder();
                sb.Append("----------\r\n");
                sb.Append("url: " + request.Url.AbsoluteUri+"\r\n");
                sb.Append("method: "+request.HttpMethod+"\r\n");
                sb.Append("data: " + HttpUtility.UrlDecode(request.Form.ToString()) + "\r\n");
                sb.Append("headers: " + HttpUtility.UrlDecode(request.Headers.ToString()) + "\r\n");
                sb.Append("\r\n----------");
                logger.Info(sb.ToString());
                var reader = new StreamReader(request.InputStream);
                string json = reader.ReadToEnd();
                if (string.IsNullOrEmpty(json))
                    return json;
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                var logger = LogManager.GetLogger(typeof(HttpRequest));
                logger.Error("bind model error");
            }
        }
    }
}