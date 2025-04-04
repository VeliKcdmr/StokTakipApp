namespace StokTakipApp.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalShelfAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shelves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShelfName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "ShelfId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "ShelfId");
            AddForeignKey("dbo.Products", "ShelfId", "dbo.Shelves", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ShelfId", "dbo.Shelves");
            DropIndex("dbo.Products", new[] { "ShelfId" });
            DropColumn("dbo.Products", "ShelfId");
            DropTable("dbo.Shelves");
        }
    }
}
