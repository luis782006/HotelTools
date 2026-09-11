using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelTools.Migrations
{
    public partial class AddFK_Huespedes_Habitaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT * FROM sys.foreign_keys 
                    WHERE object_id = OBJECT_ID(N'[Quejas].[FK_Huespedes_Habitaciones]') 
                    AND parent_object_id = OBJECT_ID(N'[Quejas].[Huespedes]')
                )
                BEGIN
                    ALTER TABLE [Quejas].[Huespedes] WITH CHECK ADD CONSTRAINT [FK_Huespedes_Habitaciones] 
                    FOREIGN KEY([ID_NroHab]) REFERENCES [General].[Habitaciones] ([ID_NroHab])
                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE [Quejas].[Huespedes] DROP CONSTRAINT [FK_Huespedes_Habitaciones]");
        }
    }
}
