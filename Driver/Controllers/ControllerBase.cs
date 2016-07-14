using System.Web;
using System.Web.Mvc;

namespace Driver.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected bool CheckToken(string token)
        {
            return (!string.IsNullOrEmpty(token)) && (HttpRuntime.Cache.Get(token) != null);
        }
    }
}