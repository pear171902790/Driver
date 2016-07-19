using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Driver.Models
{
    public class VoiceViewModel
    {
        public string CarNumber { get; set; }
        public string Source { get; set; }

        public DateTime UploadTime { get; set; }
    }
}