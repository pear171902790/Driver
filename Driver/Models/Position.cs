using System;
using System.ComponentModel.DataAnnotations;

namespace Driver.Models
{
    public class Position
    {
        [Key]
        public Guid Id { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public Guid UploadBy { get; set; }
        public string Voice { get; set; }
        public string Address { get; set; }
        public DateTime UploadTime { get; set; }
    }
}