using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.Data;
using SpotifyClone.Models;
using SpotifyClone.Models.ViewModels;

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
            var userEmail = User.Identity?.Name;

            var albums = _context.Albums
                .Include(a => a.AlbumCanciones)
                    .ThenInclude(ac => ac.Cancion)
                        .ThenInclude(c => c.Artista)
                .Include(a => a.Artista)
                .Where(a => a.Artista.Email == userEmail)
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
            var userEmail = User.Identity?.Name;
            var artista = _context.Usuarios.FirstOrDefault(u => u.Email == userEmail);
            if (artista == null || artista.Rol != "Artista") return Unauthorized();

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
            var userEmail = User.Identity?.Name;
            var album = _context.Albums
                .Include(a => a.AlbumCanciones)
                    .ThenInclude(ac => ac.Cancion)
                        .ThenInclude(c => c.Artista)
                .Include(a => a.Artista)
                .FirstOrDefault(a => a.Id == id && a.Artista.Email == userEmail);

            if (album == null)
                return NotFound();

            return View(album);
        }

        // GET: Album/AgregarCanciones/5
        public IActionResult AgregarCanciones(int id)
        {
            var userEmail = User.Identity?.Name;
            var album = _context.Albums
                .Include(a => a.Artista)
                .FirstOrDefault(a => a.Id == id && a.Artista.Email == userEmail);

            if (album == null)
                return NotFound();

            var canciones = _context.Canciones
                .Where(c => c.Artista.Email == userEmail)
                .ToList();

            var viewModel = new AgregarCancionAlbumViewModel
            {
                AlbumId = album.Id,
                AlbumTitulo = album.Titulo,
                Canciones = canciones.Select(c => new CancionSeleccionada
                {
                    CancionId = c.Id,
                    Titulo = c.Titulo,
                    Seleccionada = false
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Album/AgregarCanciones
        [HttpPost]
        public IActionResult AgregarCanciones(AgregarCancionAlbumViewModel model)
        {
            var userEmail = User.Identity?.Name;
            var album = _context.Albums
                .Include(a => a.Artista)
                .Include(a => a.AlbumCanciones)
                .FirstOrDefault(a => a.Id == model.AlbumId && a.Artista.Email == userEmail);

            if (album == null) return NotFound();

            foreach (var c in model.Canciones.Where(c => c.Seleccionada))
            {
                var yaExiste = _context.AlbumCanciones
                    .Any(ac => ac.AlbumId == album.Id && ac.CancionId == c.CancionId);

                if (!yaExiste)
                {
                    _context.AlbumCanciones.Add(new AlbumCancion
                    {
                        AlbumId = album.Id,
                        CancionId = c.CancionId
                    });
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
