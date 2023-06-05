namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfeatures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FeatureValues",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        FeatureID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Features", t => t.FeatureID, cascadeDelete: true)
                .Index(t => t.FeatureID);
            
            CreateTable(
                "dbo.Features",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FeatureName = c.String(),
                        Type = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.ProductSpecifications", "FeatureValueID", c => c.Int(nullable: false));
            CreateIndex("dbo.ProductSpecifications", "FeatureValueID");
            AddForeignKey("dbo.ProductSpecifications", "FeatureValueID", "dbo.FeatureValues", "ID", cascadeDelete: true);
            DropColumn("dbo.ProductSpecifications", "Title");
            DropColumn("dbo.ProductSpecifications", "Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductSpecifications", "Value", c => c.String());
            AddColumn("dbo.ProductSpecifications", "Title", c => c.String());
            DropForeignKey("dbo.ProductSpecifications", "FeatureValueID", "dbo.FeatureValues");
            DropForeignKey("dbo.FeatureValues", "FeatureID", "dbo.Features");
            DropIndex("dbo.FeatureValues", new[] { "FeatureID" });
            DropIndex("dbo.ProductSpecifications", new[] { "FeatureValueID" });
            DropColumn("dbo.ProductSpecifications", "FeatureValueID");
            DropTable("dbo.Features");
            DropTable("dbo.FeatureValues");
        }
    }
}
