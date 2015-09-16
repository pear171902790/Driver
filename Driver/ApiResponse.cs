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

        //��Ӧ���룬ֻ��code==200ʱ�ű�ʾ�����ɹ�
        public string Code { get; set; }

        //������Ϣ
        public string ErrorMessage { get; set; }

        //��Ӧ�����JSON��ʽ
        public string Result { get; set; }

        public static JsonResult PhoneNumberAlreadySignUp
        {
            get { return new JsonResult() { Data = new ApiResponse("201", "���ֻ����Ѿ���ע��", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }
        public static JsonResult UnknownError
        {
            get { return new JsonResult() { Data = new ApiResponse("300", "δ֪����", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }

        public static JsonResult OK(string result)
        {
            return new JsonResult() {Data = new ApiResponse("200", string.Empty, result),JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }
        public static JsonResult UserNotExist
        {
            get { return new JsonResult() { Data = new ApiResponse("202", "�û�������", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }

        public static JsonResult PasswordError
        {
            get { return new JsonResult() { Data = new ApiResponse("203", "�������", string.Empty), JsonRequestBehavior = JsonRequestBehavior.AllowGet }; }
        }
    }
}