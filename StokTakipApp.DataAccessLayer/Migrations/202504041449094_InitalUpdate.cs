namespace StokTakipApp.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Description = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Models",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Description = c.String(maxLength: 500),
                        Year = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        BrandId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: true)
                .Index(t => t.BrandId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Barcode = c.String(maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StockQuantity = c.Int(nullable: false),
                        Description = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        ModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Models", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ModelId", "dbo.Models");
            DropForeignKey("dbo.Models", "BrandId", "dbo.Brands");
            DropForeignKey("dbo.Brands", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "ModelId" });
            DropIndex("dbo.Models", new[] { "BrandId" });
            DropIndex("dbo.Brands", new[] { "CategoryId" });
            DropTable("dbo.Products");
            DropTable("dbo.Models");
            DropTable("dbo.Brands");
        }
    }
}
