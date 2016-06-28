using System;

namespace Driver.Models
{
    public class Position
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public Guid UploadBy { get; set; }
        public string Voice { get; set; }
        public string Address { get; set; }
        public string UploaderPhoneNumber { get; set; }
        public string UploaderCarNumber { get; set; }
    }
}