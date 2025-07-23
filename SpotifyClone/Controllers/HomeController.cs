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

                CancionesSueltas = _context.Canciones
                    .Include(c => c.Artista)
                    .Where(c => !c.AlbumCanciones.Any())
                    .OrderByDescending(c => c.FechaSubida)
                    .Take(20) // Limitar a las 20 canciones más recientes
                    .ToList(),
            };

            return View(model);
        }
    }
}
