using System.Web.Mvc;

namespace Driver
{
    public class ApiResponse
    {
        public ApiResponse(string code,string errorMessage,string result)
        {
            Code = code;
            ErrorMessage = errorMessage;
            Result = result;
        }

        //回应代码，只有code==200时才表示操作成功
        public string Code { get; set; }

        //错误消息
        public string ErrorMessage { get; set; }

        //回应对象的JSON格式
        public string Result { get; set; }

        public static JsonResult PhoneNumberAlreadySignUp
        {
            get { return new JsonResult() { Data = new ApiResponse("201", "该手机号已经被注册", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }
        public static JsonResult UnknownError
        {
            get { return new JsonResult() { Data = new ApiResponse("300", "未知错误", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }

        public static JsonResult OK(string result="Success")
        {
            return new JsonResult() {Data = new ApiResponse("200", string.Empty, result),JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }
        public static JsonResult UserNotExist
        {
            get { return new JsonResult() { Data = new ApiResponse("202", "用户不存在", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }

        public static JsonResult NotPhoneNumber
        {
            get { return new JsonResult() { Data = new ApiResponse("204", "请用手机号码注册", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }

        public static JsonResult PasswordError
        {
            get { return new JsonResult() { Data = new ApiResponse("203", "密码错误", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }

        public static JsonResult ParameterError
        {
            get { return new JsonResult() { Data = new ApiResponse("205", "参数错误", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }

        public static JsonResult OldPasswordError
        {
            get { return new JsonResult() { Data = new ApiResponse("206", "旧密码错误", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }

        public static ActionResult FileNotExists
        {
            get { return new JsonResult() { Data = new ApiResponse("207", "文件不存在", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }

        public static ActionResult NotSignIn
        {
            get { return new JsonResult() { Data = new ApiResponse("208", "没有登录", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }
    }
}