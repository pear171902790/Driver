namespace Driver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Data",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        Value = c.String(),
                        PhoneNumber = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        Valid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Data");
        }
    }
}
