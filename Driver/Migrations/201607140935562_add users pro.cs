namespace Driver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adduserspro : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PostageTypes", c => c.String());
            AddColumn("dbo.Users", "Amount", c => c.String());
            AddColumn("dbo.Users", "Paychannel", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Paychannel");
            DropColumn("dbo.Users", "Amount");
            DropColumn("dbo.Users", "PostageTypes");
        }
    }
}
