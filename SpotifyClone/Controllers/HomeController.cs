using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.Data;
using SpotifyClone.Models.ViewModels;

namespace SpotifyClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly SpotifyDbContext _context;

        public HomeController(SpotifyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cancionesSueltas = _context.Canciones
                .Include(c => c.Artista)
                .Where(c => !_context.AlbumCanciones.Any(ac => ac.CancionId == c.Id))
                .ToList();

            var model = new HomeViewModel
            {
                Albums = _context.Albums
                    .Include(a => a.AlbumCanciones)
                        .ThenInclude(ac => ac.Cancion)
                            .ThenInclude(c => c.Artista)
                    .ToList(),

                PlaylistsPublicas = _context.Playlists
                    .Where(p => p.EsPublica)
                    .Include(p => p.Canciones)
                        .ThenInclude(pc => pc.Cancion)
                            .ThenInclude(c => c.Artista)
                    .ToList(),

                CancionesSueltas = cancionesSueltas
            };

            return View(model);
        }
    }
}
