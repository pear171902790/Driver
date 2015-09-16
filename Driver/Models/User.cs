﻿using System;

namespace Driver.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string CarType { get; set; }
        public string CarNumber { get; set; }
        public int Integral { get; set; }
        public string PhoneNumber { get; set; }

        public SignInResponse ToSignInResponse()
        {
            return new SignInResponse()
            {
                UserId = Id,
                PhoneNumber = PhoneNumber,
                CarNumber = CarNumber,
                CarType = CarType,
                Integral = Integral
            };
        }
    }
}