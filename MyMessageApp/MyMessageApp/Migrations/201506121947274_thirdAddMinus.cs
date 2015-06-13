namespace MyMessageApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirdAddMinus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "minus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "minus");
        }
    }
}
