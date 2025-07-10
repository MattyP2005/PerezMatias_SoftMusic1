using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.Data;

namespace SpotifyClone.Controllers
{
    //[Authorize]
    public class UsuarioController : Controller
    {
        private readonly SpotifyDbContext _context;

        public UsuarioController(SpotifyDbContext context)
        {
            _context = context;
        }

        public IActionResult MiPerfil()
        {
            var email = User.Identity?.Name;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var usuario = _context.Usuarios
                .Include(u => u.Playlists)
                .Include(u => u.Historial)
                    .ThenInclude(h => h.Cancion)
                        .ThenInclude(c => c.Artista)
                .FirstOrDefault(u => u.Email == email);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }
    }
}
