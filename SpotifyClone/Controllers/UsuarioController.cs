using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.Data;
using SpotifyClone.Models;
using SpotifyClone.Models.ViewModels;

namespace SpotifyClone.Controllers
{
    //[Authorize]
    public class UsuarioController : Controller
    {
        private readonly SpotifyDbContext _context;

        public UsuarioController(SpotifyDbContext context)
        {
            _context = context;
        }

        // Vista de registro
        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        // Procesa el registro
        [HttpPost]
        public IActionResult Registrar(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                Email = model.Email,
                PasswordHash = model.Password, 
                ImagenPerfilUrl = "/img/sin-perfil.png",
                Plan = model.Plan ?? "Gratis",
                Rol = "Usuario" // Asignar rol por defecto
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            if (model.Plan == "Premium")
            {
                return RedirectToAction("MetodoPago", "Suscripcion", new { id = usuario.Id });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult MetodoPago(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null || usuario.Plan != "Premium")
            {
                return RedirectToAction("Registrar", "Usuario");
            }

            ViewBag.UsuarioId = id;
            return View();
        }

        [HttpPost]
        public IActionResult MetodoPago(FormaPago formaPago)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UsuarioId = formaPago.UsuarioId;
                return View(formaPago);
            }

            _context.FormasPago.Add(formaPago);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // Vista de perfil del usuario
        public IActionResult MiPerfil()
        {
            var email = User.Identity?.Name;

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var usuario = _context.Usuarios
                .Include(u => u.FormaPago)
                .Include(u => u.Playlists)
                .Include(u => u.Historial)
                    .ThenInclude(h => h.Cancion)
                        .ThenInclude(c => c.Artista)
                .FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // Cambiar plan de suscripción
        [HttpGet]
        public IActionResult CambiarPlan()
        {
            var email = User.Identity?.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                return NotFound();

            if (usuario.Plan == "Gratis")
            {
                usuario.Plan = "Premium";
                _context.SaveChanges();
                return RedirectToAction("MetodoPago", "Suscripcion", new { id = usuario.Id });
            }
            else
            {
                // Si tenía una forma de pago, la eliminamos al pasar a Gratis
                var pago = _context.FormasPago.FirstOrDefault(p => p.UsuarioId == usuario.Id);
                if (pago != null)
                {
                    _context.FormasPago.Remove(pago);
                }

                usuario.Plan = "Gratis";
                _context.SaveChanges();
                return RedirectToAction("MiPerfil");
            }
        }

    }
}
