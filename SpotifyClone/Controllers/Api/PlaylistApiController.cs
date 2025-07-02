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
    // This controller is for managing playlists in the Spotify Clone application.
    public class PlaylistApiController : ControllerBase
    {
        private readonly SpotifyDbContext _context;

        public PlaylistApiController(SpotifyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CrearPlaylist([FromBody] string nombre)
        {
            var email = User.Identity?.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                return Unauthorized();

            var yaExiste = _context.Playlists.Any(p => p.UsuarioId == usuario.Id && p.Nombre == nombre);
            if (yaExiste)
                return Conflict("Ya tienes una playlist con ese nombre.");

            var playlist = new Playlist
            {
                Nombre = nombre,
                FechaCreacion = DateTime.UtcNow,
                UsuarioId = usuario.Id
            };

            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            return Ok("Playlist creada.");
        }

        [HttpGet]
        public IActionResult ObtenerPlaylists()
        {
            var email = User.Identity?.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                return Unauthorized();

            var playlists = _context.Playlists
                .Where(p => p.UsuarioId == usuario.Id)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    Canciones = p.Canciones.Select(pc => pc.Cancion.Titulo)
                })
                .ToList();

            return Ok(playlists);
        }

        [HttpPost("{playlistId}/agregar")]
        public IActionResult AgregarCancion(int playlistId, [FromBody] int cancionId)
        {
            var email = User.Identity?.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            var playlist = _context.Playlists.FirstOrDefault(p => p.Id == playlistId && p.UsuarioId == usuario.Id);
            if (playlist == null)
                return NotFound("Playlist no encontrada.");

            var yaExiste = _context.PlaylistCanciones.Any(pc => pc.PlaylistId == playlistId && pc.CancionId == cancionId);
            if (yaExiste)
                return Conflict("La canción ya está en la playlist.");

            _context.PlaylistCanciones.Add(new PlaylistCancion
            {
                PlaylistId = playlistId,
                CancionId = cancionId
            });

            _context.SaveChanges();
            return Ok("Canción agregada.");
        }

        [HttpPost("{playlistId}/quitar")]
        public IActionResult QuitarCancion(int playlistId, [FromBody] int cancionId)
        {
            var cancion = _context.PlaylistCanciones.FirstOrDefault(pc =>
                pc.PlaylistId == playlistId && pc.CancionId == cancionId);

            if (cancion == null)
                return NotFound("La canción no está en la playlist.");

            _context.PlaylistCanciones.Remove(cancion);
            _context.SaveChanges();

            return Ok("Canción quitada.");
        }
    }
}
