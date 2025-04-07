namespace StokTakipApp.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLogİnital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ErrorLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogDate = c.DateTime(nullable: false),
                        LogLevel = c.String(nullable: false, maxLength: 50),
                        Message = c.String(nullable: false),
                        Source = c.String(maxLength: 255),
                        TargetSite = c.String(maxLength: 255),
                        StackTrace = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ErrorLogs");
        }
    }
}
