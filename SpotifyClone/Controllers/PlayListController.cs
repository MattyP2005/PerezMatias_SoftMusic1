using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.Data;
using SpotifyClone.Models;

namespace SpotifyClone.Controllers
{
    //[Authorize]

    public class PlayListController : Controller
    {
        private readonly SpotifyDbContext _context;

        public PlayListController(SpotifyDbContext context)
        {
            _context = context;
        }

        // GET: Playlists
        public IActionResult Index()
        {
            var playlists = _context.Playlists
                .Include(p => p.Usuario)
                .Include(p => p.Canciones)
                    .ThenInclude(pc => pc.Cancion)
                        .ThenInclude(c => c.Artista)
                .Where(p => p.Usuario.Email == User.Identity.Name)
                .ToList();

            return View(playlists);
        }

        // GET: Crear Playlist
        public IActionResult Crear()
        {
            var userEmail = User.Identity?.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == userEmail);

            if (usuario == null)
                return Unauthorized();

            if (!usuario.Plan.StartsWith("Premium"))
            {
                TempData["Error"] = "Solo los usuarios con plan Premium pueden crear playlists.";
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Crear(string nombre)
        {
            var userEmail = User.Identity?.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == userEmail);
            if (usuario == null) return Unauthorized();

            if (!usuario.Plan.StartsWith("Premium"))
            {
                TempData["Error"] = "Solo los usuarios con plan Premium pueden crear playlists.";
                return RedirectToAction("Index");
            }

            var playlist = new Playlist
            {
                Nombre = nombre,
                UsuarioId = usuario.Id,
                FechaCreacion = DateTime.UtcNow,
                EsPublica = true
            };

            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Agregar Canciones a Playlist
        public IActionResult AgregarCanciones(int id)
        {
            var playlist = _context.Playlists
                .Include(p => p.Canciones)
                .ThenInclude(pc => pc.Cancion)
                .FirstOrDefault(p => p.Id == id);

            if (playlist == null)
                return NotFound();

            ViewBag.CancionesDisponibles = _context.Canciones.ToList();
            return View(playlist);
        }

        // POST: Agregar Canciones a Playlist
        [HttpPost]
        public IActionResult AgregarCanciones(int playlistId, int[] cancionesSeleccionadas)
        {
            foreach (var cancionId in cancionesSeleccionadas)
            {
                var existe = _context.PlaylistCanciones.Any(pc => pc.PlaylistId == playlistId && pc.CancionId == cancionId);
                if (!existe)
                {
                    _context.PlaylistCanciones.Add(new PlaylistCancion
                    {
                        PlaylistId = playlistId,
                        CancionId = cancionId
                    });
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Playlists/Detalles/5
        public async Task<IActionResult> Ver(int? id)
        {
            if (id == null)
                return NotFound();

            var playlist = await _context.Playlists
                .Include(p => p.Usuario)
                .Include(p => p.Canciones)
                    .ThenInclude(pc => pc.Cancion)
                        .ThenInclude(c => c.Artista)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (playlist == null)
                return NotFound();

            return View(playlist);
        }

        // GET: Eliminar Playlist
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            var playlist = _context.Playlists
                .Include(p => p.Canciones)
                .FirstOrDefault(p => p.Id == id);

            if (playlist == null)
                return NotFound();

            // Eliminar relaciones con canciones
            _context.PlaylistCanciones.RemoveRange(playlist.Canciones);

            // Eliminar playlist
            _context.Playlists.Remove(playlist);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: Quitar Canción de Playlist
        [HttpPost]
        public IActionResult QuitarCancion(int playlistId, int cancionId)
        {
            var relacion = _context.PlaylistCanciones
                .FirstOrDefault(pc => pc.PlaylistId == playlistId && pc.CancionId == cancionId);

            if (relacion == null)
                return NotFound();

            _context.PlaylistCanciones.Remove(relacion);
            _context.SaveChanges();

            return RedirectToAction("Ver", new { id = playlistId });
        }
    }
}
