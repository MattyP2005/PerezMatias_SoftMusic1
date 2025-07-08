using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotifyClone.Migrations
{
    /// <inheritdoc />
    public partial class FixAlbumArtistaForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Artistas_ArtistaId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Artistas_ArtistaId1",
                table: "Albums");

            migrationBuilder.AlterColumn<int>(
                name: "ArtistaId1",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Artistas_ArtistaId1",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Usuarios_ArtistaId",
                table: "Albums");

            migrationBuilder.AlterColumn<int>(
                name: "ArtistaId1",
                table: "Albums",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Artistas_ArtistaId",
                table: "Albums",
                column: "ArtistaId",
                principalTable: "Artistas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Artistas_ArtistaId1",
                table: "Albums",
                column: "ArtistaId1",
                principalTable: "Artistas",
                principalColumn: "Id");
        }
    }
}
