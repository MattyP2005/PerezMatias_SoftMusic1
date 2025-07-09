using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.Data;
using SpotifyClone.Models;

namespace SpotifyClone.Controllers
{
    //[Authorize(Roles = "Artista,Admin")]

    public class ArtistaController : Controller
    {
        private readonly SpotifyDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ArtistaController(SpotifyDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var artistas = await _context.Artistas.ToListAsync();
            return View(artistas);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Artista artista, IFormFile portada)
        {
            if (portada != null && portada.Length > 0)
            {
                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(portada.FileName);
                var rutaCarpeta = Path.Combine(_env.WebRootPath, "portadas");

                if (!Directory.Exists(rutaCarpeta))
                    Directory.CreateDirectory(rutaCarpeta);

                var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await portada.CopyToAsync(stream);
                }

                artista.PortadaUrl = "/portadas/" + nombreArchivo;
            }

            _context.Artistas.Add(artista);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult SubirPortada(int id)
        {
            var artista = _context.Artistas.FirstOrDefault(a => a.Id == id);
            if (artista == null) return NotFound();

            return View(artista);
        }

        [HttpPost]
        public IActionResult SubirPortada(int id, IFormFile PortadaFile)
        {
            var artista = _context.Artistas.Find(id);
            if (artista == null) return NotFound();

            if (PortadaFile == null || PortadaFile.Length == 0)
                return BadRequest("Archivo inválido.");

            var extension = Path.GetExtension(PortadaFile.FileName).ToLower();
            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                return BadRequest("Solo se permiten imágenes .jpg y .png");

            var nombreArchivo = $"artista_{id}{extension}";
            var ruta = Path.Combine(_env.WebRootPath, "portadas", nombreArchivo);

            using (var stream = new FileStream(ruta, FileMode.Create))
            {
                PortadaFile.CopyTo(stream);
            }

            artista.PortadaUrl = "/portadas/" + nombreArchivo;
            _context.SaveChanges();

            return RedirectToAction("SubirPortada", new { id });
        }   
    }
}
