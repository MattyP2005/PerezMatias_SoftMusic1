using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotifyClone.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToArtista : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Artistas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Artistas");
        }
    }
}
