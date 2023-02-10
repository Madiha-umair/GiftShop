namespace GiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GiftOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gifts", "OrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.Gifts", "OrderId");
            AddForeignKey("dbo.Gifts", "OrderId", "dbo.Orders", "OrderId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Gifts", "OrderId", "dbo.Orders");
            DropIndex("dbo.Gifts", new[] { "OrderId" });
            DropColumn("dbo.Gifts", "OrderId");
        }
    }
}
