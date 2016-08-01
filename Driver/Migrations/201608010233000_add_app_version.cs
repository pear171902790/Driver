namespace Driver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_app_version : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppVersions",
                c => new
                    {
                        VersionCode = c.Int(nullable: false),
                        VersionName = c.String(),
                        ApkDownloadUrl = c.String(),
                    })
                .PrimaryKey(t => t.VersionCode);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AppVersions");
        }
    }
}
