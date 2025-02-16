namespace YemekTarifUgulaması.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deneme1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Malzemelers",
                c => new
                    {
                        MalzemeID = c.Int(nullable: false, identity: true),
                        MalzemeAdi = c.String(),
                        ToplamMiktar = c.String(),
                        MalzemeBirim = c.String(),
                        BirimFiyat = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.MalzemeID);
            
            CreateTable(
                "dbo.Tarif_Malzeme",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TarifID = c.Int(nullable: false),
                        MalzemeID = c.Int(nullable: false),
                        MalzemeMiktar = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Malzemelers", t => t.MalzemeID, cascadeDelete: true)
                .ForeignKey("dbo.Tariflers", t => t.TarifID, cascadeDelete: true)
                .Index(t => t.TarifID)
                .Index(t => t.MalzemeID);
            
            CreateTable(
                "dbo.Tariflers",
                c => new
                    {
                        TarifID = c.Int(nullable: false, identity: true),
                        TarifAdi = c.String(),
                        Kategori = c.String(),
                        HazirlamaSuresi = c.Int(nullable: false),
                        Talimatlar = c.String(),
                    })
                .PrimaryKey(t => t.TarifID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tarif_Malzeme", "TarifID", "dbo.Tariflers");
            DropForeignKey("dbo.Tarif_Malzeme", "MalzemeID", "dbo.Malzemelers");
            DropIndex("dbo.Tarif_Malzeme", new[] { "MalzemeID" });
            DropIndex("dbo.Tarif_Malzeme", new[] { "TarifID" });
            DropTable("dbo.Tariflers");
            DropTable("dbo.Tarif_Malzeme");
            DropTable("dbo.Malzemelers");
        }
    }
}
