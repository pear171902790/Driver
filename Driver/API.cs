using System;
using System.Collections.Generic;
using Driver.Models;

namespace Driver
{
    //请求地址：http://114.215.157.116/api/SignUp
    //请求方式：HttpPut
    //请求类型：将密码MD5加密然后将对象序列化成json
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
    //回应类型 Guid


    //请求地址：http://114.215.157.116/api/SignIn/{base64}
    //请求方式：HttpGet
    //请求类型：将密码MD5加密然后将对象序列化成json,然后再将整个json字符串base64附在url后
    public class SignInRequest
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
    //回应类型：
    public class SignInResponse
    {
        public Guid UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string CarType { get; set; }
        public string CarNumber { get; set; }
        public int Integral { get; set; }
    }

    //请求地址：http://114.215.157.116/api/UploadPosition
    //请求方式：HttpPut
    //请求类型：
    public class UploadPositionRequest
    {
        public string Voice { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Address { get; set; }
    }
    //返回类型 bool

    //请求地址：http://114.215.157.116/api/Positions
    //请求方式：HttpGet
    //请求类型：
    public class GetPositionsRequest
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
    //返回类型
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
    //请求类型：
    public class UpdateUserInfoRequest
    {
        public string PhoneNumber { get; set; }
        public string CarType { get; set; }
        public string CarNumber { get; set; }
    }
    //回应类型 bool

    //请求地址：http://114.215.157.116/api/ChangePassword
    //请求方式：HttpPost
    //请求类型：需md5加密后序列化
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    //回应类型 bool

    //请求地址：http://114.215.157.116/api/version
    //请求方式：HttpGet
    //请求类型：
    public class GetVersionRequest
    {
        public string VersionNumber { get; set; }
    }
    //回应类型 
    public class GetVersionResponse
    {
        public string LatestVersion { get; set; }
        public string DownloadUrl { get; set; }
    }

    //请求地址：http://114.215.157.116/api/share
    //请求方式：HttpPut
    //请求类型：
    public class ShareRequest
    {
        public string ShareType { get; set; }
    }
    //回应类型 bool
}