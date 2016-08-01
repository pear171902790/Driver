using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.Models;

namespace Driver
{
    public class DriverDBContext : DbContext
    {
        public DriverDBContext()
            : base("name=DriverDB")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DriverDBContext, Migrations.Configuration>());
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<AppVersion> AppVersions { get; set; }
    }
}
