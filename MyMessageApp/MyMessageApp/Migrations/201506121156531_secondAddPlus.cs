namespace MyMessageApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secondAddPlus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "plus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "plus");
        }
    }
}
