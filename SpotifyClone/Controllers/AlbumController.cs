using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.Data;
using SpotifyClone.Models;
using SpotifyClone.Models.ViewModels;

namespace SpotifyClone.Controllers
{
    //[Authorize(Roles = "Artista,Admin")]

    public class AlbumController : Controller
    {
        private readonly SpotifyDbContext _context;

        public AlbumController(SpotifyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var albums = _context.Albums
                .Include(a => a.AlbumCanciones)
                    .ThenInclude(ac => ac.Cancion)
                        .ThenInclude(c => c.Artista) 
                .Include(a => a.Artista)
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
                .Include(a => a.AlbumCanciones)
                .ThenInclude(ac => ac.Cancion)
                    .ThenInclude(c => c.Artista)
                .FirstOrDefault(a => a.Id == id);

            if (album == null)
                return NotFound();

            return View(album);
        }

        // GET: Album/AgregarCanciones/5
        public IActionResult AgregarCanciones(int id)
        {
            var album = _context.Albums.Find(id);
            if (album == null) return NotFound();

            var canciones = _context.Canciones.ToList();

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
            var album = _context.Albums
                .Include(a => a.AlbumCanciones)
                .FirstOrDefault(a => a.Id == model.AlbumId);

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
