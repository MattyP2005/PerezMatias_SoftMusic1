using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SpotifyClone.Data;
using SpotifyClone.Models;

namespace SpotifyClone.Controllers
{
    [Authorize(Roles = "Artista,Admin")]

    public class AlbumController : Controller
    {
        private readonly SpotifyDbContext _context;

        public AlbumController(SpotifyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var email = User.Identity?.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null) return Unauthorized();

            var albums = _context.Albums
                .Where(a => a.ArtistaId == usuario.Id)
                .Include(a => a.Canciones)
                .ToList();

            return View(albums);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(string titulo, DateTime fechaLanzamiento)
        {
            var email = User.Identity?.Name;
            var artista = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (artista == null) return Unauthorized();

            var album = new Album
            {
                Titulo = titulo,
                FechaLanzamiento = fechaLanzamiento,
                ArtistaId = artista.Id
            };

            _context.Albums.Add(album);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Ver(int id)
        {
            var album = _context.Albums
                .Include(a => a.Canciones)
                .ThenInclude(c => c.Artista)
                .FirstOrDefault(a => a.Id == id);

            if (album == null)
                return NotFound();

            return View(album);
        }

        [HttpGet]
        public IActionResult AgregarCanciones(int id)
        {
            var album = _context.Albums.Find(id);
            if (album == null) return NotFound();

            ViewBag.CancionesDisponibles = _context.Canciones
                .Where(c => c.AlbumId == null)
                .ToList();

            return View(album);
        }

        [HttpPost]
        public IActionResult AgregarCanciones(int albumId, int[] cancionesSeleccionadas)
        {
            var album = _context.Albums.Find(albumId);
            if (album == null) return NotFound();

            foreach (var cancionId in cancionesSeleccionadas)
            {
                var cancion = _context.Canciones.Find(cancionId);
                if (cancion != null)
                {
                    cancion.AlbumId = albumId;
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Ver", new { id = albumId });
        }
    }
}
