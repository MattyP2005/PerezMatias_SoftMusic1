using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotifyClone.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Artistas_ArtistaId1",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Usuarios_ArtistaId",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_ArtistaId1",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "ArtistaId1",
                table: "Albums");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Usuarios_ArtistaId",
                table: "Albums",
                column: "ArtistaId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Usuarios_ArtistaId",
                table: "Albums");

            migrationBuilder.AddColumn<int>(
                name: "ArtistaId1",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ArtistaId1",
                table: "Albums",
                column: "ArtistaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Artistas_ArtistaId1",
                table: "Albums",
                column: "ArtistaId1",
                principalTable: "Artistas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Usuarios_ArtistaId",
                table: "Albums",
                column: "ArtistaId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
