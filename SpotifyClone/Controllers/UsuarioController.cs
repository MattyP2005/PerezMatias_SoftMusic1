using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.Data;
using SpotifyClone.Models;
using SpotifyClone.Models.ViewModels;
using System.Security.Claims;

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

        // Vista de inicio de sesión
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Procesa el inicio de sesión
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == model.Password);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Credenciales incorrectas");
                return View(model);
            }

            // Iniciar sesión usando cookie (simple)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim("Rol", usuario.Rol),
                new Claim("Plan", usuario.Plan)
            };

            var identity = new ClaimsIdentity(claims, "login");
            HttpContext.SignInAsync(new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Home");
        }

        // Procesa el cierre de sesión
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
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
                return RedirectToAction("ElegirSubPlan", "Usuario", new { id = usuario.Id });
            }

            return RedirectToAction("Index", "Home");
        }

        // Vista de selección de sub-plan
        [HttpGet]
        public IActionResult ElegirSubPlan(int id)
        {
            var model = new ElegirSubPlanViewModel { UsuarioId = id };
            return View(model);
        }

        // Procesa la selección de sub-plan
        [HttpPost]
        public IActionResult ElegirSubPlan(ElegirSubPlanViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = _context.Usuarios.Find(model.UsuarioId);
            if (usuario == null)
                return NotFound();

            // Guardamos el sub-plan como parte del Plan
            usuario.Plan = "Premium-" + model.SubPlan;
            _context.SaveChanges();

            return RedirectToAction("MetodoPago", "Usuario", new { id = usuario.Id });
        }

        // Vista de métodos de pago
        [HttpGet]
        public IActionResult MetodoPago(int id)
        {
            var model = new MetodoPagoViewModel { UsuarioId = id };
            return View(model);
        }

        // Procesa el método de pago
        [HttpPost]
        public async Task<IActionResult> MetodoPago(MetodoPagoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = _context.Usuarios.Find(model.UsuarioId);
            if (usuario == null)
                return NotFound();

            var formaPago = new FormaPago
            {
                UsuarioId = usuario.Id,
                Tipo = model.TipoTarjeta,
                Detalles = $"**** **** **** {model.NumeroTarjeta[^4..]}", // muestra solo los últimos 4 dígitos
                FechaRegistro = DateTime.Now
            };

            _context.FormasPago.Add(formaPago);
            _context.SaveChanges();

            TempData["Mensaje"] = $"Gracias por suscribirte al plan {usuario.Plan}!";

            // Iniciar sesión automáticamente (si aún no está logueado)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim("Rol", usuario.Rol),
                new Claim("Plan", usuario.Plan),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            };

            var identity = new ClaimsIdentity(claims, "login");
            await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

            return RedirectToAction("MiPerfil", "Usuario");

        }

        // Vista de perfil del usuario
        //[Authorize(Roles = "Usuario,Admin,Artista")]
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
        [Authorize(Roles = "Usuario,Admin,Artista")]
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
                return RedirectToAction("MetodoPago", "Usuario", new { id = usuario.Id });
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
