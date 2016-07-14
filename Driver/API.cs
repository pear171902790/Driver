using System;
using System.Collections.Generic;
using Driver.Models;

namespace Driver
{
    public class SignUpRequest
    {
        public string PhoneNumber { get; set; }
        public string CarType { get; set; }
        public string CarNumber { get; set; }
        public string Password { get; set; }
    }

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
        public DateTime ExpireDate { get; set; }
    }

    
    public class UploadPositionRequest
    {
        public string Voice { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Address { get; set; }
    }
    

    public class PayRequest
    {
        public int PostageType { get; set; }
        public string Paychannel { get; set; }
        public double Amount { get; set; }
    }


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

    
    public class UpdateUserInfoRequest
    {
        public string PhoneNumber { get; set; }
        public string CarType { get; set; }
        public string CarNumber { get; set; }
    }


    
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

   
    public class GetVersionResponse
    {
        public string LatestVersion { get; set; }
        public string DownloadUrl { get; set; }
    }

    
    public class ShareRequest
    {
        public string ShareType { get; set; }
    }
}