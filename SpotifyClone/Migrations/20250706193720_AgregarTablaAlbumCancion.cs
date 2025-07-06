using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotifyClone.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaAlbumCancion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumCancion_Albums_AlbumId",
                table: "AlbumCancion");

            migrationBuilder.DropForeignKey(
                name: "FK_AlbumCancion_Canciones_CancionId",
                table: "AlbumCancion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlbumCancion",
                table: "AlbumCancion");

            migrationBuilder.RenameTable(
                name: "AlbumCancion",
                newName: "AlbumCanciones");

            migrationBuilder.RenameIndex(
                name: "IX_AlbumCancion_CancionId",
                table: "AlbumCanciones",
                newName: "IX_AlbumCanciones_CancionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlbumCanciones",
                table: "AlbumCanciones",
                columns: new[] { "AlbumId", "CancionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumCanciones_Albums_AlbumId",
                table: "AlbumCanciones",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumCanciones_Canciones_CancionId",
                table: "AlbumCanciones",
                column: "CancionId",
                principalTable: "Canciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumCanciones_Albums_AlbumId",
                table: "AlbumCanciones");

            migrationBuilder.DropForeignKey(
                name: "FK_AlbumCanciones_Canciones_CancionId",
                table: "AlbumCanciones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlbumCanciones",
                table: "AlbumCanciones");

            migrationBuilder.RenameTable(
                name: "AlbumCanciones",
                newName: "AlbumCancion");

            migrationBuilder.RenameIndex(
                name: "IX_AlbumCanciones_CancionId",
                table: "AlbumCancion",
                newName: "IX_AlbumCancion_CancionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlbumCancion",
                table: "AlbumCancion",
                columns: new[] { "AlbumId", "CancionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumCancion_Albums_AlbumId",
                table: "AlbumCancion",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumCancion_Canciones_CancionId",
                table: "AlbumCancion",
                column: "CancionId",
                principalTable: "Canciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
