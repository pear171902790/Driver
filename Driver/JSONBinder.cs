using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Driver
{
    public class JsonBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var reader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            string json = reader.ReadToEnd();
            if (string.IsNullOrEmpty(json))
                return json;
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}