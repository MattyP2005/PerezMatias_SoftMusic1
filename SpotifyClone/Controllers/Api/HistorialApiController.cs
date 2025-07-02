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

    public class HistorialApiController : ControllerBase
    {
        private readonly SpotifyDbContext _context;

        public HistorialApiController(SpotifyDbContext context)
        {
            _context = context;
        }

        [HttpPost("{cancionId}")]
        public IActionResult RegistrarReproduccion(int cancionId)
        {
            var email = User.Identity?.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                return Unauthorized();

            var cancion = _context.Canciones.Find(cancionId);
            if (cancion == null)
                return NotFound("Canción no encontrada.");

            var registro = new Historial
            {
                UsuarioId = usuario.Id,
                CancionId = cancionId,
                FechaHora = DateTime.UtcNow
            };

            _context.Historiales.Add(registro);
            _context.SaveChanges();

            return Ok("Reproducción registrada.");
        }

        [HttpGet]
        public IActionResult VerHistorial()
        {
            var email = User.Identity?.Name;
            var usuario = _context.Usuarios
                .Include(u => u.Historial)
                .ThenInclude(h => h.Cancion)
                .FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                return Unauthorized();

            var historial = usuario.Historial
                .OrderByDescending(h => h.FechaHora)
                .Select(h => new
                {
                    Cancion = h.Cancion.Titulo,
                    h.FechaHora
                }).ToList();

            return Ok(historial);
        }
    }
}
