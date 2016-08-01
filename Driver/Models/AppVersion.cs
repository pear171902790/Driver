using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Driver.Models
{
    public class AppVersion
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VersionCode { get; set; }

        public string VersionName { get; set; }

        public string FileName { get; set; }
    }
}