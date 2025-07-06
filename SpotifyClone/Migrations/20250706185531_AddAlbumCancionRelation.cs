using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotifyClone.Migrations
{
    /// <inheritdoc />
    public partial class AddAlbumCancionRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Canciones_Albums_AlbumId",
                table: "Canciones");

            migrationBuilder.CreateTable(
                name: "AlbumCancion",
                columns: table => new
                {
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    CancionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumCancion", x => new { x.AlbumId, x.CancionId });
                    table.ForeignKey(
                        name: "FK_AlbumCancion_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlbumCancion_Canciones_CancionId",
                        column: x => x.CancionId,
                        principalTable: "Canciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumCancion_CancionId",
                table: "AlbumCancion",
                column: "CancionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Canciones_Albums_AlbumId",
                table: "Canciones",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Canciones_Albums_AlbumId",
                table: "Canciones");

            migrationBuilder.DropTable(
                name: "AlbumCancion");

            migrationBuilder.AddForeignKey(
                name: "FK_Canciones_Albums_AlbumId",
                table: "Canciones",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
