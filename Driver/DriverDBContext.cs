using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver
{
    public class DriverDBContext : DbContext
    {
        private static DriverDBContext _driverDbContext;
        public DriverDBContext()
            : base("name=DriverDB")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DriverDBContext, Migrations.Configuration>());
        }
        public DbSet<Data> Datas { get; set; }

        public static DriverDBContext Instance
        {
            get { return _driverDbContext ?? (_driverDbContext = new DriverDBContext()); }
        }
    }

    public class Data
    {
        [Key]
        public Guid Key { get; set; }
        public int Type { get; set; }
        public string Value { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Valid { get; set; }
    }

    public enum DataType
    {
        User = 1,
        Position = 2
    }
}
