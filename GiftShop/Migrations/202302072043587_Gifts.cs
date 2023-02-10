namespace GiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gifts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gifts",
                c => new
                    {
                        GiftId = c.Int(nullable: false, identity: true),
                        GiftBasketSize = c.String(),
                        GiftBasketQuantity = c.Int(nullable: false),
                        GiftBasketDetails = c.String(),
                    })
                .PrimaryKey(t => t.GiftId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Gifts");
        }
    }
}
