using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.Data;
using SpotifyClone.Models;

namespace SpotifyClone.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ReproduccionApiController : ControllerBase
    {
        private readonly SpotifyDbContext _context;

        public ReproduccionApiController(SpotifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("siguiente/{cancionId}")]
        public IActionResult ObtenerSiguienteCancion(int cancionId)
        {
            var email = User.Identity?.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null) return Unauthorized();

            // 1. Buscar si la canción pertenece a alguna playlist del usuario
            var playlist = _context.PlaylistCanciones
                .Include(pc => pc.Playlist)
                .Where(pc => pc.Playlist.UsuarioId == usuario.Id && pc.CancionId == cancionId)
                .Select(pc => pc.Playlist)
                .FirstOrDefault();

            if (playlist != null)
            {
                // 2. Obtener la lista de canciones ordenadas por ID
                var cancionesEnPlaylist = _context.PlaylistCanciones
                    .Where(pc => pc.PlaylistId == playlist.Id)
                    .OrderBy(pc => pc.CancionId)
                    .ToList();

                // 3. Buscar la siguiente canción
                var posicion = cancionesEnPlaylist.FindIndex(pc => pc.CancionId == cancionId);
                if (posicion < cancionesEnPlaylist.Count - 1)
                {
                    var siguienteId = cancionesEnPlaylist[posicion + 1].CancionId;
                    var siguiente = _context.Canciones.Find(siguienteId);
                    return Ok(siguiente);
                }
            }

            // 4. Si no está en playlist, buscar otra del mismo artista
            var cancionActual = _context.Canciones.Find(cancionId);
            var otraDelArtista = _context.Canciones
                .Where(c => c.UsuarioId == cancionActual.UsuarioId && c.Id != cancionId)
                .FirstOrDefault();

            if (otraDelArtista != null)
                return Ok(otraDelArtista);

            // 5. Si no hay más del artista, devolver una aleatoria
            var aleatoria = _context.Canciones
                .Where(c => c.Id != cancionId)
                .OrderBy(c => Guid.NewGuid())
                .FirstOrDefault();

            return aleatoria != null ? Ok(aleatoria) : NotFound("No hay canciones disponibles.");

        }
    }
}
