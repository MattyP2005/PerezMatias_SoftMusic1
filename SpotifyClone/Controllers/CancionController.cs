using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.Data;
using SpotifyClone.Models;

namespace SpotifyClone.Controllers
{
    [Authorize(Roles = "Artista,Admin")]

    public class CancionController : Controller
    {
        private readonly SpotifyDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CancionController(SpotifyDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Cancion
        [Authorize(Roles = "Artista,Admin")]
        public IActionResult MisCanciones()
        {
            var email = User.Identity?.Name;
            var usuario = _context.Usuarios
                .Include(u => u.Rol) // opcional
                .FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                return Unauthorized();

            var canciones = _context.Canciones
                .Where(c => c.UsuarioId == usuario.Id)
                .OrderByDescending(c => c.FechaSubida)
                .ToList();

            return View(canciones);
        }


        [HttpGet]
        public IActionResult Subir()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Subir(string Titulo, string Genero, IFormFile AudioFile)
        {
            if (AudioFile == null || AudioFile.Length == 0)
                return BadRequest("Debes subir un archivo de audio.");

            // 🔒 Validar extensión del archivo
            var extension = Path.GetExtension(AudioFile.FileName).ToLower();
            if (extension != ".mp3")
                return BadRequest("Solo se permiten archivos .mp3");

            // Guardar archivo con nombre único
            var fileName = Guid.NewGuid().ToString() + extension;
            var path = Path.Combine(_env.WebRootPath, "audio", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                AudioFile.CopyTo(stream);
            }

            // Obtener usuario autenticado
            var email = User.Identity?.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                return Unauthorized();

            var cancion = new Cancion
            {
                Titulo = Titulo,
                Genero = Genero,
                Url = "/audio/" + fileName,
                FechaSubida = DateTime.UtcNow,
                UsuarioId = usuario.Id
            };

            // Validar nombre único (opcional)
            var existe = _context.Canciones.Any(c => c.Titulo == Titulo);
            if (existe)
                return Conflict("Ya existe una canción con ese título.");

            _context.Canciones.Add(cancion);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
