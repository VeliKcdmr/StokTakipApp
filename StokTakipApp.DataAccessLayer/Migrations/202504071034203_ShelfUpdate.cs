namespace StokTakipApp.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShelfUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shelves", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shelves", "IsActive");
        }
    }
}
