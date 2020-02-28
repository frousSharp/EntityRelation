using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityRelationAPI.DataLayer.Migrations
{
    public partial class AddEntityCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entities_Entities_ParentId",
                table: "Entities");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_Entities_ParentId",
                table: "Entities",
                column: "ParentId",
                principalTable: "Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entities_Entities_ParentId",
                table: "Entities");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_Entities_ParentId",
                table: "Entities",
                column: "ParentId",
                principalTable: "Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
