using System;
using System.ComponentModel.DataAnnotations;

namespace Driver.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string CarType { get; set; }
        public string CarNumber { get; set; }
        public int Integral { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegsiterTime { get; set; }
        public bool Valid { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string ImageUrl { get; set; }
        public string PostageTypes { get; set; }
        public string Amount { get; set; }
        public string Paychannel { get; set; }

        public SignInResponse ToSignInResponse()
        {
            return new SignInResponse()
            {
                UserId = Id,
                PhoneNumber = PhoneNumber,
                CarNumber = CarNumber,
                CarType = CarType,
                Integral = Integral,
                ExpireDate = ExpirationTime
            };
        }
    }
}