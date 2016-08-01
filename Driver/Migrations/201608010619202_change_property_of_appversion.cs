namespace Driver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_property_of_appversion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppVersions", "FileName", c => c.String());
            DropColumn("dbo.AppVersions", "ApkDownloadUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppVersions", "ApkDownloadUrl", c => c.String());
            DropColumn("dbo.AppVersions", "FileName");
        }
    }
}
