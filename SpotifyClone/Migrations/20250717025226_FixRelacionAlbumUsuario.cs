using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotifyClone.Migrations
{
    /// <inheritdoc />
    public partial class FixRelacionAlbumUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Albums",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_UsuarioId",
                table: "Albums",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Usuario_UsuarioId",
                table: "Albums",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Usuario_UsuarioId",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_UsuarioId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Albums");
        }
    }
}
