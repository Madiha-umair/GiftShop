namespace GiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GiftsItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        ItemDescription = c.String(),
                    })
                .PrimaryKey(t => t.ItemId);
            
            CreateTable(
                "dbo.ItemGifts",
                c => new
                    {
                        Item_ItemId = c.Int(nullable: false),
                        Gift_GiftId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Item_ItemId, t.Gift_GiftId })
                .ForeignKey("dbo.Items", t => t.Item_ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Gifts", t => t.Gift_GiftId, cascadeDelete: true)
                .Index(t => t.Item_ItemId)
                .Index(t => t.Gift_GiftId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemGifts", "Gift_GiftId", "dbo.Gifts");
            DropForeignKey("dbo.ItemGifts", "Item_ItemId", "dbo.Items");
            DropIndex("dbo.ItemGifts", new[] { "Gift_GiftId" });
            DropIndex("dbo.ItemGifts", new[] { "Item_ItemId" });
            DropTable("dbo.ItemGifts");
            DropTable("dbo.Items");
        }
    }
}
