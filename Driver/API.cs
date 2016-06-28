using System;
using System.Collections.Generic;
using Driver.Models;

namespace Driver
{
    //请求地址：http://114.215.157.116/api/SignUp
    //请求方式：HttpPost
    //参数示例：{"PhoneNumber":"","CarType":"","CarNumber":"","Password":"密码需要md5加密"}
    //返回数据示例：{"Code":"200","ErrorMessage":"","Result":"Success"}
    public class SignUpRequest
    {
        public string PhoneNumber { get; set; }
        public string CarType { get; set; }
        public string CarNumber { get; set; }
        public string Password { get; set; }

        public User ToUser(Guid id)
        {
            return new User()
            {
                Id = id,
                CarNumber = CarNumber,
                CarType = CarType,
                Password = Password,
                Integral = 0,
                PhoneNumber = PhoneNumber
            };
        }
    }

    //请求地址：http://114.215.157.116/api/SignIn
    //请求方式：HttpPost
    //请求参数示例：{"PhoneNumber":"","Password":"密码需要md5加密"}
    //返回数据示例：{"Code":"200","ErrorMessage":"","Result":"{'UserId':'','PhoneNumber':'','CarType':'','CarNumber':'','Integral':''}"}
    //注意返回的响应头里有一个“X-Token”(X-Token:"xxx"),需要在后续的请求中将这个token值带在请求头里(Token:"xxx")，否则视为没有登陆
    public class SignInRequest
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
    public class SignInResponse
    {
        public Guid UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string CarType { get; set; }
        public string CarNumber { get; set; }
        public int Integral { get; set; }
    }

    //请求地址：http://114.215.157.116/api/UploadPosition
    //请求方式：HttpPost
    //请求参数示例：{"Voice":"","Longitude":"","Latitude":"","Address":"","CarNumber":""}
    //返回数据示例：{"Code":"200","ErrorMessage":"","Result":"Success"}
    public class UploadPositionRequest
    {
        public string Voice { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Address { get; set; }
        public string CarNumber { get; set; }
    }

    //请求地址：http://114.215.157.116/api/Positions
    //请求方式：HttpGet
    //返回数据示例：{"Code":"200","ErrorMessage":"","Result":"{'Count':2,'Positions':[{'Longitude':'','Latitude':'','Address':''},{'Longitude':'','Latitude':'','Address':''}]}"}
    public class GetPositionsResponse
    {
        public int Count
        {
            get { return Positions.Count; }
        }

        public List<Position> Positions { get; set; }

        public class Position
        {
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public string Address { get; set; }
        }
    }

    //请求地址：http://114.215.157.116/api/UpdateUserInfo
    //请求方式：HttpPost
    //请求参数示例：{"PhoneNumber":"","CarType":"","CarNumber":""}
    //返回数据示例：{"Code":"200","ErrorMessage":"","Result":"Success"}
    public class UpdateUserInfoRequest
    {
        public string PhoneNumber { get; set; }
        public string CarType { get; set; }
        public string CarNumber { get; set; }
    }


    //请求地址：http://114.215.157.116/api/ChangePassword
    //请求方式：HttpPost
    //请求参数示例：{"OldPassword":"","NewPassword":""}
    //返回数据示例：{"Code":"200","ErrorMessage":"","Result":"Success"}
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    //请求地址：http://114.215.157.116/api/version
    //请求方式：HttpGet
    //返回数据示例：{"Code":"200","ErrorMessage":"","Result":"{'LatestVersion':'','DownloadUrl':''}"}
    public class GetVersionResponse
    {
        public string LatestVersion { get; set; }
        public string DownloadUrl { get; set; }
    }

    //请求地址：http://114.215.157.116/api/share
    //请求方式：HttpPost
    //请求参数示例：{"ShareType":"微信"}
    //返回数据示例：{"Code":"200","ErrorMessage":"","Result":"Success"}
    public class ShareRequest
    {
        public string ShareType { get; set; }
    }
}