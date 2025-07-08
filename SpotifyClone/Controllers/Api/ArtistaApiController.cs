using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.Data;
using SpotifyClone.Models;
using System.Linq;
using SpotifyClone.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace SpotifyClone.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistaApiController : ControllerBase
    {
        private readonly SpotifyDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ArtistaApiController(SpotifyDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/ArtistaApi
        [HttpGet]
        public IActionResult GetArtistas()
        {
            var artistas = _context.Artistas.ToList();
            return Ok(artistas);
        }

        // GET: api/ArtistaApi/5
        [HttpGet("{id}")]
        public IActionResult GetArtista(int id)
        {
            var artista = _context.Artistas.Find(id);
            if (artista == null)
                return NotFound();

            return Ok(artista);
        }

        // POST: api/ArtistaApi
        [HttpPost]
        public async Task<IActionResult> CrearArtista([FromForm] Artista artista, IFormFile portada)
        {
            if (portada != null)
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

            return Ok(artista);
        }


        // PUT: api/ArtistaApi/5
        [HttpPut("{id}")]
        public IActionResult EditarArtista(int id, [FromBody] Artista artista)
        {
            if (id != artista.Id)
                return BadRequest();

            _context.Entry(artista).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/ArtistaApi/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult EliminarArtista(int id)
        {
            var artista = _context.Artistas
                .Include(a => a.Canciones)
                .FirstOrDefault(a => a.Id == id);

            if (artista == null)
                return NotFound("Artista no encontrado.");

            // 1. Eliminar canciones y archivos físicos
            foreach (var cancion in artista.Canciones)
            {
                // Eliminar historial de esta canción
                var historial = _context.Historiales.Where(h => h.CancionId == cancion.Id);
                _context.Historiales.RemoveRange(historial);

                // Eliminar de playlists
                var enPlaylists = _context.PlaylistCanciones.Where(pc => pc.CancionId == cancion.Id);
                _context.PlaylistCanciones.RemoveRange(enPlaylists);

                // Eliminar archivo físico
                var ruta = Path.Combine(_env.WebRootPath, cancion.Url.TrimStart('/'));
                if (System.IO.File.Exists(ruta))
                    System.IO.File.Delete(ruta);
            }

            _context.Canciones.RemoveRange(artista.Canciones);

            // 2. Eliminar portada del artista (si existe)
            if (!string.IsNullOrWhiteSpace(artista.PortadaUrl))
            {
                var rutaPortada = Path.Combine(_env.WebRootPath, artista.PortadaUrl.TrimStart('/'));
                if (System.IO.File.Exists(rutaPortada))
                    System.IO.File.Delete(rutaPortada);
            }

            // 3. Eliminar artista
            _context.Artistas.Remove(artista);

            _context.SaveChanges();

            return Ok("Artista eliminado junto con sus canciones, álbumes y portada.");
        }
    }
}