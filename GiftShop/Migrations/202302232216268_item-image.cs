namespace GiftShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itemimage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "ItemHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "PicExtension");
            DropColumn("dbo.Items", "ItemHasPic");
        }
    }
}
