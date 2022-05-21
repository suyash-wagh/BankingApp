namespace BankingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreationV3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transactions", "TransactionType", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transactions", "TransactionType", c => c.Int(nullable: false));
        }
    }
}
