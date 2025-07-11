using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotifyClone.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Usuarios_ArtistaId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Canciones_Usuarios_UsuarioId",
                table: "Canciones");

            migrationBuilder.DropForeignKey(
                name: "FK_FormasPago_Usuarios_UsuarioId",
                table: "FormasPago");

            migrationBuilder.DropForeignKey(
                name: "FK_FormasPago_Usuarios_UsuarioId1",
                table: "FormasPago");

            migrationBuilder.DropForeignKey(
                name: "FK_Historiales_Usuarios_UsuarioId",
                table: "Historiales");

            migrationBuilder.DropForeignKey(
                name: "FK_Historiales_Usuarios_UsuarioId1",
                table: "Historiales");

            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Usuarios_UsuarioId",
                table: "Playlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Usuarios_UsuarioId1",
                table: "Playlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                newName: "Usuario");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId2",
                table: "FormasPago",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FormasPago_UsuarioId2",
                table: "FormasPago",
                column: "UsuarioId2",
                unique: true,
                filter: "[UsuarioId2] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Usuario_ArtistaId",
                table: "Albums",
                column: "ArtistaId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Canciones_Usuario_UsuarioId",
                table: "Canciones",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormasPago_Usuario_UsuarioId",
                table: "FormasPago",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormasPago_Usuario_UsuarioId1",
                table: "FormasPago",
                column: "UsuarioId1",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormasPago_Usuario_UsuarioId2",
                table: "FormasPago",
                column: "UsuarioId2",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Historiales_Usuario_UsuarioId",
                table: "Historiales",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Historiales_Usuario_UsuarioId1",
                table: "Historiales",
                column: "UsuarioId1",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Usuario_UsuarioId",
                table: "Playlists",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Usuario_UsuarioId1",
                table: "Playlists",
                column: "UsuarioId1",
                principalTable: "Usuario",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Usuario_ArtistaId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Canciones_Usuario_UsuarioId",
                table: "Canciones");

            migrationBuilder.DropForeignKey(
                name: "FK_FormasPago_Usuario_UsuarioId",
                table: "FormasPago");

            migrationBuilder.DropForeignKey(
                name: "FK_FormasPago_Usuario_UsuarioId1",
                table: "FormasPago");

            migrationBuilder.DropForeignKey(
                name: "FK_FormasPago_Usuario_UsuarioId2",
                table: "FormasPago");

            migrationBuilder.DropForeignKey(
                name: "FK_Historiales_Usuario_UsuarioId",
                table: "Historiales");

            migrationBuilder.DropForeignKey(
                name: "FK_Historiales_Usuario_UsuarioId1",
                table: "Historiales");

            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Usuario_UsuarioId",
                table: "Playlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Usuario_UsuarioId1",
                table: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_FormasPago_UsuarioId2",
                table: "FormasPago");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "UsuarioId2",
                table: "FormasPago");

            migrationBuilder.RenameTable(
                name: "Usuario",
                newName: "Usuarios");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Usuarios_ArtistaId",
                table: "Albums",
                column: "ArtistaId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Canciones_Usuarios_UsuarioId",
                table: "Canciones",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormasPago_Usuarios_UsuarioId",
                table: "FormasPago",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormasPago_Usuarios_UsuarioId1",
                table: "FormasPago",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Historiales_Usuarios_UsuarioId",
                table: "Historiales",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Historiales_Usuarios_UsuarioId1",
                table: "Historiales",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Usuarios_UsuarioId",
                table: "Playlists",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Usuarios_UsuarioId1",
                table: "Playlists",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
