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
                    .Where(c => c.AlbumId == null) // ? canciones que no están en ningún álbum
                    .ToList();
            var model = new HomeViewModel
            {
                Albums = _context.Albums
                    .Include(a => a.Canciones)
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
