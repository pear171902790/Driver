using System;

namespace Driver.Models
{
    public class Position
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid UploadBy { get; set; }
        public string Voice { get; set; }
        public string Address { get; set; }
    }
}