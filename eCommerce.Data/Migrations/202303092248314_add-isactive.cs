namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addisactive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "OrderId", c => c.String());
            DropColumn("dbo.Products", "IsRental");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "IsRental", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "OrderId");
            DropColumn("dbo.Users", "isActive");
        }
    }
}
